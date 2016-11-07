using System.ComponentModel;

namespace HBD.Framework.Core
{
    public interface IUnChangableTracking : IChangeTracking
    {
        /// <summary>
        /// Undo the changes of entity.
        /// </summary>
        void UndoChanges();
    }
}