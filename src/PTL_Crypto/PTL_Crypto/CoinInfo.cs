using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL_Crypto
{
    /// <summary>
    /// Contain Coin information and Override it ToString() to prevent Unkown values
    /// </summary>
    public class CoinInfo
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Formats the coin info for display in ComboBox as "Bitcoin (BTC)"
        /// Includes null-safety: if name or symbol are missing, substitutes default values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {// Implementing "null protection":
         // Sometimes CoinGecko API returns coins without a symbol or name → to prevent this, set "?" or "Unknown"
            string safeName = string.IsNullOrWhiteSpace(Name) ? "Unknown" : Name;
            string safeSymbol = string.IsNullOrWhiteSpace(Symbol) ? "?" : Symbol.ToUpper();
            return $"{safeName} ({safeSymbol})";
        }
    }
}
