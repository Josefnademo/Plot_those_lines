using ScottPlot;
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

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void formsPlot1_Load(object sender, EventArgs e)
        {
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 12, 23, 24, 35 };

            formsPlot1.Plot.AddScatterLines(dataX, dataY);// ligne avec points
            // formsPlot1.Plot.AddScatterPoints(dataX, dataY); // seulement points
            formsPlot1.Refresh();
        }
    }
}
