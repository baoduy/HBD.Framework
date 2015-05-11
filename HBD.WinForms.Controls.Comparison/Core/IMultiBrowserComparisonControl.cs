using System;
namespace HBD.WinForms.Controls.Comparison.Core
{
    public interface IMultiBrowserComparisonControl
    {
        Type CustomBrowserControlAType { get; set; }
        Type CustomBrowserControlBType { get; set; }
        OpenBrowserType OpenBrowserAType { get; set; }
        OpenBrowserType OpenBrowserBType { get; set; }
    }
}
