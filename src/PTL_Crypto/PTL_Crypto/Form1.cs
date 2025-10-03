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

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Downloading a list of coins from CoinGecko
            var coins = await _apiClient.GetCoinsListAsync();

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

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }

        // Load crypto data and plot chart using textbox and buttons
        private async Task LoadCryptoData(int days)
        {

            // Get coin name from search bar
            string coin = textBoxCoin.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(coin))
            {
                MessageBox.Show("Entre un nom d'une crypto monnaie");
                return;
            }

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void comboBoxCoins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCoins.SelectedItem is CoinInfo selectedCoin)
            {
                try
                {
                    // when a person selects from the ComboBox → fill the TextBox
                    textBoxCoin.Text = selectedCoin.Id;
                    // loading 7 days of data bydefault
                    await LoadCryptoData(7);
                }
                catch
                {
                    MessageBox.Show("Impossible de mettre à jour les données pour cette crypto.");
                }
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