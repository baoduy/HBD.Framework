using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Core;
using HBD.Libraries.Unity;
using HBD.WinForms.Controls.Attributes;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("SelectChange")]
    public partial class MultiBrowserControl : HBDControl, IOpenBrowserConvertableControl, IMultiBrowserControl
    {
        private HBDControl CreateControl(OpenBrowserType controlType, Type customType)
        {
            HBDControl control = null;

            switch (controlType)
            {
                case OpenBrowserType.CSVOpenBrowser:
                    control = new CSVOpenBrowser(); break;
                case OpenBrowserType.XMLOpenBrowser:
                    control = new XMLOpenBrowser(); break;
                case OpenBrowserType.Custom:
                    {
                        if (this.DesignMode)
                        {
                            control = null;
                            break;
                        }
                        Guard.ArgumentNotNull(customType, "Custom type of Browser");
                        control = UnityManager.Container.RegisterResolve(customType) as HBDControl;
                    } break;
                default:
                    control = new ExcelOpenBrowser(); break;
            }

            if (control == null) return null;

            control.AutoLoadChildrenControlData = this.AutoLoadChildrenControlData;
            if (!this.AutoLoadChildrenControlData)
                control.CreateChildrenControl();

            if (!(control is IOpenBrowserConvertableControl))
            {
                control.Dispose();
                throw new ArgumentException("Browser control should be an instance of IOpenBrowser");
            }

            ((IOpenBrowserConvertableControl)control).SelectChange += new EventHandler(MultiBrowserComparisonControl_SelectChange);
            return control;

        }

        OpenBrowserType _currentOpenBrowserType = OpenBrowserType.None;
        OpenBrowserType _openBrowserType = OpenBrowserType.ExcelOpenBrowser;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(OpenBrowserType.ExcelOpenBrowser)]
        public OpenBrowserType OpenBrowserType
        {
            get { return _openBrowserType; }
            set
            {
                _openBrowserType = value;
                this.CreateChildrenControl();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Type CustomBrowserControlType { get; set; }

        public UserControl Control { get; private set; }

        public MultiBrowserControl()
        {
            InitializeComponent();
        }

        private void MultiBrowserComparisonControl_SelectChange(object sender, EventArgs e)
        {
            var ctrl = sender as IOpenBrowserConvertableControl;
            if (ctrl != null)
            {
                this.SourcePath = ctrl.SourcePath;
                this.SourceName = ctrl.SourceName;
            }
            this.OnSelectChange(e);
        }

        public override void CreateChildrenControl()
        {
            base.CreateChildrenControl();

            if (this.OpenBrowserType == WinForms.Controls.OpenBrowserType.None)
            {
                this.Controls.Clear();
                return;
            }

            if (this._currentOpenBrowserType == this.OpenBrowserType)
                return;
            this._currentOpenBrowserType = this.OpenBrowserType;

            this.SuspendLayout();
            this.Controls.Clear();

            if (this.Control != null)
            {
                this.Control.Dispose();
                this.Control = null;
            }

            this.Control = this.CreateControl(this.OpenBrowserType, this.CustomBrowserControlType);

            this.Controls.Add(this.Control);
            this.Control.Margin = new Padding(0, 0, 0, 0);
            this.Control.Dock = DockStyle.Fill;

            this.ResumeLayout(true);
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            if (this.Control != null)
            {
                var ctrl = this.Control as IOpenBrowserConvertableControl;
                ctrl.SourceName = this.SourceName;
                ctrl.SourcePath = this.SourcePath;

                if (this.Control is HBDControl)
                    ((HBDControl)this.Control).LoadControlData();
            }
        }

        public override bool ValidateData()
        {
            if (this.Control == null)
                return false;
            return this.Control.Validate();
        }

        #region IOpenBrowserConvertable

        public DataTable GetDataTable()
        {
            if (!this.ValidateData())
                return null;
            return ((IOpenBrowserConvertableControl)this.Control).GetDataTable();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ControlPropertyState]
        public string SourcePath { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ControlPropertyState]
        public string SourceName { get; set; }

        public event EventHandler SelectChange;
        protected virtual void OnSelectChange(EventArgs e)
        {
            if (this.SelectChange != null)
                this.SelectChange(this, e);
        }
        #endregion
    }
}
