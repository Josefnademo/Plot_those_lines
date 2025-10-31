using OpenTK.Graphics.ES10;
using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;


namespace PTL_Crypto
{
    /// <summary>
    /// Main form of the application. Manages UI, coordinates API calls, file loading,
    /// plotting, and application state persistence.
    /// </summary>
    public partial class Form1 : Form
    {
        // --- Instances of helper classes ---
        private readonly ApiClient _apiClient = new ApiClient();        // For API requests
        private readonly PlotManager _plotManager = new PlotManager();  // For plotting crypto graphs
        private readonly FileClient _fileClient = new FileClient();     // For reading JSON files

        // --- Data collections ---
        // ! HashSet(This is a collection of unique elements.) is simpler because we only need contains/not contains, without extra bools(as in Dictionary).
        private readonly Dictionary<string, List<CryptoPrice>> loadedCryptos = new(); // All loaded crypto price data
        private readonly HashSet<string> visibleCryptos = new();                      // Currently visible cryptos on the chart

        public Form1()
        {
            InitializeComponent();
            // Important: explicitly attach the FormClosing event
            this.FormClosing += Form1_FormClosing;
        }

        /// <summary>
        /// On form load: attempts to restore application state, loads available coin list (API fallback to local),
        /// sets up ComboBox and default graphs, and restores visibility state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_Load(object sender, EventArgs e)
        {
            // Restore previously saved state first 
            LoadAppState();

            List<CoinInfo> coins;
            try
            {
                // Try via API
                coins = await _apiClient.GetCoinsListAsync();
                await Task.Delay(2000); // Small delay after API call to avoid 429
            }
            catch
            {
                // If Api isn't available  - loading local files
                coins = new List<CoinInfo>
                {
                      new CoinInfo { Id = "btc", Name = "Bitcoin", Symbol = "BTC" },
                      new CoinInfo { Id = "eth", Name = "Ethereum", Symbol = "ETH" },
                      new CoinInfo { Id = "sol", Name = "Solana", Symbol = "SOL" },
                      new CoinInfo { Id = "pepe", Name = "PepeCoin", Symbol = "PEPE" }
                };
            }

            // --- Filter invalid entries using LINQ ---
            coins = coins.Where(c => !string.IsNullOrWhiteSpace(c.Id) &&
                                     !string.IsNullOrWhiteSpace(c.Symbol) &&
                                     !string.IsNullOrWhiteSpace(c.Name))
                         .ToList();

            if (coins.Count == 0)
            {
                MessageBox.Show("La liste des pièces est vide");
                return;
            }

            // Binding the ComboBox data source
            comboBoxCoins.DataSource = coins;
            comboBoxCoins.DisplayMember = "Name"; // what to show
            comboBoxCoins.ValueMember = "Id";     // Id for API

            // --- Enable autocompletion for better UX ---
            comboBoxCoins.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxCoins.AutoCompleteSource = AutoCompleteSource.ListItems;

            // Load a default graph on startup
            await LoadDefaultGraph();

            // Restore previously saved app state (selected cryptos, visibility, etc.)
            await LoadAppState();
        }

        /// <summary>
        /// Loads a default graph for a selected coin from local files (Loads local JSON files)
        /// </summary>
        private async Task LoadDefaultGraph()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "local_data");

            var allPrices =
                (comboBoxCoins.DataSource as List<CoinInfo>)
                .Select(coin => new
                {
                    Coin = coin,
                    FileName = coin.Id switch
                    {
                        "btc" => "btc_7days.json",
                        "eth" => "eth_7days.json",
                        "sol" => "solana_7days.json",
                        "pepe" => "pepe_7days.json",
                        _ => null
                    }
                })
                .Where(x => x.FileName != null)
                .Select(x => new
                {
                    x.Coin,
                    Path = Path.Combine(basePath, x.FileName)
                })
                .Where(x => File.Exists(x.Path))
                .ToDictionary(
                    x => x.Coin.Symbol,
                    x => _fileClient.LoadPricesFromFile(x.Path)
                );

            if (allPrices.Any())
                _plotManager.PlotData(formsPlot1, allPrices);
            else
                MessageBox.Show("There are no local files available to plot the graph");
        }



        /// <summary>
        /// Load or add a cryptocurrency series by ID. Tries import file, then API, then local fallback.
        /// Saves loaded data into memory, marks it visible and updates UI controls and chart.
        /// </summary>
        /// <param name="coinId">Coin identifier for API/local file lookup.</param>
        /// <param name="coinName">Human-readable coin name.</param>
        /// <param name="coinSymbol">Ticker symbol (e.g., BTC).</param>
        /// <param name="days">Number of days of data to request or load.</param>
        /// <param name="importFile">Optional path to a JSON file to import.</param>
        /// <returns></returns>
        private async Task LoadOrAddCrypto(string coinId, string coinName, string coinSymbol, int days = 7, string importFile = null)
        {
            List<CryptoPrice> prices;

            // 1️ Try load from import
            if (importFile != null)
            {
                prices = _fileClient.LoadPricesFromFile(importFile);
            }
            else
            {
                // 2️ Try API
                try
                { 
                    await Task.Delay(3000);// Delay 3 sec before the request to reduce 429 errors
                    prices = await _apiClient.GetCryptoPricesAsync(coinId, days);
                }
                catch
                {
                    // 3️Fallback to local JSON
                    string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "local_data");
                    string fileName = coinId switch
                    {
                        "btc" => "btc_7days.json",
                        "eth" => "eth_7days.json",
                        "sol" => "solana_7days.json",
                        "pepe" => "pepe_7days.json",
                        _ => null
                    };

                    prices = fileName != null
                        ? _fileClient.LoadPricesFromFile(Path.Combine(basePath, fileName))
                        : new List<CryptoPrice>();
                }
            }

            if (prices.Count == 0)
            {
                MessageBox.Show($"No data found for {coinName} ({coinSymbol})");
                return;
            }

            string key = coinSymbol.ToUpper();

            // Save and mark as visible
            loadedCryptos[key] = prices;
            visibleCryptos.Add(key);

            // Update CheckedListBox
            if (!clbCryptos.Items.Contains(key))
                clbCryptos.Items.Add(key, true);

            // Update chart
            UpdatePlot();
        }

        /// <summary>
        /// Rebuilds the plot based on currently visible cryptos (visibleCryptos HashSet).
        /// Clears the plot and uses PlotManager to draw selected series.
        /// </summary>
        private void UpdatePlot()
        {
            formsPlot1.Plot.Clear();

            // We only take those cryptos that are in visibleCryptos
            var toPlot = loadedCryptos
                .Where(kvp => visibleCryptos.Contains(kvp.Key))  // filter by HashSet
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value); // putting it back into the Dictionary

            if (toPlot.Any())
                _plotManager.PlotData(formsPlot1, toPlot);

            formsPlot1.Refresh();
        }

        /// <summary>
        /// Handles CheckedListBox ItemCheck to add/remove symbols to visibility set and trigger plot update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxCryptos_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string symbol = clbCryptos.Items[e.Index].ToString();

            // Update visibility according to the new state
            if (e.NewValue == CheckState.Checked)
                visibleCryptos.Add(symbol);
            else
                visibleCryptos.Remove(symbol);

            // Delay update until checkbox state actually changes
            UpdatePlot();
        }

        /// <summary>
        /// Opens a file dialog to import a JSON file. Extracts name/symbol from filename and loads that file.
        /// </summary>>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportJson_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON files (*.json)|*.json";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                string name = Path.GetFileNameWithoutExtension(path);
                string symbol = name.Substring(0, Math.Min(4, name.Length)).ToUpper();
                _ = LoadOrAddCrypto(symbol.ToLower(), name, symbol, importFile: path);
            }
        }

        /// <summary>
        /// Loads crypto data for selected coin in the ComboBox for given days (1,7,30,365).
        /// </summary>
        /// <param name="days">Days of data to load.</param>
        /// <returns></returns>
        private async Task LoadCryptoData(int days)
        {
            if (comboBoxCoins.SelectedItem is not CoinInfo selectedCoin)
            {
                MessageBox.Show("Sélectionnez une crypto monnaie");
                return;
            }
            await LoadOrAddCrypto(selectedCoin.Id, selectedCoin.Name, selectedCoin.Symbol, days);
        }

        private async void btn1Day_Click(object sender, EventArgs e) // 1 day button
        { await LoadCryptoData(1); }

        private async void btn7Days_Click(object sender, EventArgs e) // 7 day button
        { await LoadCryptoData(7); }

        private async void btn30Days_Click(object sender, EventArgs e) // 30 day button
        { await LoadCryptoData(30); }

        private async void btn365Day_Click(object sender, EventArgs e)
        // 365 day button
        { await LoadCryptoData(365); }

        /// <summary>
        /// When user selects another crypto in the ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void comboBoxCoins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCoins.SelectedItem is CoinInfo coin)
                await LoadOrAddCrypto(coin.Id, coin.Name, coin.Symbol, 7); // default 7 days
            else
                MessageBox.Show("Impossible de mettre à jour les données pour cette crypto.");
        }

        private void clbCryptos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }


        private readonly string stateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "local_data", "state.json");

        /// <summary>
        /// Save current application state (which coins were loaded and which are visible) to local state.json.
        /// </summary>
        private void SaveAppState()
        {
            var state = new AppState
            {
                LoadedCryptos = loadedCryptos.Keys.ToList(),
                VisibleCryptos = visibleCryptos.ToList()
            };

            // Ensure directory exists before writing
            Directory.CreateDirectory(Path.GetDirectoryName(stateFilePath)!);

            File.WriteAllText(
                stateFilePath,
                JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true })
            );
        }

        /// <summary>
        /// Restore application state from state.json: load local files for previously loaded coins,
        /// restore visibility and CheckedListBox items, then update the plot.
        /// </summary>
        private async Task LoadAppState()
        {
            if (!File.Exists(stateFilePath)) return;

            var state = JsonSerializer.Deserialize<AppState>(File.ReadAllText(stateFilePath));
            if (state is null) return;

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "local_data");

            var reloaded = state.LoadedCryptos
                .Select(symbol => new
                {
                    Symbol = symbol,
                    Path = Path.Combine(basePath, $"{symbol.ToLower()}_7days.json")
                })
                .Where(x => File.Exists(x.Path))
                .Select(x => new
                {
                    x.Symbol,
                    Prices = _fileClient.LoadPricesFromFile(x.Path)
                })
                .Where(x => x.Prices.Any())
                .ToDictionary(x => x.Symbol, x => x.Prices);

            reloaded.ToList().ForEach(kvp => loadedCryptos[kvp.Key] = kvp.Value);
            visibleCryptos.UnionWith(state.VisibleCryptos.Where(v => reloaded.ContainsKey(v)));

            state.VisibleCryptos
                .Where(symbol => reloaded.ContainsKey(symbol) && !clbCryptos.Items.Contains(symbol))
                .ToList()
                .ForEach(symbol => clbCryptos.Items.Add(symbol, true));

            UpdatePlot();
        }

        /// <summary>
        /// Triggered when the form is closing (including pressing the "X").
        /// Ensures app state is saved before exit.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveAppState();
        }

    }
}
