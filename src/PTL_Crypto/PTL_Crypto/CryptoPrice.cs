using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PTL_Crypto
{
    //Role: to represent a single price data.
    public class CryptoPrice
    {
        public DateTime Time { get; set; } // Date and time of the price
        public double Price { get; set; }  // Price in USD
    }
}
