using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.ControlStates;
using HBD.Framework.Extension;
using System.ComponentModel;
using HBD.Framework.Extension.WinForms;

namespace HBD.WinForms.Controls.Core
{
    public class HBDControl : UserControl, IHBDControl
    {
        public HBDControl()
        {
            this.InitializeComponent();
            this.AutoLoadChildrenControlData = true;
            this.IsControlDataLoaded = false;
            this.IsCreateChildrenControlCreated = false;
        }
        private ErrorProvider errorProvider;

        #region Properties
        /// <summary>
        /// Auto execute CreateChildrenControl and LoadControlData method OnLoad
        /// </summary>
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public bool AutoLoadChildrenControlData { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsCreateChildrenControlCreated { get; protected set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsControlDataLoaded { get; protected set; }

        //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public ErrorProvider ErrorProvider
        //{
        //    get { return errorProvider; }
        //}

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsLayoutSuspended
        {
            get { return this.IsLayoutSuspended(); }
        }
        #endregion

        #region Public methods
        public virtual void DisableWithWaitCursor(bool disabled)
        {
            this.Enabled = !disabled;
            if (this.ParentForm != null)
                this.ParentForm.UseWaitCursor = disabled;
        }

        protected void SetError(Control control, string message)
        {
            this.errorProvider.SetError(control, message);
        }
        protected void ClearError()
        {
            this.errorProvider.Clear();
        }
        public bool ValidateData(bool showErrorMessage)
        {
            var val = this.ValidateData();
            if (!showErrorMessage) this.errorProvider.Clear();
            return val;
        }
        protected bool ValidateControls(params Control[] controls)
        {
            this.errorProvider.Clear();

            foreach (var c in controls)
            {
                var validated = true;
                if (c is HBDControl)
                    validated = ((HBDControl)c).ValidateData();
                else
                {
                    var defaultVal = c.GetDefaultValue();
                    if (defaultVal == null)
                        validated = false;
                    else validated = !string.IsNullOrEmpty(defaultVal.ToString());
                }

                if (!validated)
                {
                    if (!(c is HBDControl))
                        this.errorProvider.SetError(c, "Value cannot be empty.");
                    return false;
                }
            }
            return true;
        }
        public virtual bool ValidateData()
        { return true; }
        public virtual void LoadControlData()
        {
            //Ensure children control is loaded
            if (!IsCreateChildrenControlCreated)
                this.CreateChildrenControl();

            this.IsControlDataLoaded = true;
        }
        public virtual void CreateChildrenControl() { this.IsCreateChildrenControlCreated = true; }

        public virtual new void ResumeLayout()
        { base.ResumeLayout(); }

        public virtual new void ResumeLayout(bool performLayout)
        { base.ResumeLayout(performLayout); }
        #endregion

        #region Override Events
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (!this.IsCreateChildrenControlCreated)
            {
                this.CreateChildrenControl();
                this.IsCreateChildrenControlCreated = true;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //Deactivate AutoLoadChildrenControlData when Parent control is HBDControl
            if (this.Parent is HBDControl)
                this.AutoLoadChildrenControlData = false;

            //Do not load Data in Design Mode.
            //Do not load Data if AutoLoadChildrenControlData is deactivated.
            //Do not load data agian once IsControlDataLoaded is true.
            if (!this.DesignMode && this.AutoLoadChildrenControlData && !this.IsControlDataLoaded)
            {
                this.LoadControlData();
                this.IsControlDataLoaded = true;
            }
        }
        protected virtual new void SuspendLayout()
        { base.SuspendLayout(); }
        #endregion

        #region Show Message
        protected virtual void ShowErrorMessage(Exception exception)
        {
            this.ShowErrorMessage(exception.Message);
        }
        protected virtual void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error Messge", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected virtual void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected virtual DialogResult ShowConfirmationMessage(string message)
        {
            return MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        #endregion

        private System.ComponentModel.IContainer components;
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // HBDControl
            // 
            this.Name = "HBDControl";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
