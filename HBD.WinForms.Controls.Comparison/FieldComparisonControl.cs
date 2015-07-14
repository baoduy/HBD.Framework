using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data;
using HBD.WinForms.Controls.Core;
using HBD.Framework.Data.Comparison;

namespace HBD.WinForms.Controls.Comparison
{
    [DefaultEvent( "SelectedChanged" ), DefaultProperty( "SelectedField" )]
    public partial class FieldComparisonControl : HBDControl
    {
        //bool _raiseSelectionChangesEvent = false;

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ), DefaultValue( null )]
        public object DataSourceA
        {
            get { return this.comboBoxA.DataSource; }
            set { this.comboBoxA.DataSource = value; }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ), DefaultValue( null )]
        public object DataSourceB
        {
            get { return this.comboBoxB.DataSource; }
            set { this.comboBoxB.DataSource = value; }
        }

        private string FieldA { get; set; }
        private string FieldB { get; set; }

        //Don't keep FieldComparison object as it may referenced by another control
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ), DefaultValue( null )]
        public FieldComparison SelectedField
        {
            get {
                if ( string.IsNullOrEmpty( this.FieldA ) )
                    this.FieldA = this.comboBoxA.Text;
                if ( string.IsNullOrEmpty( this.FieldB ) )
                    this.FieldB = this.comboBoxB.Text;

                return new FieldComparison( this.FieldA, this.FieldB ); 
            }
            set
            {
                if ( value != null && !value.IsEmpty() )
                {
                    this.FieldA = value.FieldA;
                    this.FieldB = value.FieldB;
                }
            }
        }

        [DefaultValue( false )]
        public bool CheckBoxEnable
        {
            get { return this.chEnable.Visible; }
            set { this.chEnable.Visible = value; }
        }

        public event EventHandler SelectedChanged;
        protected virtual void OnSelectedChanged( EventArgs e )
        {
            if ( this.SelectedChanged != null )
                this.SelectedChanged( this, e );
        }

        public FieldComparisonControl()
        {
            InitializeComponent();
        }

        public override void LoadControlData()
        {
            base.LoadControlData();
            //this._raiseSelectionChangesEvent = false;

            if ( string.IsNullOrEmpty( this.FieldA ) || string.IsNullOrEmpty( this.FieldB ) )
                return;
            if ( this.DataSourceA == null || this.DataSourceB == null )
                return;

            this.comboBoxA.SelectedItem = this.FieldA;
            this.comboBoxB.SelectedItem = this.FieldB;

            //this._raiseSelectionChangesEvent = true;
        }

        private void chEnable_CheckedChanged( object sender, EventArgs e )
        {
            this.comboBoxA.Enabled = this.comboBoxB.Enabled = this.chEnable.Checked;
        }

        protected override void OnSizeChanged( EventArgs e )
        {
            base.OnSizeChanged( e );

            this.Height = this.comboBoxA.Height + 5;
        }

        private void comboBox_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( sender == this.comboBoxA )
                this.FieldA = this.comboBoxA.Text;
            else this.FieldB = this.comboBoxB.Text;

            //if ( this._raiseSelectionChangesEvent )
                this.OnSelectedChanged( e );
        }
    }
}
