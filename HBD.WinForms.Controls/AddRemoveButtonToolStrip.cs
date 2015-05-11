using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using HBD.WinForms.Controls.Core;
using System.Drawing.Design;

namespace HBD.WinForms.Controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class AddRemoveButtonToolStrip : HBDToolStripControlHost<AddRemoveButton>, IAddRemoveButton
    {
        [DefaultValue(""), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public IListControlCollection CollectionControl
        {
            get { return this.ChildControl.CollectionControl; }
            set { this.ChildControl.CollectionControl = value; }
        }

        #region Button Properties
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        public bool AutoEllipsis
        {
            get { return this.ChildControl.AutoEllipsis; }
            set { this.ChildControl.AutoEllipsis = value; }
        }

        [Localizable(true)]
        [DefaultValue(FlatStyle.Standard)]
        public FlatStyle FlatStyle
        {
            get { return this.ChildControl.FlatStyle; }
            set { this.ChildControl.FlatStyle = value; }
        }

        [Localizable(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new ContentAlignment ImageAlign
        {
            get { return this.ChildControl.ImageAlign; }
            set { this.ChildControl.ImageAlign = value; }
        }

        [Localizable(true)]
        [DefaultValue("")]
        public Image AddImage
        {
            get { return this.ChildControl.AddImage; }
            set { this.ChildControl.AddImage = value; }
        }

        [Localizable(true)]
        [DefaultValue("")]
        public Image RemoveImage
        {
            get { return this.ChildControl.RemoveImage; }
            set { this.ChildControl.RemoveImage = value; }
        }

        [TypeConverter(typeof(ImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [DefaultValue(-1)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int AddImageIndex
        {
            get { return this.ChildControl.AddImageIndex; }
            set { this.ChildControl.AddImageIndex = value; }
        }

        [TypeConverter(typeof(ImageKeyConverter))]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string AddImageKey
        {
            get { return this.ChildControl.AddImageKey; }
            set { this.ChildControl.AddImageKey = value; }
        }

        [TypeConverter(typeof(ImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [DefaultValue(-1)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int RemoveImageIndex
        {
            get { return this.ChildControl.RemoveImageIndex; }
            set { this.ChildControl.RemoveImageIndex = value; }
        }

        [TypeConverter(typeof(ImageKeyConverter))]
        [Localizable(true)]
        [DefaultValue("")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string RemoveImageKey
        {
            get { return this.ChildControl.RemoveImageKey; }
            set { this.ChildControl.RemoveImageKey = value; }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue("")]
        public ImageList ImageList
        {
            get { return this.ChildControl.ImageList; }
            set { this.ChildControl.ImageList = value; }
        }

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [SettingsBindable(true), DefaultValue("+")]
        public virtual string AddText
        {
            get { return this.ChildControl.AddText; }
            set { this.ChildControl.AddText = value; }
        }

        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [SettingsBindable(true), DefaultValue("-")]
        public virtual string RemoveText
        {
            get { return this.ChildControl.RemoveText; }
            set { this.ChildControl.RemoveText = value; }
        }

        [Localizable(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new virtual ContentAlignment TextAlign
        {
            get { return this.ChildControl.TextAlign; }
            set { this.ChildControl.TextAlign = value; }
        }

        [Localizable(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new TextImageRelation TextImageRelation
        {
            get { return this.ChildControl.TextImageRelation; }
            set { this.ChildControl.TextImageRelation = value; }
        }

        #endregion

        public new event EventHandler<AddRemoveButtonEventArgs> Click
        {
            add { this.ChildControl.Click += value; }
            remove { this.ChildControl.Click -= value; }
        }
    }
}
