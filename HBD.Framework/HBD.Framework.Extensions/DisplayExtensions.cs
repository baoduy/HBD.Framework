using Pluralize.NET.Core;
using System;
using System.ComponentModel;
using System.Reflection;

namespace HBD.Framework.Extensions
{
    public static class DisplayExtensions
    {
        #region Public Methods

        public static string GetDisplayName<TEntityOrDto>() => typeof(TEntityOrDto).GetDisplayName();

        public static string GetDisplayName(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? type.Name.ToDisplayWords();
        }

        public static string Pluralize(this string word) => new Pluralizer().Pluralize(word);

        public static string Singularize(this string word) => new Pluralizer().Singularize(word);

        #endregion Public Methods
    }
}