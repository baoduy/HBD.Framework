using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Core;

namespace HBD.Framework.Extension.WinForms
{
    public static class MultiThreadExtension
    {
        public static void SafeThreadExecute<TResult>(this Control control, Func<TResult> func, Action<TResult> callBack)
        {
            BackgroundThreadManager.StartThread<TResult>(
                func,
                (result) =>
                {
                    if (control.InvokeRequired)
                        control.Invoke((MethodInvoker)delegate { callBack.Invoke(result); });
                    else callBack.Invoke(result);
                });
        }
    }
}
