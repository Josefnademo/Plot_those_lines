using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot;

namespace PTL_Crypto
{
    //Role: to represent a single price data.

    /*Methods:*/
    public class CryptoPrice
    {
        public DateTime Time { get; set; }
        public double Price { get; set; }
    }
}
