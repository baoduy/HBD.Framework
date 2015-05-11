using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HBD.Framework.Log;

namespace HBD.Applications.ManagerTools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            //{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());
            //}
            //catch (Exception ex)
            //{
            //    LogManager.Write(ex);
            //    Constants.ShowError(ex);
            //}
        }
    }
}
