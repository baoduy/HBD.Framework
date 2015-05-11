using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Utilities;
using HBD.Framework.Data.Utilities;

namespace HBD.WinForms.Controls
{
    public partial class DataGridViewSearchControl : HBDControl
    {
        public DataGridViewSearchControl()
            : this(null) { }

        public DataGridViewSearchControl(ISearchable searchableControl)
        {
            InitializeComponent();
            this.SearchableControl = searchableControl;
            this.Enabled = false;
            this.MinimumSize = new Size(200, 20);
        }

        ISearchable searchableControl;
        [DefaultValue("(none)"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ISearchable SearchableControl
        {
            get { return searchableControl; }
            set
            {
                if (this.searchableControl != value)
                {
                    if (this.searchableControl != null)
                    {
                        this.SearchableControl.SearchStatusChanged -= SearchableControl_SearchStatusChanged;
                        this.searchableControl.ItemsChanged -= SearchableControl_ItemsChanged;
                    }

                    searchableControl = value;
                    this.SearchableControl.SearchStatusChanged += SearchableControl_SearchStatusChanged;
                    this.searchableControl.ItemsChanged += SearchableControl_ItemsChanged;
                }
            }
        }

        private void SearchableControl_SearchStatusChanged(object sender, Events.SearchlEventArgs e)
        {
            switch (e.SearchManager.Status)
            {
                case SearchStatus.None:
                    this.bt_Back.Visible = false; break;
                case SearchStatus.Started:
                    {
                        this.bt_Stop.Visible = true;
                        this.txt_Keyword.Enabled = false;
                    } break;
                case SearchStatus.ResultFound:
                    this.DisableWithWaitCursor(false); break;
                case SearchStatus.Completed:
                    {
                        this.DisableWithWaitCursor(false);
                        this.bt_Stop.Visible = false;
                        this.txt_Keyword.Enabled = true;
                    } break;
                default:
                    break;
            }
        }

        private void SearchableControl_ItemsChanged(object sender, EventArgs e)
        {
            this.Enabled = this.SearchableControl.ItemCount > 0;
        }

        public void Search()
        {
            if (this.SearchableControl == null)
                return;

            if (this.SearchableControl.SearchManager.Status == SearchStatus.None)
            {
                this.DisableWithWaitCursor(true);
                this.SearchableControl.Search(this.txt_Keyword.Text);
            }
            else
            {
                this.bt_Search.Enabled = this.SearchableControl.SearchManager.Next();
                this.bt_Back.Visible = true;
                this.bt_Back.Enabled = true;
            }
        }

        public void Stop()
        {
            if (this.SearchableControl == null)
                return;
            this.SearchableControl.SearchManager.Stop();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Height = this.txt_Keyword.Height + (this.txt_Keyword.Margin.All * 2);
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void txt_Keyword_TextChanged(object sender, EventArgs e)
        {
            if (this.SearchableControl != null)
            {
                //Reset the search
                this.SearchableControl.SearchManager.Reset();
            }
        }

        private void txt_Keyword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                this.Search();
        }

        private void bt_Stop_Click(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void bt_Back_Click(object sender, EventArgs e)
        {
            if (this.SearchableControl.SearchManager.Status != SearchStatus.None)
                this.bt_Back.Enabled = this.SearchableControl.SearchManager.Previous();
            this.bt_Search.Enabled = this.searchableControl.SearchManager.Total > 0;
        }
    }
}
