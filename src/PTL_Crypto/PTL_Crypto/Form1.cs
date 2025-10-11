using OpenTK.Graphics.ES10;
using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScottPlot.Generate;

namespace PTL_Crypto
{
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
        }

        // --- On Form Load: load the list of available coins (from API or local fallback) ---
        private async void Form1_Load(object sender, EventArgs e)
        {
            List<CoinInfo> coins;
            try
            {
                // Try via API
                coins = await _apiClient.GetCoinsListAsync();
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
        }

        // Method for loading a chart for a selected coin (Loads local JSON files)
        private async Task LoadDefaultGraph()
        {
            string basePath = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\local_data")
            );

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



        // ---  Main universal method — loads or add crypto data from API, local or import file.
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
                    prices = await _apiClient.GetCryptoPricesAsync(coinId, days);
                }
                catch
                {
                    // 3️Fallback to local JSON
                    string basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\local_data"));
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
            if (!checkedListBoxCryptos1.Items.Contains(key))
                checkedListBoxCryptos1.Items.Add(key, true);

            // Update chart
            UpdatePlot();
        }

        // --- Method for updating a graph based on visibility ---
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

        // Controlling visibility (CheckedListBox or buttons)
        private void checkedListBoxCryptos_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string symbol = checkedListBoxCryptos1.Items[e.Index].ToString();

            // Update visibility according to the new state
            if (e.NewValue == CheckState.Checked)
                visibleCryptos.Add(symbol);
            else
                visibleCryptos.Remove(symbol);

            // Delay update until checkbox state actually changes
            UpdatePlot();
        }

        // --- Import custom JSON file (.json only) ---
        private void button1_Click(object sender, EventArgs e)
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

        // Load crypto from ComboBox for 1/7/30/365 days
        private async Task LoadCryptoData(int days)
        {
            if (comboBoxCoins.SelectedItem is not CoinInfo selectedCoin)
            {
                MessageBox.Show("Sélectionnez une crypto monnaie");
                return;
            }
            await LoadOrAddCrypto(selectedCoin.Id, selectedCoin.Name, selectedCoin.Symbol, days);
        }

        private async void button5_Click(object sender, EventArgs e) // 1 day button
        { await LoadCryptoData(1); }

        private async void button2_Click(object sender, EventArgs e) // 7 day button
        { await LoadCryptoData(7); }

        private async void button3_Click(object sender, EventArgs e) // 30 day button
        { await LoadCryptoData(30); }

        private async void button4_Click(object sender, EventArgs e) // 365 day button
        { await LoadCryptoData(365); }

        // --- When user selects another crypto in the ComboBox ---
        private async void comboBoxCoins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCoins.SelectedItem is CoinInfo coin)
                await LoadOrAddCrypto(coin.Id, coin.Name, coin.Symbol, 7); // default 7 days
            else
                MessageBox.Show("Impossible de mettre à jour les données pour cette crypto.");
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }
    }
}
/*
 test with json local data:

// Create an instance of FileClient to read local JSON files
            var fileClient = new FileClient();

            // Define the base path where local JSON data files are stored,then enter the "local_data" folder
            string basePath = Path.Combine(Application.StartupPath, "..", "..", "..", "..", "..", "local_data");

            // Load data for each cryptocurrency from its corresponding JSON file
            var allPrices = new Dictionary<string, List<CryptoPrice>>
{

    { "BTC", fileClient.LoadPricesFromFile(Path.Combine(basePath, "btc_1y.json")) },
    { "ETH", fileClient.LoadPricesFromFile(Path.Combine(basePath, "eth_1y.json")) },
    { "SOL", fileClient.LoadPricesFromFile(Path.Combine(basePath, "solana_7days.json")) },
    { "PEPE", fileClient.LoadPricesFromFile(Path.Combine(basePath, "pepe_7days.json")) }
};
 // Plot data on the chart using PlotManager
            _plotManager.PlotData(formsPlot1, allPrices);
 */