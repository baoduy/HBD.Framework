using System;
namespace HBD.WinForms.Controls.Core
{
    public interface IMultiBrowserControl : IHBDControl
    {
        Type CustomBrowserControlType { get; set; }
        OpenBrowserType OpenBrowserType { get; set; }
        event EventHandler SelectChange;
    }
}
