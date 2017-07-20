using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X32.Enums
{
    public enum X32CompressorEnvelopeType
    {
        NotSet = -1,
        Lin = 0,
        Log
    }

    public static partial class X32Extensions
    {
        public static string GetName(this X32CompressorEnvelopeType val)
        {
            return Enum.GetName(typeof(X32CompressorEnvelopeType), val);
        }
    }
}
