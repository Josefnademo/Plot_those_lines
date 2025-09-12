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

            // Plot data on the chart using PlotManager
            _plotManager.PlotData(formsPlot1, prices);
        }

        private void button5_Click(object sender, EventArgs e) // 1 day button
        {
            await LoadCryptoData(1);
        }

        private void button2_Click(object sender, EventArgs e) // 7 day button
        {
            await LoadCryptoData(7);
        }

        private void button3_Click(object sender, EventArgs e) // 30 day button
        {
            await LoadCryptoData(30);
        }

        private void button4_Click(object sender, EventArgs e) // 365 day button
        {
            await LoadCryptoData(365);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
