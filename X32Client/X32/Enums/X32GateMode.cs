using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X32.Enums
{
    public enum X32GateMode
    {
        NotSet = -1,
        Exp2 = 0,
        Exp3,
        Exp4,
        Gate,
        Duck
    }
    public static partial class X32Extensions
    {
        public static string GetName(this X32GateMode val)
        {
            return Enum.GetName(typeof(X32GateMode), val);
        }
    }
}
