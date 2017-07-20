using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X32.Enums
{
    public enum X32FilterType
    {
        NotSet = -1,
        LC6 = 0,
        LC12,
        HC6,
        HC12,
        One,
        Two,
        Three,
        Five,
        Ten
    }

    public static partial class X32Extensions
    {
        public static string GetName(this X32FilterType val)
        {
            return Enum.GetName(typeof(X32FilterType), val);
        }
    }
}
