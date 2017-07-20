using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X32.Enums
{
    public enum  X32CompressorMode
    {
        NotSet = -1,
        Comp = 0,
        Exp
    }

    public static partial class X32Extensions
    {
        public static string GetName(this X32CompressorMode val)
        {
            return Enum.GetName(typeof(X32CompressorMode), val);
        }
    }
}
