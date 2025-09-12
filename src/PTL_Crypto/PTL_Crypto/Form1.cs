using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL_Crypto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Label the axes and title
            formsPlot1.Plot.Title($"{CryptoCurrencyPrice} – prix sur 30 jours");
            formsPlot1.Plot.XLabel("Date");
            formsPlot1.Plot.YLabel("Prix (USD)");
            double[] dataX = { Time };
            double[] dataY = { Price };

            // Ajout avec la nouvelle API (ScottPlot 5)
            formsPlot1.Plot.Add.Scatter(dataX, dataY);

            formsPlot1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
