using System;
namespace HBD.WinForms.Controls.Core
{
    public interface IOpenBrowserControl : IHBDControl
    {
        string SourcePath { get; set; }
        string SourceName { get; set; }
        bool ValidateData();
        event EventHandler SelectChange;
    }
}
