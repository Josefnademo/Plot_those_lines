using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTest_CryptoPrice
{
    public class CryptoPrice
    {
        public string Symbol { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Price { get; set; }
    }

    public class CryptoPriceTests
    {
        [Fact]
        public void CryptoPrices_AreValidAndTemporalOrderCorrect()
        {
            // Arrange: create a list of test data
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
