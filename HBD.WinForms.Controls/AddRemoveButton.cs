using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls.Core;
using System.Drawing.Design;

namespace HBD.WinForms.Controls
{
    [DefaultEvent("Click")]
    public partial class AddRemoveButton : UserControl, IAddRemoveButton
    {
        public AddRemoveButton()
        {
            InitializeComponent();
            this.MinimumSize = new Size(30, 15);
        }

        #region Button Properties
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoEllipsis
        {
            get { return this.bt_Add.AutoEllipsis; }
            set
            {
                this.bt_Add.AutoEllipsis = value;
                this.bt_Remove.AutoEllipsis = value;
            }
        }

        [Localizable(true)]
        [DefaultValue(FlatStyle.Standard)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FlatStyle FlatStyle
        {
            get { return this.bt_Add.FlatStyle; }
            set
            {
                this.bt_Add.FlatStyle = value;
                this.bt_Remove.FlatStyle = value;
            }
        }

        [Localizable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ContentAlignment ImageAlign
        {
            get { return this.bt_Add.ImageAlign; }
            set
            {
                this.bt_Add.ImageAlign = value;
                this.bt_Remove.ImageAlign = value;
            }
        }

        [Localizable(true)]
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image AddImage
        {
            get { return this.bt_Add.Image; }
            set { this.bt_Add.Image = value; }
        }

        [Localizable(true)]
        [DefaultValue("")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image RemoveImage
        {
            get { return this.bt_Remove.Image; }
            set { this.bt_Remove.Image = value; }
        }

        [TypeConverter(typeof(ImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [DefaultValue(-1)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int AddImageIndex
        {
            get { return this.bt_Add.ImageIndex; }
            set { this.bt_Add.ImageIndex = value; }
        }

        [TypeConverter(typeof(ImageKeyConverter))]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string AddImageKey
        {
            get { return this.bt_Add.ImageKey; }
            set { this.bt_Add.ImageKey = value; }
        }

        [TypeConverter(typeof(ImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [DefaultValue(-1)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int RemoveImageIndex
        {
            get { return this.bt_Remove.ImageIndex; }
            set { this.bt_Remove.ImageIndex = value; }
        }

        [TypeConverter(typeof(ImageKeyConverter))]
        [Localizable(true)]
        [DefaultValue("")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string RemoveImageKey
        {
            get { return this.bt_Remove.ImageKey; }
            set { this.bt_Remove.ImageKey = value; }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue("")]
        public ImageList ImageList
        {
            get { return this.bt_Add.ImageList; }
            set
            {
                this.bt_Add.ImageList = value;
                this.bt_Remove.ImageList = value;
            }
        }

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [SettingsBindable(true), DefaultValue("+")]
        public virtual string AddText
        {
            get { return this.bt_Add.Text; }
            set { this.bt_Add.Text = value; }
        }

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [SettingsBindable(true), DefaultValue("-")]
        public virtual string RemoveText
        {
            get { return this.bt_Remove.Text; }
            set { this.bt_Remove.Text = value; }
        }

        [Localizable(true)]
        public virtual ContentAlignment TextAlign
        {
            get { return this.bt_Add.TextAlign; }
            set
            {
                this.bt_Add.TextAlign = value;
                this.bt_Remove.TextAlign = value;
            }
        }

        [Localizable(true)]
        public TextImageRelation TextImageRelation
        {
            get { return this.bt_Add.TextImageRelation; }
            set
            {
                this.bt_Add.TextImageRelation = value;
                this.bt_Remove.TextImageRelation = value;
            }
        }
        #endregion

        #region Others Properties
        IListControlCollection collectionControl;
        [DefaultValue(null)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public IListControlCollection CollectionControl
        {
            get { return this.collectionControl; }
            set
            {
                if (this.collectionControl != value)
                {
                    if (this.collectionControl != null)
                    {
                        this.collectionControl.ChildrenControlAdded -= control_ChildrenControlAdded;
                        this.collectionControl.ChildrenControlRemoved -= control_ChildrenControlRemoved;
                        this.collectionControl.AllowancePropertiesChanged -= control_AllowancePropertiesChanged;
                    }

                    this.collectionControl = value;
                    this.collectionControl.ChildrenControlAdded += control_ChildrenControlAdded;
                    this.collectionControl.ChildrenControlRemoved += control_ChildrenControlRemoved;
                    this.collectionControl.AllowancePropertiesChanged += control_AllowancePropertiesChanged;
                }
            }
        }

        private void CheckButtonsEnability()
        {
            if (this.CollectionControl == null)
                return;

            this.bt_Remove.Enabled = this.CollectionControl.ChildrenControls.Count > 0 && this.CollectionControl.AllowRemoveControl;
            this.bt_Add.Enabled = this.CollectionControl.AllowAddControl;
        }

        private void control_AllowancePropertiesChanged(object sender, EventArgs e)
        {
            this.CheckButtonsEnability();
        }

        private void control_ChildrenControlRemoved(object sender, EventArgs e)
        {
            this.CheckButtonsEnability();
        }

        private void control_ChildrenControlAdded(object sender, Events.ListControlCollectionEventArgs e)
        {
            this.CheckButtonsEnability();
        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.CheckButtonsEnability();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            this.bt_Add.BackColor = this.BackColor;
            this.bt_Remove.BackColor = this.BackColor;
        }

        private void bt_Remove_Click(object sender, EventArgs e)
        {
            if (this.CollectionControl != null)
                this.CollectionControl.RemoveControl();
            this.OnClick(new AddRemoveButtonEventArgs(AddRemoveButtonAction.Remove));
        }

        private void bt_Add_Click(object sender, EventArgs e)
        {
            if (this.CollectionControl != null)
                this.CollectionControl.AddNewControl();
            this.OnClick(new AddRemoveButtonEventArgs(AddRemoveButtonAction.Add));
        }

        public new event EventHandler<AddRemoveButtonEventArgs> Click;
        protected virtual void OnClick(AddRemoveButtonEventArgs e)
        {
            if (this.Click != null)
                this.Click(this, e);
        }
    }

    public class AddRemoveButtonEventArgs : EventArgs
    {
        public AddRemoveButtonAction ButtonType { get; private set; }
        public AddRemoveButtonEventArgs(AddRemoveButtonAction buttonType)
        { this.ButtonType = buttonType; }
    }

    public enum AddRemoveButtonAction { Add, Remove }
}
