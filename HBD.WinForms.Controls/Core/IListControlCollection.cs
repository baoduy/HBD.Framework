using System;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using HBD.WinForms.Controls.Events;
namespace HBD.WinForms.Controls.Core
{
    public interface IListControlCollection : IHBDControl
    {
        bool AllowAddControl { get; set; }
        bool AllowRemoveControl { get; set; }
        Type ChildrenControlType { get; set; }
        ReadOnlyCollection<Control> ChildrenControls { get; }

        Control AddNewControl();
        void Clear();
        void RemoveControl();

        event EventHandler<ListControlCollectionEventArgs> ChildrenControlAdded;
        event EventHandler<ListControlCollectionEventArgs> ChildrenControlRemoved;
        event EventHandler AllowancePropertiesChanged;
    }
}
