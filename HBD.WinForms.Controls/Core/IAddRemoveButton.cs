using System;
namespace HBD.WinForms.Controls.Core
{
    interface IAddRemoveButton : IHBDControl
    {
        System.Drawing.Image AddImage { get; set; }
        int AddImageIndex { get; set; }
        string AddImageKey { get; set; }
        string AddText { get; set; }
        bool AutoEllipsis { get; set; }
        event EventHandler<AddRemoveButtonEventArgs> Click;
        HBD.WinForms.Controls.Core.IListControlCollection CollectionControl { get; set; }
        System.Windows.Forms.FlatStyle FlatStyle { get; set; }
        System.Drawing.ContentAlignment ImageAlign { get; set; }
        System.Windows.Forms.ImageList ImageList { get; set; }
        System.Drawing.Image RemoveImage { get; set; }
        int RemoveImageIndex { get; set; }
        string RemoveImageKey { get; set; }
        string RemoveText { get; set; }
        System.Drawing.ContentAlignment TextAlign { get; set; }
        System.Windows.Forms.TextImageRelation TextImageRelation { get; set; }
    }
}
