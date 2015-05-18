using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls;

namespace HBD.WinForms.Controls
{
    public partial class HBDForm : Form
    {
        public HBDForm()
        {
            InitializeComponent();
        }

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
    }
}
