using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace HBD.WinForms.Controls.Core
{
    public interface IHBDViewBase :IHBDControl, IContainerControl
    {
        void LoadState(StringCollection collection);
        void SaveState(StringCollection collection);
    }
}
