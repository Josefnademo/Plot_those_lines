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
    // Purpose: Manage the graphical display with ScottPlot.

    /*Methods:*/
    internal class PlotManager
    {
        // Storage of all ScatterPlots for deletion/restoration
        private readonly List<IPlottable> currentPlots = new List<IPlottable>();

        /// <summary>
        /// Plots multiple cryptocurrency price series on the same ScottPlot chart.
        /// The dictionary key is used as the label for each series.
        /// </summary>
        /// <param name="formsPlot">The FormsPlot control to draw on.</param>
        /// <param name="allPrices">Dictionary with coin label as key and list of CryptoPrice as value.</param>
        public void PlotData(FormsPlot formsPlot, Dictionary<string, List<CryptoPrice>> allPrices)
        {

            // Clearing previous charts
            formsPlot.Plot.Clear();
            

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
        // Deleting charts
        public void ClearPlots(FormsPlot formsPlot)
        {
            currentPlots.ForEach(p => formsPlot.Plot.Remove(p));
            currentPlots.Clear();
            formsPlot.Refresh();
        }

        /// <summary>
        /// Restoring charts
        /// </summary>
        /// <param name="formsPlot"></param>
      /* public void RestorePlots(FormsPlot formsPlot)
        {
            currentPlots
                .ToList()                    
                .ForEach(p => formsPlot.Plot.Add.Scatter(p)); //Add accepts IPlottable directly

            formsPlot.Refresh();
        }*/



        /// Importing a JSON file and returning a List<CryptoPrice>
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
