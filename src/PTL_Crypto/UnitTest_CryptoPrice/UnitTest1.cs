using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace UnitTest_CryptoPrice
{
    // --- Model used for tests ---
    public class CryptoPrice
    {
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Price { get; set; }
    }

    // --- Extension method for timestamp conversion ---
    public static class DateTimeExtensions
    {
        // Converts Unix timestamp (in milliseconds) to DateTime
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

            using JsonDocument doc = JsonDocument.Parse(jsonSample);
            var prices = doc.RootElement.GetProperty("prices")
                .EnumerateArray()
                .Select(el => new CryptoPrice
                {
                    Symbol = "BTC",
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(el[0].GetInt64()).UtcDateTime,
                    Price = (decimal)el[1].GetDouble()
                })
                .ToList();

            // Expected results
            Assert.Equal(3, prices.Count);
            Assert.All(prices, p => Assert.True(p.Price > 0, "Le prix doit être positif"));
            Assert.All(prices, p => Assert.NotEqual(default, p.Timestamp));
        }

        // b. Vérification de la conversion timestamp → DateTime
        [Fact]
        public void Test_Timestamp_To_DateTime_Conversion()
        {
            long ts1 = 1733846400000;
            long ts2 = 1733932800000;

            DateTime d1 = DateTimeExtensions.FromUnixTimestamp(ts1);
            DateTime d2 = DateTimeExtensions.FromUnixTimestamp(ts2);

            Assert.True(d2 > d1, "Les dates doivent être dans un ordre croissant");
            Assert.Equal(DateTimeKind.Utc, d1.Kind);
        }

        // c. Vérification du filtrage des périodes avec LINQ
        [Fact]
        public void Test_Linq_Filtering_By_Period()
        {
            DateTime now = DateTime.UtcNow;
            var prices = Enumerable.Range(0, 30)
                .Select(i => new CryptoPrice
                {
                    Symbol = "BTC",
                    Timestamp = now.AddDays(-i),
                    Price = 50000 + i
                })
                .ToList();

            // Filtrage sur 7 jours
            var filtered = prices
                .Where(p => p.Timestamp >= now.AddDays(-7))
                .ToList();

            Assert.All(filtered, p => Assert.True(p.Timestamp >= now.AddDays(-7)));
            Assert.True(filtered.Count <= 8, "Le nombre de points doit correspondre à la période");
        }

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
            double[] dataX = prices.Select(p => p.Timestamp.ToOADate()).ToArray();
            double[] dataY = prices.Select(p => (double)p.Price).ToArray();

            Assert.Equal(prices.Count, dataX.Length);
            Assert.Equal(prices.Count, dataY.Length);
            Assert.True(dataY.SequenceEqual(dataY.OrderBy(v => v)) ||
                        dataY.SequenceEqual(dataY.OrderByDescending(v => v)),
                        "Les valeurs doivent être ordonnées correctement");
        }

        // Test existant : Validité et ordre temporel
        [Fact]
        public void CryptoPrices_AreValidAndTemporalOrderCorrect()
        {
            // Arrange
            var prices = new List<CryptoPrice>
            {
                new CryptoPrice { Symbol = "BTC", Timestamp = DateTime.UtcNow.AddMinutes(-5), Price = 49000m },
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
        }
    }
}
