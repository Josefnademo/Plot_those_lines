using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PTL_Crypto
{
    /// <summary>
    /// Role: to represent a single price data.
    /// </summary>
    public class CryptoPrice
    {
        public DateTime Time { get; set; } // Date and time of the price
        public double Price { get; set; }  // Price in USD
    }
}
