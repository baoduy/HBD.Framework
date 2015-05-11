using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data.Sharepoint;
using HBD.WinForms.Controls.Sharepoint.Libraries;
using HBD.WinForms.Controls.Core;

namespace HBD.WinForms.Controls.Sharepoint
{
    [DefaultEvent("Selected")]
    public partial class SPAllSiteContentTreeControl : HBDControl
    {
        const string _TowValuesFormat = "{0}_{1}";
        const string _ListCollection = "Lists";
        const string _List = "List";
        const string _View = "View";
        const string _Site = "Site";
        const string _SiteCollection = "SubSites";
        const string _GroupPermission = "Group Permission";
        const string _GroupPermissionCollection = "Groups Permission";
        const string _Account = "Account";

        static readonly object _IsLoaded = new object();
        public SPAllSiteContentTreeControl()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.OnSizeChanged(e);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.data_URL.Width = this.toolStrip1.Width - this.bt_Open.Width - this.lb_URL.Width - 18;
        }
        private void bt_Open_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.data_URL.Text))
            {
                this.data_URL.Focus();
                return;
            }

            this.DisableWithWaitCursor(true);

            var site = this.LoadSite(this.data_URL.Text);
            this.treeView.Nodes.Add(site);

            this.DisableWithWaitCursor(false);
            this.OnSourceChanged(e);
        }
        private SPTreeNode LoadSite(string url)
        {
            var adapter = SPAdapterManager.Open(url);
            var site = new SPSiteTreeNode() { SPAdapter = adapter, Name = url, Text = adapter.Title, ImageIndex = 0, SelectedImageIndex = 0, Tag = _IsLoaded };
            var list = new SPListCollectionTreeNode() { Name = _ListCollection, Text = _ListCollection, ImageIndex = 1, SelectedImageIndex = 1 };
            var subsite = new SPSiteCollectionTreeNode() { Name = _SiteCollection, Text = _SiteCollection, ImageIndex = 0, SelectedImageIndex = 0 };
            var groups = new SPGroupPermissionCollection() { Name = _GroupPermissionCollection, Text = _GroupPermissionCollection, ImageIndex = 3, SelectedImageIndex = 3 };

            site.Nodes.Add(list);
            site.Nodes.Add(subsite);
            site.Nodes.Add(groups);

            return site;
        }
        private void LoadLists(SPTreeNode listsNode)
        {
            if (listsNode == null) return;
            listsNode.Tag = _IsLoaded;
            //Load List
            foreach (var l in listsNode.SPAdapter.ListTitles)
            {
                var lt = new SPListTreeNode() { Name = string.Format(_TowValuesFormat, _List, l), Text = l, ImageIndex = 1, SelectedImageIndex = 1, };
                listsNode.Nodes.Add(lt);
            }
        }
        private void LoadViews(SPTreeNode listNode)
        {
            if (listNode == null) return;
            listNode.Tag = _IsLoaded;

            foreach (var v in listNode.SPAdapter.GetViewTitles(listNode.Text))
            {
                var vt = new SPViewTreeNode() { Name = string.Format(_TowValuesFormat, _View, v), Text = v, ImageIndex = 2, SelectedImageIndex = 2 };
                listNode.Nodes.Add(vt);
            }
        }
        private void LoadSubSites(SPTreeNode subSites)
        {
            if (subSites == null) return;
            subSites.Tag = _IsLoaded;

            foreach (var s in subSites.SPAdapter.SubSites)
            {
                var subSiteURL = subSites.SPAdapter.SiteURL + s.Key;
                var site = this.LoadSite(subSiteURL);
                subSites.Nodes.Add(site);
            }
        }
        private void LoadGroups(SPTreeNode groups)
        {
            if (groups == null) return;
            groups.Tag = _IsLoaded;

            foreach (var g in groups.SPAdapter.SiteGroups)
            {
                var vt = new SPGroupPermission() { Name = string.Format(_TowValuesFormat, _GroupPermission, g), Text = g, ImageIndex = 3, SelectedImageIndex = 3 };
                groups.Nodes.Add(vt);
            }
        }
        //private void LoadAccounts(SPTreeNode group)
        //{
        //    if (group == null) return;
        //    group.Tag = _IsLoaded;

        //    foreach (var a in group.SPAdapter.GetAccounts(group.Text))
        //    {
        //        var acc = new SPAccountTreeNode() { Name = string.Format(_TowValuesFormat, _GroupPermission, a), Text = a, ImageIndex = 4, SelectedImageIndex = 4 };
        //        group.Nodes.Add(acc);
        //    }
        //}
        private void data_URL_TextChanged(object sender, EventArgs e)
        {
            this.treeView.Nodes.Clear();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0 || e.Node.Tag == _IsLoaded)
                return;

            this.DisableWithWaitCursor(true);

            if (e.Node.Name == _ListCollection)
                this.LoadLists(e.Node as SPTreeNode);
            else if (e.Node.Name.StartsWith(_List))
                this.LoadViews(e.Node as SPTreeNode);
            else if (e.Node.Name == _SiteCollection)
                this.LoadSubSites(e.Node as SPTreeNode);
            else if (e.Node.Name == _GroupPermissionCollection)
                this.LoadGroups(e.Node as SPTreeNode);
            //else if (e.Node.Name.StartsWith(_GroupPermission))
            //    this.LoadAccounts(e.Node as SPTreeNode);

            this.DisableWithWaitCursor(false);

            this.OnSelected(new SPTreeNodeEventArgs(e.Node as SPTreeNode));
        }

        /// <summary>
        /// Event of selection is changed
        /// </summary>
        public event EventHandler<SPTreeNodeEventArgs> Selected;
        protected virtual void OnSelected(SPTreeNodeEventArgs e)
        {
            if (this.Selected != null)
                this.Selected(this, e);
        }

        /// <summary>
        /// Event of URL changed
        /// </summary>
        public event EventHandler SourceChanged;
        protected virtual void OnSourceChanged(EventArgs e)
        {
            if (this.SourceChanged != null)
                this.SourceChanged(this, e);
        }
    }
}
