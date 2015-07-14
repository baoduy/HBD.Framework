using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HBD.Framework.Data.Comparison;
using HBD.WinForms.Controls.Comparison.Events;
using HBD.Framework.Data;
using System.Linq;
using HBD.WinForms.Controls.Core;
using HBD.WinForms.Controls.Attributes;
using HBD.WinForms.Controls.Events;
using HBD.Framework.Extension;

namespace HBD.WinForms.Controls.Comparison
{
    public partial class FieldComparisonCollectionControl : HBDControl
    {
        //bool _allowRaiseChangedEvent = false;

        public FieldComparisonCollectionControl()
        {
            InitializeComponent();
            this.SelectedFields = new FieldComparisonCollection();
            this.ListControls = new List<FieldComparisonControl>();
        }

        ColumnNamesCollection columnsA;
        ColumnNamesCollection columnsB;
        volatile FieldComparisonCollection selectedFields;

        [DefaultValue( null ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public ColumnNamesCollection DataSourceA
        {
            get { return this.columnsA; }
            set
            {
                this.columnsA = value;
                this.OnDataSourceChanged( EventArgs.Empty );
            }
        }

        [DefaultValue( null ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
        public ColumnNamesCollection DataSourceB
        {
            get { return this.columnsB; }
            set
            {
                this.columnsB = value;
                this.OnDataSourceChanged( EventArgs.Empty );
            }
        }

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden ), Browsable( false )]
        [ControlPropertyState]
        public FieldComparisonCollection SelectedFields
        {
            get
            {
                if ( this.selectedFields == null )
                    this.selectedFields = new FieldComparisonCollection();

                if ( this.selectedFields.Count == 0 && this.ListControls != null )
                {
                    foreach ( var c in this.ListControls )
                        this.selectedFields.Add( c.SelectedField );
                }

                return this.selectedFields;
            }
            set
            {
                if ( value != null )
                    this.selectedFields = value;
            }
        }

        private IList<FieldComparisonControl> ListControls { get; set; }

        [DefaultValue( null )]
        public string TitleA
        {
            get { return this.lbSheetA.Text; }
            set { this.lbSheetA.Text = value; }
        }

        [DefaultValue( null )]
        public string TitleB
        {
            get { return this.lbSheetB.Text; }
            set { this.lbSheetB.Text = value; }
        }

        [DefaultValue( null )]
        public string Header
        {
            get { return this.grColumns.Text; }
            set { this.grColumns.Text = value; }
        }

        [DefaultValue( true )]
        public bool ShowTitle
        {
            get { return this.tableLabel.Visible; }
            set { this.tableLabel.Visible = value; }
        }

        public event EventHandler DataSourceChanged;
        protected virtual void OnDataSourceChanged( EventArgs e )
        {
            #region Set Column

            if ( this.ListControls == null )
                return;

            foreach ( var item in this.ListControls )
            {
                item.DataSourceA = this.DataSourceA;
                item.DataSourceB = this.DataSourceB;
            }
            #endregion

            if ( this.DataSourceChanged != null )
                this.DataSourceChanged( this, e );
        }

        public event System.EventHandler<CompareFieldEventArgs> ColumnAdded;
        protected virtual void OnColumnAdded( CompareFieldEventArgs e )
        {
            if ( this.ColumnAdded != null )
                this.ColumnAdded( this, e );
        }

        public event System.EventHandler<CompareFieldEventArgs> ColumnRemoved;
        protected virtual void OnColumnRemoved( CompareFieldEventArgs e )
        {
            if ( this.ColumnRemoved != null )
                this.ColumnRemoved( this, e );
        }

        public override void CreateChildrenControl()
        {
            base.CreateChildrenControl();
            this.tableCompareColumns.ChildrenControlType = typeof( FieldComparisonControl );
        }

        public override void LoadControlData()
        {
            base.LoadControlData();

            if ( this.selectedFields == null || this.selectedFields.Count == 0 )
                return;

            this.DisableWithWaitCursor( true );
            //this._allowRaiseChangedEvent = false;
            this.SuspendLayout();
            this.tableLabel.Visible = false;

            this.Clear();
            foreach ( var f in this.selectedFields )
                this.Add( f );

            //this._allowRaiseChangedEvent = true;
            this.DisableWithWaitCursor( false );
            this.tableLabel.Visible = true;
            this.ResumeLayout( false );
        }

        FieldComparison addingField;
        /// <summary>
        /// Add compare column
        /// </summary>
        /// <param name="columnA"></param>
        /// <param name="columnB"></param>
        public void Add( FieldComparison field = null )
        {
            if ( field == null ) field = new FieldComparison();
            this.addingField = field;

            this.tableCompareColumns.AddNewControl();
        }

        /// <summary>
        /// Clear all compare columns
        /// </summary>
        public void Clear()
        {
            if ( this.ListControls != null )
            {
                this.tableCompareColumns.Clear();
                this.ListControls.Clear();
            }
        }

        public bool IsListControlEmpty()
        {
            return this.ListControls == null || this.ListControls.Count == 0;
        }

        //private void colCP_SelectedChanged( object sender, EventArgs e )
        //{
        //    if ( this.selectedFields != null )
        //        this.selectedFields.Clear();

        //    var control = sender as FieldComparisonControl;
        //    var key = this.ListControls.FirstOrDefault( c => c == control );
        //    key.Value.FieldA = control.SelectedField.FieldA;
        //    key.Value.FieldB = control.SelectedField.FieldB;
        //}

        public virtual void EnableChildControls( bool enabled )
        {
            if ( this.IsListControlEmpty() )
                return;

            foreach ( var c in this.ListControls )
                c.Enabled = enabled;
        }

        private void listControlCollection_ChildrenControlAdded( object sender, ListControlCollectionEventArgs e )
        {
            var colCP = e.Control as FieldComparisonControl;

            if ( this.DataSourceA == null || this.DataSourceB == null ) return;

            if ( ( this.addingField == null || addingField.IsEmpty() ) && this.ListControls.Count > 0 )
                addingField = this.ListControls[this.ListControls.Count - 1].SelectedField;

            colCP.DataSourceA = this.DataSourceA.Copy();
            colCP.DataSourceB = this.DataSourceB.Copy();
            colCP.SelectedField = addingField;

            this.ListControls.Add( colCP );
            this.addingField = null;

            colCP.AutoLoadChildrenControlData = this.AutoLoadChildrenControlData;
            if ( !this.AutoLoadChildrenControlData )
            {
                colCP.CreateChildrenControl();
                colCP.LoadControlData();
            }

            if ( this.selectedFields != null )
                this.selectedFields.Clear();

            //if ( this._allowRaiseChangedEvent )
            //{
            this.OnColumnAdded( new CompareFieldEventArgs( colCP ) );
            //}
        }

        private void listControlCollection_ChildrenControlRemoved( object sender, ListControlCollectionEventArgs e )
        {
            var c = this.ListControls.FirstOrDefault( f => f == e.Control );
            if ( c != null )
                this.ListControls.Remove( c );

            if ( this.selectedFields != null )
                this.selectedFields.Clear();
        }

        protected override void SuspendLayout()
        {
            if ( this.tableCompareColumns != null )
                this.tableCompareColumns.SuspendLayout();

            base.SuspendLayout();
        }
        public override void ResumeLayout()
        {
            base.ResumeLayout();
            if ( this.tableCompareColumns != null )
                this.tableCompareColumns.ResumeLayout();
        }
        public override void ResumeLayout( bool performLayout )
        {
            base.ResumeLayout( performLayout );
            if ( this.tableCompareColumns != null )
                this.tableCompareColumns.ResumeLayout( performLayout );
        }
    }
}
