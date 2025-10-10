using ScottPlot;
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
        // Instancies
        private readonly ApiClient _apiClient = new ApiClient();
        private readonly PlotManager _plotManager = new PlotManager();
        private readonly FileClient _fileClient = new FileClient();

        public Form1()
        {
            InitializeComponent();
        }

        // Downloading a list of coins from CoinGecko OR Local files in case of error with API
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
                // If Api isn't avalabel - loading local files
                coins = new List<CoinInfo>
                {
                      new CoinInfo { Id = "btc", Name = "Bitcoin", Symbol = "BTC" },
                      new CoinInfo { Id = "eth", Name = "Ethereum", Symbol = "ETH" },
                      new CoinInfo { Id = "sol", Name = "Solana", Symbol = "SOL" },
                      new CoinInfo { Id = "pepe", Name = "PepeCoin", Symbol = "PEPE" }
                };
            }

            // filter just in case
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

            // Autocompletion
            comboBoxCoins.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxCoins.AutoCompleteSource = AutoCompleteSource.ListItems;

            await LoadDefaultGraph();
        }

        // Method for loading a chart for a selected coin
        private async Task LoadDefaultGraph()
        {
            string basePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\local_data"));
            var allPrices = new Dictionary<string, List<CryptoPrice>>();

            foreach (var coin in comboBoxCoins.DataSource as List<CoinInfo>)
            {
                string fileName = coin.Id switch
                {
                    "btc" => "btc_7days.json",
                    "eth" => "eth_7days.json",
                    "sol" => "solana_7days.json",
                    "pepe" => "pepe_7days.json",
                    _ => null
                };

                if (fileName != null)
                {
                    string path = Path.Combine(basePath, fileName);

                    if (!File.Exists(path))
                    {
                        MessageBox.Show($"File {fileName} not found in {basePath}");
                        continue;
                    }

                    var prices = _fileClient.LoadPricesFromFile(path);
                    allPrices.Add(coin.Symbol, prices);
                }
            }

            if (allPrices.Count > 0)
                _plotManager.PlotData(formsPlot1, allPrices);
            else
                MessageBox.Show("There are no local files available to plot the graph");
        }



        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }

        // Load crypto data and plot chart using combobox and buttons
        private async Task LoadCryptoData(int days)
        {

            // Get coin name from search bar of combobox
            if (comboBoxCoins.SelectedItem is not CoinInfo selectedCoin)
            {
                MessageBox.Show("Sélectionnez une crypto monnaie");
                return;
            }

            string coin = selectedCoin.Id.ToLower();

            try
            {
                // Fetch prices from API
                var prices = await _apiClient.GetCryptoPricesAsync(coin, days);

                // Put data in Dictionary
                var allPrices = new Dictionary<string, List<CryptoPrice>>
                 {
                   { coin.ToUpper(), prices }
                 };

                // Plot data on the chart using PlotManager
                _plotManager.PlotData(formsPlot1, allPrices);
            }
            catch
            {
                MessageBox.Show("Impossible de récupérer les données depuis l'API.");
            }
        }
        private async void button5_Click(object sender, EventArgs e) // 1 day button
        {
            await LoadCryptoData(1);
        }

        private async void button2_Click(object sender, EventArgs e) // 7 day button
        {
            await LoadCryptoData(7);
        }

        private async void button3_Click(object sender, EventArgs e) // 30 day button
        {
            await LoadCryptoData(30);
        }

        private async void button4_Click(object sender, EventArgs e) // 365 day button
        {
            await LoadCryptoData(365);
        }


        private async void comboBoxCoins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCoins.SelectedItem is CoinInfo selectedCoin)
            {
                    // loading 7 days of data bydefault
                    await LoadCryptoData(7);
            }
            else {
                MessageBox.Show("Impossible de mettre à jour les données pour cette crypto.");
            }
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