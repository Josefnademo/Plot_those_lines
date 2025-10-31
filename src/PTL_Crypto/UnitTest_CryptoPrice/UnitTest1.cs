using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit; // framework for Unit tests

namespace UnitTest_CryptoPrice
{
    // --- Model used for tests ---
    public class CryptoPrice
    {
        public string Symbol { get; set; } //cryptocurrency symbol, such as "BTC"
        public DateTime Timestamp { get; set; } //The time the price was recorded
        public decimal Price { get; set; } //The price at that moment
    }

    // --- Extension method for UNIX timestamp conversion ---
    public static class DateTimeExtensions
    {
        // Converts Unix timestamp (in milliseconds, from 1970) to DateTime
        public static DateTime FromUnixTimestamp(long timestampMs)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestampMs).UtcDateTime;
        }
    }


    // --- Test class grouping all unit tests ---
    public class CryptoPriceTests
    {
        // a. Vérification du parsing du JSON depuis l’API CoinGecko
        [Fact]
        public void Test_Json_Parsing_From_CoinGecko()
        {
            // Sample JSON as returned by API
            string jsonSample = @"{
                ""prices"": [
                    [1733846400000, 45000.5],
                    [1733932800000, 45550.7],
                    [1734019200000, 46000.2]
                ]
            }";

            using JsonDocument doc = JsonDocument.Parse(jsonSample); //parsing json
            var prices = doc.RootElement.GetProperty("prices") 
                .EnumerateArray() //go through each element [timestamp, price]
                .Select(el => new CryptoPrice
                {
                    Symbol = "BTC",
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(el[0].GetInt64()).UtcDateTime, //take the timestamp (ms)
                    Price = (decimal)el[1].GetDouble() //take the price
                })
                .ToList();

            // Expected results
            Assert.Equal(3, prices.Count); // There must be 3 records
            Assert.All(prices, p => Assert.True(p.Price > 0, "Le prix doit être positif")); // All prices are positive
            Assert.All(prices, p => Assert.NotEqual(default, p.Timestamp)); // Time is not empty
        }

        // b. Vérification de la conversion UNIX timestamp → DateTime
        [Fact]
        public void Test_Timestamp_To_DateTime_Conversion()
        {
            long ts1 = 1733846400000;
            long ts2 = 1733932800000;

            DateTime d1 = DateTimeExtensions.FromUnixTimestamp(ts1);
            DateTime d2 = DateTimeExtensions.FromUnixTimestamp(ts2);

            Assert.True(d2 > d1, "Les dates doivent être dans un ordre croissant");
            Assert.Equal(DateTimeKind.Utc, d1.Kind);
        } //The later date must be greater than the earlier date, and the time must be in UTC.


        // c. Vérification du filtrage des périodes avec LINQ
        [Fact]
        public void Test_Linq_Filtering_By_Period()
        {
            DateTime now = DateTime.UtcNow;
            var prices = Enumerable.Range(0, 30) //list of all prices (for 30 days)
                .Select(i => new CryptoPrice //Created 30 days of prices, the price increases every day
                {
                    Symbol = "BTC",
                    Timestamp = now.AddDays(-i),
                    Price = 50000 + i
                })
                .ToList();

            // Filtrage sur 7 jours
            var filtered = prices
                .Where(p => p.Timestamp >= now.AddDays(-7))//take only records that are no older than 7 days
                .ToList();

            Assert.All(filtered, p => Assert.True(p.Timestamp >= now.AddDays(-7)));
            Assert.True(filtered.Count <= 8, "Le nombre de points doit correspondre à la période");
        }//All dates in the last 7 days,The number of entries is correct.



        // d. Vérification de la transformation des données en séries ScottPlot
        [Fact]
        public void Test_Data_To_ScottPlot_Series()
        {
            var prices = new List<CryptoPrice>
            {
                new CryptoPrice { Symbol = "BTC", Timestamp = DateTime.UtcNow.AddDays(-3), Price = 47000 },
                new CryptoPrice { Symbol = "BTC", Timestamp = DateTime.UtcNow.AddDays(-2), Price = 48000 },
                new CryptoPrice { Symbol = "BTC", Timestamp = DateTime.UtcNow.AddDays(-1), Price = 49000 }
            };

            // Simulation des séries (comme dans PlotManager)
            double[] dataX = prices.Select(p => p.Timestamp.ToOADate()).ToArray(); //dates as numbers
            double[] dataY = prices.Select(p => (double)p.Price).ToArray(); //prices

            Assert.Equal(prices.Count, dataX.Length);
            Assert.Equal(prices.Count, dataY.Length);
            Assert.True(dataY.SequenceEqual(dataY.OrderBy(v => v)) ||
                        dataY.SequenceEqual(dataY.OrderByDescending(v => v)),
                        "Les valeurs doivent être ordonnées correctement");
        }//The length matches and the data is ordered.

        // Test existant : Validité et ordre temporel
        [Fact]
        public void CryptoPrices_AreValidAndTemporalOrderCorrect()
        {
            // Arrange
            var prices = new List<CryptoPrice>
            {
                new CryptoPrice { Symbol = "BTC", Timestamp = DateTime.UtcNow.AddMinutes(-5), Price = 49000m },//in c# "m" is a decimal.Without m, the default value is double.
                new CryptoPrice { Symbol = "BTC", Timestamp = DateTime.UtcNow.AddMinutes(-3), Price = 49500m },
                new CryptoPrice { Symbol = "BTC", Timestamp = DateTime.UtcNow.AddMinutes(-1), Price = 50000m }
            };

            // Act & Assert
            for (int i = 0; i < prices.Count; i++)
            {
                Assert.False(string.IsNullOrWhiteSpace(prices[i].Symbol), $"Symbol at index {i} is empty");
                Assert.True(prices[i].Price > 0, $"Price at index {i} is not positive");
                Assert.True(prices[i].Timestamp <= DateTime.UtcNow, $"Timestamp at index {i} is in the future");

                if (i > 0)
                {
                    Assert.True(prices[i].Timestamp > prices[i - 1].Timestamp,
                        $"Timestamp at index {i} is not later than previous one");
                }
            }
        }//There is a symbol. Price > 0. The date is not in the future. Each subsequent date is later than the previous one.
    }
}
