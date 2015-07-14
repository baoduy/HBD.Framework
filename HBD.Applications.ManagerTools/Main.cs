using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Plugin.Configuration;
using HBD.Framework.Plugin;
using System.IO;
using HBD.Framework.Log;
using HBD.Framework.Extension.WinForms;
using HBD.WinForms.Controls.Core;

namespace HBD.Applications.ManagerTools
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Properties.Settings.Default.State == null)
            {
                Properties.Settings.Default.State = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.Save();
            }

#if !DEBUG
            try
            {
#endif
            var plugin = PluginManager.Plugins;
            foreach (PluginElementGroup i in plugin.WinFormPlugin)
            {
                ToolStripMenuItem menu = new ToolStripMenuItem(i.Name.Replace("_", "&"));
                if (!string.IsNullOrEmpty(i.Icon))
                    menu.Image = Image.FromFile(i.Icon);

                menu.DropDownItemClicked += menuStrip_ItemClicked;

                this.menuStrip.Items.Add(menu);
                foreach (PluginElement p in i.Plugins)
                {
                    ToolStripMenuItem submenu = new ToolStripMenuItem(p.Title.Replace("_", "&"));
                    submenu.Name = p.Name;
                    if (!string.IsNullOrEmpty(p.Icon))
                        submenu.Image = Image.FromFile(p.Icon);

                    menu.DropDownItems.Add(submenu);
                }
            }
#if !DEBUG
            }
            catch (Exception ex)
            {
                LogManager.Write(ex);
                Constants.ShowError(ex);
            }
#endif
        }

        HBDViewBase _currentControl;
        private string _currentViewName;
        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
#if !DEBUG
            try
            {
#endif
            if (string.IsNullOrEmpty(e.ClickedItem.Name))
                return;

            if (e.ClickedItem.Name == _currentViewName)
                return;

            this._currentViewName = e.ClickedItem.Name;
            this.Text = string.Format("Tools - {0}", e.ClickedItem.Text.Replace("&", string.Empty));

            this.SaveCurrentControlState();

            this._currentControl = null;
            this.mainPanel.ClearControls();

            var newctrl = PluginManager.GetWinFormPlugin(this._currentViewName);
            if (newctrl != null)
            {
                this._currentControl = newctrl as HBDViewBase;
                //Load new control
                newctrl.LoadState(Properties.Settings.Default.State);
                this.mainPanel.AddControlWithFillDock(this._currentControl);
            }
#if !DEBUG
            }
            catch (Exception ex)
            {
                LogManager.Write(ex);
                Constants.ShowError(ex);
            }
#endif
        }

        private void SaveCurrentControlState()
        {
            //Save current control
            if (this._currentControl != null)
            {
                this._currentControl.SaveState(Properties.Settings.Default.State);
                Properties.Settings.Default.Save();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.SaveCurrentControlState();
            base.OnClosed(e);
        }
    }
}
