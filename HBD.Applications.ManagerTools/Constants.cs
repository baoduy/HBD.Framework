using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HBD.Framework.Configuration;
using System.Windows.Forms;

namespace HBD.Applications.ManagerTools
{
    public static class Constants
    {
        public static bool ShowErrorMessage
        {
            get
            {
                return ConfigurationManager.GetAppSettingValue<bool>("ShowErrorMessage");
            }
        }

        public static void ShowError(Exception ex)
        {
            if (Constants.ShowErrorMessage)
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
