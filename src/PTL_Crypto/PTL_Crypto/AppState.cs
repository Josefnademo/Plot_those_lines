using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL_Crypto
{
    class AppState
    {
        public List<string> LoadedCryptos { get; set; } = new();
        public List<string> VisibleCryptos { get; set; } = new();
    }
}
