using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PTL_Crypto
{
    public class FileClient
    {
        // Reads JSON from a local file and converts into a list of CryptoPrice objects
        public List<CryptoPrice> LoadPricesFromFile(string filePath)
        {
            // Read JSON text from the file
            string json = File.ReadAllText(filePath);

            // Parse the JSON
            using JsonDocument doc = JsonDocument.Parse(json);

            JsonElement root = doc.RootElement;

            // Get the "prices" array
            if (!root.TryGetProperty("prices", out JsonElement pricesEl))
                return new List<CryptoPrice>();

            // Convert every [timestamp, price] entry into a CryptoPrice object
            var list = pricesEl.EnumerateArray()
                .Select(el =>
                {
                    // Element format: [timestampMs, priceUsd]
                    long ms = Convert.ToInt64(el[0].GetDouble());
                    DateTime time = DateTimeOffset.FromUnixTimeMilliseconds(ms).DateTime;

                    double price = el[1].GetDouble();

                    return new CryptoPrice { Time = time, Price = price };
                })
                .ToList();

            return list;
        } 
    }
}
