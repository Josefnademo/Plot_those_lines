using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ScottPlot.Generate;

namespace PTL_Crypto
{
    // Purpose: Manage the graphical display with ScottPlot.

    /*Methods:*/
    internal class PlotManager
    {
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
                scatter.Label = label; ;

                // Set chart title and axis labels
                
                formsPlot.Plot.Title($"Prix de {label}");
                formsPlot.Plot.XLabel("Date");
                formsPlot.Plot.YLabel("Price (USD)");

                // Update control
                formsPlot.Refresh();
            });
    }   }
}   
