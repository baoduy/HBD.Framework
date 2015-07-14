using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Comparison.Core;
using HBD.WinForms.Controls.Comparison.Events;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Attributes;

namespace HBD.WinForms.Controls.Comparison
{
    [DefaultEvent("SelectChange")]
    public partial class MultiBrowserComparisonControl : ComparisonBrowser, IMultiBrowserComparisonControl
    {
        [ControlPropertyState]
        public string Title
        {
            get { return this.gr_Parent.Text; }
            set { this.gr_Parent.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(OpenBrowserType.ExcelOpenBrowser)]
        [ControlPropertyState]
        public OpenBrowserType OpenBrowserAType
        {
            get { return this.multiBrowserA.OpenBrowserType; }
            set { this.multiBrowserA.OpenBrowserType = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(OpenBrowserType.ExcelOpenBrowser)]
        [ControlPropertyState]
        public OpenBrowserType OpenBrowserBType
        {
            get { return this.multiBrowserB.OpenBrowserType; }
            set { this.multiBrowserB.OpenBrowserType = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type CustomBrowserControlAType
        {
            get { return this.multiBrowserA.CustomBrowserControlType; }
            set { this.multiBrowserA.CustomBrowserControlType = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type CustomBrowserControlBType
        {
            get { return this.multiBrowserB.CustomBrowserControlType; }
            set { this.multiBrowserB.CustomBrowserControlType = value; }
        }

        public MultiBrowserComparisonControl()
        {
            InitializeComponent();
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            this.multiBrowserA.SourcePath = this.OriginalSourceA;
            this.multiBrowserB.SourcePath = this.OriginalSourceB;

            this.multiBrowserA.LoadControlData();
            this.multiBrowserB.LoadControlData();
        }

        private void MultiBrowserComparisonControl_SelectChange(object sender, EventArgs e)
        {
            var control = ((IOpenBrowserConvertableControl)sender);
            var fileType = sender == this.multiBrowserB ? SelectedFileType.FileB : SelectedFileType.FileA;

            switch (fileType)
            {
                case SelectedFileType.FileA:
                    {
                        this.OriginalSourceA = control.SourcePath;
                        this.TableA = control.GetDataTable();
                    } break;
                case SelectedFileType.FileB:
                    {
                        this.OriginalSourceB = control.SourcePath;
                        this.TableB = control.GetDataTable();
                    } break;
            }

            this.OnSelectionChanged(new FileSelectedEventArgs(sender as IOpenBrowserConvertableControl, fileType));
        }
    }
}
