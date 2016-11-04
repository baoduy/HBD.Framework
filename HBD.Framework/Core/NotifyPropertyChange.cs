using HBD.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace HBD.Framework.Core
{
    public abstract class NotifyPropertyChange : INotifyPropertyChanging, INotifyPropertyChanged
    {
        // ReSharper disable once RedundantAssignment
        protected void SetValue<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(member, value)) return;

            RaisePropertyChanging(propertyName);

            member = value;
            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanging(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual void RaisePropertyChanging(string propertyName)
             => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        protected void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
            // ReSharper disable once ExplicitCallerInfoArgument
            => RaisePropertyChanging(ExtractPropertyName(propertyExpression));

        #endregion INotifyPropertyChanging

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            => OnPropertyChanged(propertyName);

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
            // ReSharper disable once ExplicitCallerInfoArgument
            => RaisePropertyChanged(ExtractPropertyName(propertyExpression));

        #endregion INotifyPropertyChanged

        protected virtual string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
            => propertyExpression.ExtractPropertyName();
    }
}