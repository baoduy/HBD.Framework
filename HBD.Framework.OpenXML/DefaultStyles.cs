using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HBD.Framework.OpenXML
{
    public enum DefaultFontStyle
    {
        Default = 0,
        Bold = 1,
        Italic = 2,
        TimesRomanSize16 = 3,
    }

    public enum DefaultFillStyle
    {
        Default = 0,
        FillOfGray125 = 1,
        YellowFill = 2,
        RedFill = 3,
        GreenFill = 4,
    }

    public enum DefaultBorderStyle
    {
        NonBoder = 0,
        Boder = 1,
    }

    public enum DefaultCellStyle
    {
        Default = 0,
        Bold = 1,
        Italic = 2,
        TimesRoman = 3,
        YellowFill = 4,
        Alignment = 5,
        Boder = 6,
        RedFill = 7,
    }
}
