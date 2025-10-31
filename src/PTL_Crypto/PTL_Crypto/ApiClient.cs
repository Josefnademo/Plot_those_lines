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
        private static DateTime _lastRequest = DateTime.MinValue;
        private const int MinDelayMs = 1500; // 1.5 sec between requests


        /// <summary>
        /// Simple delay to avoid hitting API rate limits (CoinGecko allows ~50 req/min).
        /// Calls this before every API request.
        /// </summary>
        private static async Task EnforceRateLimitAsync()
        {   
            // Calculate the time elapsed since the last API request
            var diff = DateTime.UtcNow - _lastRequest;

            // If the elapsed time is less than the minimum required delay, wait
            if (diff.TotalMilliseconds < MinDelayMs)
                await Task.Delay(MinDelayMs - (int)diff.TotalMilliseconds);

            // Update the timestamp of the last request to the current time
            _lastRequest = DateTime.UtcNow;
        }


        /// <summary>
        /// Fetch cryptocurrency price data from CoinGecko API and put it in List<CryptoPrice>
        /// </summary>
        /// <param name="coin"></param>
        /// <param name="days"></param>
        /// <returns></returns>
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
        /// Retrieves a list of all cryptocurrencies from the CoinGecko API.
        /// The method enforces a rate limit delay, sends an HTTP request,
        /// parses the JSON response, maps the data to <see cref="CoinInfo"/> objects,
        /// and filters out invalid or empty records. 
        /// </summary>
        /// <returns>
        /// /// A list of <see cref="CoinInfo"/> representing valid cryptocurrencies.
        /// Returns an empty list if an error occurs.
        /// </returns>
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