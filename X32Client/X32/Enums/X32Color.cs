using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X32.Enums
{
    public enum X32Color
    {
        NotSet = -1,
        Black = 0,
        Red,
        Green,
        Yellow,
        Blue,
        Purple,
        Cyan,
        White,
        InvertedBlack,
        InvertedRed,
        InvertedGreen,
        InvertedYellow,
        InvertedBlue,
        InvertedPurple,
        InvertedCyan,
        InvertedWhite
    }

    public static partial class X32Extensions
    {
        public static string GetName(this X32Color val)
        {
            return Enum.GetName(typeof(X32Color), val);
        }
    }
}
