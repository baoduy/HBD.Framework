using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

// ReSharper disable MemberCanBePrivate.Global

namespace HBD.Framework.Extensions
{
    public static class EnumExtensions
    {
        #region Public Methods

        public static T GetAttribute<T>(this Enum @this) where T : Attribute
        {
            var type = @this.GetType();
            var mem = type.GetMember(@this.ToString())[0];
            return mem.GetCustomAttribute<T>();
        }

        public static EnumInfo GetEumInfo(this Enum @this)
        {
            var att = @this.GetAttribute<DisplayAttribute>();

            return new EnumInfo
            {
                Key = @this.ToString(),
                Description = att?.Description,
                Name = att?.Name,
                GroupName = att?.GroupName
            };
        }

        public static IEnumerable<EnumInfo> GetEumInfos<T>() where T : Enum
        {
            var type = typeof(T);
            var members = type.GetFields();

            foreach (var info in members)
            {
                if (info.FieldType == typeof(int)) continue;

                var att = info.GetCustomAttribute<DisplayAttribute>();

                yield return new EnumInfo
                {
                    Key = info.Name,
                    Description = att?.Description,
                    Name = att?.Name ?? info.Name,
                    GroupName = att?.GroupName
                };
            }
        }

        #endregion Public Methods
    }
}