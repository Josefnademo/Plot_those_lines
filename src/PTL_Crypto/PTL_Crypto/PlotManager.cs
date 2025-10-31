using ScottPlot;
using ScottPlot.Triangulation;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static ScottPlot.Generate;

namespace PTL_Crypto
{
    /// <summary>
    /// Manages plotting cryptocurrency price data using ScottPlot.
    /// Handles drawing multiple price series on the same chart,
    /// clearing plots, and loading price data from JSON files.
    /// </summary>
    internal class PlotManager
    {
        // Storage of all ScatterPlots for deletion/restoration
        private readonly List<IPlottable> currentPlots = new List<IPlottable>();

        /// <summary>
        /// Plots multiple cryptocurrency price series on a single ScottPlot chart.
        /// Each entry in the dictionary represents one coin, where the key is the label
        /// and the value is the list of price data points.
        /// </summary>
        /// <param name="formsPlot">The FormsPlot control to draw on.</param>
        /// <param name="allPrices">Dictionary with coin label as key and list of CryptoPrice as value.</param>
        public void PlotData(FormsPlot formsPlot, Dictionary<string, List<CryptoPrice>> allPrices)
        {
            // LINQ: go through each crypto and add Scatter
            allPrices.ToList().ForEach(kvp =>
            {
                string label = kvp.Key;             // Coin name, used as label
                List<CryptoPrice> prices = kvp.Value;

                // Convert DateTime and Price to arrays for plotting
                double[] dataX = prices.Select(p => p.Time.ToOADate()).ToArray();
                double[] dataY = prices.Select(p => p.Price).ToArray();

                // Add scatter series
                var scatter = formsPlot.Plot.Add.Scatter(dataX, dataY);

                // Assign label for legend
                scatter.Label = label;
                scatter.MarkerSize = 4;
                scatter.LineWidth = 1;
                scatter.MarkerShape = MarkerShape.OpenCircle;

                // Enable display of date on the X-axis
                formsPlot.Plot.Axes.DateTimeTicksBottom();

                // Set chart title and axis labels
                formsPlot.Plot.Title($"Prix de {label}");
                formsPlot.Plot.XLabel("Date");
                formsPlot.Plot.YLabel("Price (USD)");

                // Update control
                formsPlot.Refresh();
            });
    }
        /// <summary>
        /// Removes all currently displayed plots from the ScottPlot chart
        /// and refreshes the control.
        /// </summary>
        /// <param name="formsPlot">ScottPlot control to clear</param>
        public void ClearPlots(FormsPlot formsPlot)
        {
            currentPlots.ForEach(p => formsPlot.Plot.Remove(p));
            currentPlots.Clear();
            formsPlot.Refresh();
        }


        /// <summary>
        /// Loads historical price data from a local JSON file.
        /// Converts Unix timestamps to DateTime and returns a list of CryptoPrice objects.
        /// </summary>
        /// <param name="filePath">Full path to the JSON file.</param>
        /// <returns>List of CryptoPrice values parsed from JSON.</returns>
        public List<CryptoPrice> LoadFromJsonFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty("prices", out var pricesEl))
                return new List<CryptoPrice>();

            return pricesEl.EnumerateArray()
                .Select(el =>
                {
                    long unixMs = el[0].GetInt64();
                    System.DateTime dt = System.DateTimeOffset.FromUnixTimeMilliseconds(unixMs).DateTime;
                    double price = el[1].GetDouble();
                    return new CryptoPrice { Time = dt, Price = price };
                })
                .ToList();
        }
    }

}   
