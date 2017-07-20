using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X32.Enums
{
    public enum X32CompressorDetectionType
    {
        NotSet = -1,
        Peak = 0,
        RMS
    }

    public static partial class X32Extensions
    {
        public static string GetName(this X32CompressorDetectionType val)
        {
            return Enum.GetName(typeof(X32CompressorDetectionType), val);
        }
    }
}
