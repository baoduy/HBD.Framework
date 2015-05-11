using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.WinForms.Controls.Core;

namespace HBD.WinForms.Controls.Comparison.Events
{
    public enum SelectedFileType { FileA, FileB }

    public class FileSelectedEventArgs : EventArgs
    {
        public IOpenBrowserConvertableControl OpenBrowser { get; private set; }
        public SelectedFileType FileType { get; private set; }

        public FileSelectedEventArgs(IOpenBrowserConvertableControl openBrowser, SelectedFileType fileType)
        {
            this.OpenBrowser = openBrowser;
            this.FileType = fileType;
        }
    }
}
