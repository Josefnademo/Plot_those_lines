using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;


namespace PTL_Crypto
{
    //Role: Connect to the CoinGecko API.

    /*Methods:*/
    internal class ApiClient
    {
        private readonly HttpClient _httpClient = new HttpClient(); //creating an object of the HttpClient class, which is used to send HTTP requests

        // Fetch cryptocurrency price data from CoinGecko API
        public async Task<List<CryptoPrice>> GetCryptoPricesAsync(string coin, int days)
        {
            try
            {
                // Build API URL dynamically based on coin and period (days)
                string url = $"https://api.coingecko.com/api/v3/coins/{coin}/market_chart?vs_currency=usd&days={days}";

                // Send GET request to API and wait for response
                string json = await _httpClient.GetStringAsync(url);

                // Parse JSON response
                var jsonData = JsonDocument.Parse(json);
                var pricesArray = jsonData.RootElement.GetProperty("prices").EnumerateArray();

                // LINQ: transform array [timestamp, price] → List<CryptoPrice>
                var pricesList = pricesArray.Select(item =>{
                                               long unixTime = item[0].GetInt64(); //timestamp in millliseconds
                                               double price = item[1].GetDouble(); //price
                                               DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
                                               return new CryptoPrice { Time = date, Price = price };
                                           })
                                           .ToList();

                return pricesList;
            }
            catch
            {
                // Show error message if API call fails
                MessageBox.Show("Erreur lors de la récupération des données.");
                return new List<CryptoPrice>();
            }

        }
    }
}
