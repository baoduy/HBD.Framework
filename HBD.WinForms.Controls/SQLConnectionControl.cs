using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using HBD.Framework.Data.SQL;
using HBD.WinForms.Controls.Attributes;
using HBD.WinForms.Controls.Core;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("Connect"), DefaultProperty("ConnectionString")]
    public partial class SQLConnectionControl : HBDControl
    {
        public SQLConnectionControl()
        {
            InitializeComponent();
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        SqlConnectionStringBuilder _connectionString = null;
        public SqlConnectionStringBuilder ConnectionStringBuilder
        {
            get
            {
                if (this._connectionString == null)
                    this._connectionString = new SqlConnectionStringBuilder();
                this._connectionString.DataSource = this.txt_ServerName.Text;

                if (!string.IsNullOrEmpty(this.cb_DBName.Text))
                    this._connectionString.InitialCatalog = this.cb_DBName.Text;

                if (this.ch_SQL.Checked)
                {
                    this._connectionString.UserID = this.loginControl.UserName;
                    this._connectionString.Password = this.loginControl.Password;
                    this._connectionString.IntegratedSecurity = false;
                }
                else
                    this._connectionString.IntegratedSecurity = true;
                return _connectionString;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        [ControlPropertyState]
        public string ConnectionString
        {
            get { return this.ConnectionStringBuilder.ConnectionString; }
            set { this.ConnectionStringBuilder.ConnectionString = value; }
        }

        bool _isConnected = false;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsConnected
        {
            get { return _isConnected; }
            private set
            {
                _isConnected = value;
                this.CheckControlEnable();
            }
        }

        public override void LoadControlData()
        {
            base.LoadControlData();

            if (this._connectionString != null)
            {
                this.txt_ServerName.Text = this.ConnectionStringBuilder.DataSource;
                this.LoadDBName();
                this.cb_DBName.Text = this.ConnectionStringBuilder.InitialCatalog;

                if (!this.ConnectionStringBuilder.IntegratedSecurity)
                {
                    this.ch_SQL.Checked = true;
                    this.loginControl.UserName = this.ConnectionStringBuilder.UserID;
                    this.loginControl.Password = this.ConnectionStringBuilder.Password;
                }
                else this.ch_Window.Checked = true;
            }
        }

        private void CheckControlEnable()
        {
            this.txt_ServerName.Enabled = !this.IsConnected;
            this.loginControl.Enabled = !this.IsConnected && this.ch_SQL.Checked;
            this.ch_SQL.Enabled = this.ch_Window.Enabled = !this.IsConnected;
            this.loginControl.Enabled = this.ch_SQL.Checked;

            if (this.ch_Window.Checked)
                this.cb_DBName.Enabled = !this.IsConnected && !string.IsNullOrEmpty(this.txt_ServerName.Text);
            else
                this.cb_DBName.Enabled = !this.IsConnected && !string.IsNullOrEmpty(this.txt_ServerName.Text) && this.loginControl.ValidateData(false);

            this.bt_Test.Enabled = this.cb_DBName.Enabled;
            this.bt_Connect.Enabled = !string.IsNullOrEmpty(this.txt_ServerName.Text);
            this.bt_Connect.Text = this.IsConnected ? "Disconnect" : "Connect";
        }

        public override bool ValidateData()
        {
            if (!base.ValidateData()) return false;
            if (!this.ValidateControls(this.txt_ServerName)) return false;
            if (this.ch_SQL.Checked && !this.ValidateControls(this.loginControl)) return false;
            return true;
        }

        SQLAdapter _connection = null;
        private void OpenConnection()
        {
            if (this._connection == null)
                this._connection = new SQLAdapter();

            this._connection.ConnectionString = this.ConnectionString;
            this._connection.Open();
        }

        public virtual SQLAdapter GetAdapter()
        {
            if (this.TestConnection(false))
                return this._connection;
            return null;
        }
        private bool TestConnection(bool showMessage = true)
        {
            if (!this.ValidateData()) return false;
            this.DisableWithWaitCursor(true);

            try { this.OpenConnection(); }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
                return false;
            }
            finally { this.DisableWithWaitCursor(false); }

            if (showMessage)
                this.ShowInfoMessage("Open connection suscessfully");
            return true;
        }
        private void DisposeConnection()
        {
            if (this._connection != null)
            {
                this._connection.Dispose();
                this._connection = null;
                this.IsConnected = false;
            }
        }
        private void LoadDBName()
        {
            if (this.cb_DBName.Items.Count > 0) return;
            if (!this.ValidateData()) return;

            try
            {
                this.OpenConnection();

                var dbNames = this._connection.GetAllDatabaseNames();
                if (dbNames != null && dbNames.Length > 0)
                {
                    this.cb_DBName.Items.AddRange(dbNames);
                    this.cb_DBName.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage(ex);
            }
        }

        public event EventHandler Connect;
        protected virtual void OnConnect(EventArgs e)
        { if (this.Connect != null)this.Connect(this, e); }
        private void ch_Window_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckControlEnable();
            this.ClearDBNameItem();
        }

        private void bt_Test_Click(object sender, EventArgs e)
        {
            TestConnection();
        }

        private void bt_Connect_Click(object sender, EventArgs e)
        {
            if (this.IsConnected)
            {
                this.DisposeConnection();
                return;
            }

            if (this.TestConnection(false))
                this.IsConnected = true;
            this.OnConnect(e);
        }

        private void cb_DBName_DropDown(object sender, EventArgs e)
        {
            this.LoadDBName();
        }

        private void cb_DBName_KeyUp(object sender, KeyEventArgs e)
        {
            this.LoadDBName();
        }
        private void txt_ServerName_TextChanged(object sender, EventArgs e)
        {
            this.CheckControlEnable();
        }

        private void loginControl_ValueChanges(object sender, EventArgs e)
        {
            this.CheckControlEnable();
            this.ClearDBNameItem();
        }

        private void ClearDBNameItem()
        {
            this.cb_DBName.Items.Clear();
            this.cb_DBName.Text = string.Empty;
        }
    }
}
