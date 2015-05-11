using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Attributes;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("ValueChanges")]
    public partial class LoginControl : HBDControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        [ControlPropertyState]
        public string UserName
        {
            get { return this.txt_UserName.Text; }
            set { this.txt_UserName.Text = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        [ControlPropertyState]
        public string Password
        {
            get { return this.txt_Password.Text; }
            set { this.txt_Password.Text = value; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        [ControlPropertyState]
        public bool RememberPassword
        {
            get { return this.ch_RememberPassword.Checked; }
            set { this.ch_RememberPassword.Checked = value; }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("Remember password")]
        public string RememberPasswordText
        {
            get { return this.ch_RememberPassword.Text; }
            set { this.ch_RememberPassword.Text = value; }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public bool ShowRememberPassword
        {
            get { return this.ch_RememberPassword.Visible; }
            set { this.ch_RememberPassword.Visible = value; }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue("Remember password")]
        public string LoginText
        {
            get { return this.bt_Login.Text; }
            set { this.bt_Login.Text = value; }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public bool ShowLoginButton
        {
            get { return this.bt_Login.Visible; }
            set { this.bt_Login.Visible = value; }
        }

        public event System.EventHandler Login;
        protected virtual void OnLogin(EventArgs e)
        { if (this.Login != null)this.Login(this, e); }

        /// <summary>
        /// User name and password changes
        /// </summary>
        public event System.EventHandler ValueChanges;
        protected virtual void OnValueChanges(EventArgs e)
        { if (this.ValueChanges != null)this.ValueChanges(this, e); }

        public override bool ValidateData()
        {
            return this.ValidateControls(this.txt_UserName, this.txt_Password);
        }

        private void bt_Login_Click(object sender, EventArgs e)
        {
            if (this.ValidateData())
                this.OnLogin(e);
        }

        private void txt_Password_TextChanged(object sender, EventArgs e)
        {
            this.OnValueChanges(e);
        }
    }
}
