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
        public void PlotData(FormsPlot formsPlot, Dictionary<string, List<CryptoPrice>> allPrices)
        {
            // Clearing previous charts
            formsPlot.Plot.Clear();

            // LINQ: go through each crypto and add Scatter
            allPrices.ToList().ForEach(kvp =>
            {
                string label = kvp.Key;
                List<CryptoPrice> prices = kvp.Value;

                // Convert DateTime and Price to arrays for plotting
                double[] dataX = prices.Select(p => p.Time.ToOADate()).ToArray();
                double[] dataY = prices.Select(p => p.Price).ToArray();

                //Clear all old charts series
                formsPlot.Plot.Clear();

                // Label the axes and title
                formsPlot.Plot.Add.Scatter(dataX, dataY, label: label);
                formsPlot.Plot.XAxis.DateTimeFormat(true);
                formsPlot.Plot.Legend();
                formsPlot.Plot.Title($"Prix de {label}");
                formsPlot.Plot.XLabel("Date");
                formsPlot.Plot.YLabel("Prix (USD)");

                // Added with new API (ScottPlot 5)
                formsPlot.Plot.Add.Scatter(dataX, dataY);

                // Update control
                formsPlot.Refresh();
            }
    }
    }   } 
