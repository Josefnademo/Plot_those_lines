using ScottPlot;
using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace PTL_Crypto
{
    /// <summary>
    /// Role: Connect to the CoinGecko API.
    /// </summary>
    internal class ApiClient
    {
        private readonly HttpClient _httpClient = new HttpClient(); //creating an object of the HttpClient class, which is used to send HTTP requests


        /// <summary>
        /// Simple delay to avoid hitting API rate limits (CoinGecko allows ~50 req/min).
        /// Calls this before every API request.
        /// </summary>
        private static DateTime _lastRequestTime = DateTime.MinValue;

        private static async Task EnforceRateLimitAsync(int delayMs = 3000)
        {
            var now = DateTime.Now;
            var elapsed = (now - _lastRequestTime).TotalMilliseconds;

            if (elapsed < delayMs)
                await Task.Delay(delayMs - (int)elapsed);

            _lastRequestTime = DateTime.Now;
        }


        // Fetch cryptocurrency price data from CoinGecko API
        public async Task<List<CryptoPrice>> GetCryptoPricesAsync(string coin, int days)
        {
            try
            {
                // Delay 3 sec before the request to reduce 429 errors
                await EnforceRateLimitAsync();

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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoinInfo>> GetCoinsListAsync()
        {
            try
            {
                // Delay 3 sec before the request to reduce 429 errors
                await EnforceRateLimitAsync();

                string url = "https://api.coingecko.com/api/v3/coins/list";
                // http request
                string json = await _httpClient.GetStringAsync(url);

                // Parsing JSON
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // LINQ: Selecting Only Valid Records
                var coins = root.EnumerateArray()
                                .Select(el => new CoinInfo
                                {
                                    Id = el.GetProperty("id").GetString(),
                                    Symbol = el.GetProperty("symbol").GetString(),
                                    Name = el.GetProperty("name").GetString()
                                })
                                .Where(c => !string.IsNullOrWhiteSpace(c.Id) &&
                                            !string.IsNullOrWhiteSpace(c.Symbol) &&
                                            !string.IsNullOrWhiteSpace(c.Name))
                                .ToList();

                return coins;
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Error loading coin list: " + ex.Message);
                return new List<CoinInfo>();
            }
        }

    }
}
// ?? - null-coalescing, operatorChecks: if the value to the left of ?? is null, then the value on the right is taken.
// If it is not null, then the value on the left is taken.