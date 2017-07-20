using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X32.Enums
{
    public enum X32Source
    {
        NotSet = -1,
        Off = 0,

        In01,
        In02,
        In03,
        In04,
        In05,
        In06,
        In07,
        In08,
        In09,

        In10,
        In11,
        In12,
        In13,
        In14,
        In15,
        In16,
        In17,
        In18,
        In19,

        In20,
        In21,
        In22,
        In23,
        In24,
        In25,
        In26,
        In27,
        In28,
        In29,

        In30,
        In31,
        In32,

        Aux1,
        Aux2,
        Aux3,
        Aux4,
        Aux5,
        Aux6,

        USBL,
        USBR,

        FX1L,
        FX1R,
        FX2L,
        FX2R,
        FX3L,
        FX3R,
        FX4L,
        FX4R,

        Bus1,
        Bus2,
        Bus3,
        Bus4,
        Bus5,
        Bus6,
        Bus7,
        Bus8,
        Bus9,
        Bus10,
        Bus11,
        Bus12,
        Bus13,
        Bus14,
        Bus15,
        Bus16
    }

    public static partial class X32Extensions
    {
        public static string GetName(this X32Source val)
        {
            return Enum.GetName(typeof(X32Source), val);
        }
    }
}
