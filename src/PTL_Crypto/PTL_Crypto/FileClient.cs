using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PTL_Crypto
{
    /// <summary>
    /// Loads crypto price data from a local JSON file and converts it into a list of CryptoPrice objects.
    /// Optionally displays raw formatted price data in a TextBox(ComboBox).
    /// </summary>
    /// <param name="filePath">Full path to the JSON file.</param>
    /// <param name="textBoxRawData">Optional TextBox for showing formatted raw data (time and price).</param>
    /// <returns>List of parsed CryptoPrice entries from the file.</returns>
    public class FileClient
    {
        // Reads JSON from a local file and converts into a list of CryptoPrice objects
        public List<CryptoPrice> LoadPricesFromFile(string filePath, TextBox textBoxRawData = null) //i left prameters as optional so it will also work  with or without Textbox
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
                    long unixMs = el[0].GetInt64();
                    DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(unixMs).DateTime;

                    double price = el[1].GetDouble();

                    return new CryptoPrice { Time = date, Price = price };
                })
                .ToList();

            // LINQ: generate strings for the TextBox (only if a TextBox is passed)
            if (textBoxRawData != null)
            {
                var lines = list
                    .Select(p => $"{p.Time:G} : {p.Price} USD")
                    .ToArray();

                textBoxRawData.Text = string.Join(Environment.NewLine, lines);
            }

            return list;
        } 
    }
}
