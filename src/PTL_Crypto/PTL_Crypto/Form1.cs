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
        //Creat
        private readonly ApiClient _apiClient = new ApiClient();
        private readonly PlotManager _plotManager = new PlotManager();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create an instance of FileClient to read local JSON files
            var fileClient = new FileClient();

            // Define the base path where local JSON data files are stored,then enter the "local_data" folder
            string basePath = Path.Combine(Application.StartupPath, "..", "..", "..", "..", "..", "local_data");

            // Load data for each cryptocurrency from its corresponding JSON file
            var allPrices = new Dictionary<string, List<CryptoPrice>>
{
   
    { "BTC", fileClient.LoadPricesFromFile(Path.Combine(basePath, "btc_1y.json")) },
    { "ETH", fileClient.LoadPricesFromFile(Path.Combine(basePath, "eth_7days.json")) },
    { "SOL", fileClient.LoadPricesFromFile(Path.Combine(basePath, "solana_7days.json")) },
    { "PEPE", fileClient.LoadPricesFromFile(Path.Combine(basePath, "pepe_7days.json")) }
};
            // Plot data on the chart using PlotManager
            _plotManager.PlotData(formsPlot1, allPrices);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }

        // Load crypto data and plot chart using textbox and buttons
        private async Task LoadCryptoData(int days) {

            // Get coin name from search bar
            string coin = textBoxCoin.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(coin))
            {
                MessageBox.Show("Recherche de crypto");
                return;
            }
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
    }
}
