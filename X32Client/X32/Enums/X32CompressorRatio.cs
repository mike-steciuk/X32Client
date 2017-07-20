using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X32.Enums
{
    public enum X32CompressorRatio
    {
        NotSet = -1,
        OnePointOne,
        OnePointThree,
        OnePointFive,
        Two,
        TwoPointFive,
        Three,
        Four,
        Five,
        Seven,
        Ten,
        Twenty,
        OneHundred
    }

    public static partial class X32Extensions
    {
        public static string GetName(this X32CompressorRatio val)
        {
            return Enum.GetName(typeof(X32CompressorRatio), val);
        }
    }
}
