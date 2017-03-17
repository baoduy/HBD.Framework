#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using HBD.Framework.EventArguments;

#endregion

namespace HBD.Framework.Core
{
    /// <summary>
    ///     The implementation of INotifyPropertyChanged and  ICancelableNotifyPropertyChanging with some useful feature.
    /// </summary>
    public abstract class NotifyPropertyChange : ICancelableNotifyPropertyChanging, INotifyPropertyChanged,
        IInternalNotifyPropertyChanging, IInternalNotifyPropertyChanged
    {
        /// <summary>
        ///     This flag to allow to disable changes event temporaries.
        /// </summary>
        internal int NotifyPropertyChangedCount { get; set; }

        internal int NotifyPropertyChangingCount { get; set; }

        internal IList<IDisposable> DisableNotifyPropertyChangeObjects { get; } = new List<IDisposable>();

        protected void SetValue<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(member, value)) return;

            if (RaisePropertyChanging(propertyName, member, value).Cancel) return;

            member = value;
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(propertyName);
        }

        protected virtual string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
            => propertyExpression.ExtractPropertyName();

        /// <summary>
        ///     Disable the NotifyPropertyChanged event
        /// </summary>
        /// <returns></returns>
        public virtual IDisposable DisablePropertyChangedNotify()
        {
            var item = new DisableNotifyPropertyChanged(this);
            DisableNotifyPropertyChangeObjects.Add(item);
            return item;
        }

        /// <summary>
        ///     Disable the NotifyPropertyChanging event
        /// </summary>
        /// <returns></returns>
        public virtual IDisposable DisablePropertyChangingNotify()
        {
            var item = new DisableNotifyPropertyChanging(this);
            DisableNotifyPropertyChangeObjects.Add(item);
            return item;
        }

        /// <summary>
        ///     Disable both NotifyPropertyChanging and NotifyPropertyChanged events
        /// </summary>
        /// <returns></returns>
        public virtual IDisposable DisablePropertyChangeNotifies()
        {
            var item = new DisableNotifyPropertyChanges(this);
            DisableNotifyPropertyChangeObjects.Add(item);
            return item;
        }

        internal abstract class DisableNotifyProperty : IDisposable
        {
            protected DisableNotifyProperty(NotifyPropertyChange notifyPropertyChange)
            {
                NotifyPropertyChange = notifyPropertyChange;
            }

            protected NotifyPropertyChange NotifyPropertyChange { get; }

            public virtual void Dispose() => NotifyPropertyChange.DisableNotifyPropertyChangeObjects.Remove(this);
        }


        /// <summary>
        ///     Disable the NotifyPropertyChanged event
        /// </summary>
        internal class DisableNotifyPropertyChanged : DisableNotifyProperty
        {
            public DisableNotifyPropertyChanged(NotifyPropertyChange notifyPropertyChange)
                : base(notifyPropertyChange)
            {
                notifyPropertyChange.NotifyPropertyChangedCount += 1;
            }

            public override void Dispose()
            {
                NotifyPropertyChange.NotifyPropertyChangedCount -= 1;
                base.Dispose();
            }
        }

        /// <summary>
        ///     Disable the NotifyPropertyChanging event
        /// </summary>
        internal class DisableNotifyPropertyChanging : DisableNotifyProperty
        {
            public DisableNotifyPropertyChanging(NotifyPropertyChange notifyPropertyChange)
                : base(notifyPropertyChange)
            {
                notifyPropertyChange.NotifyPropertyChangingCount += 1;
            }

            public override void Dispose()
            {
                NotifyPropertyChange.NotifyPropertyChangingCount -= 1;
                base.Dispose();
            }
        }

        /// <summary>
        ///     Disable both NotifyPropertyChanging and NotifyPropertyChanged events
        /// </summary>
        internal class DisableNotifyPropertyChanges : DisableNotifyProperty
        {
            public DisableNotifyPropertyChanges(NotifyPropertyChange notifyPropertyChange)
                : base(notifyPropertyChange)
            {
                notifyPropertyChange.NotifyPropertyChangingCount += 1;
                notifyPropertyChange.NotifyPropertyChangedCount += 1;
            }

            public override void Dispose()
            {
                NotifyPropertyChange.NotifyPropertyChangingCount -= 1;
                NotifyPropertyChange.NotifyPropertyChangedCount -= 1;
                base.Dispose();
            }
        }

        #region INotifyPropertyChanging

        public event EventHandler<CancelablePropertyChangingEventArgs> PropertyChanging;
        public event EventHandler<CancelablePropertyChangingEventArgs> InternalPropertyChanging;

        protected virtual void OnPropertyChanging(CancelablePropertyChangingEventArgs e)
        {
            InternalPropertyChanging?.Invoke(this, e);
            if (NotifyPropertyChangingCount > 0) return;
            PropertyChanging?.Invoke(this, e);
        }

        protected CancelablePropertyChangingEventArgs RaisePropertyChanging(string propertyName, object oldValue,
            object newValue)
        {
            var rs = new CancelablePropertyChangingEventArgs(propertyName, oldValue, newValue);
            OnPropertyChanging(rs);
            return rs;
        }

        protected CancelablePropertyChangingEventArgs RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression,
            object newValue)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            var oldValue = propertyExpression.Compile().Invoke();
            return RaisePropertyChanging(ExtractPropertyName(propertyExpression), oldValue, newValue);
        }

        #endregion INotifyPropertyChanging

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandler InternalPropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            InternalPropertyChanged?.Invoke(this, e);
            if (NotifyPropertyChangedCount > 0) return;
            PropertyChanged?.Invoke(this, e);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
            // ReSharper disable once ExplicitCallerInfoArgument
            => RaisePropertyChanged(ExtractPropertyName(propertyExpression));

        #endregion INotifyPropertyChanged
    }
}