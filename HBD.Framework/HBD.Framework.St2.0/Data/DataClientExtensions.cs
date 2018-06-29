#region using

using System;
using System.Data;

#endregion

namespace HBD.Framework.Data
{
    public static class DataClientExtensions
    {
        //public static Exception LastException { get; private set; }

        public static bool TryOpen(this IDbConnection @this, bool throwException = false)
        {
            if (@this == null) return false;
            if (@this.State == ConnectionState.Open) return true;
            try
            {
                @this.Open();
                return true;
            }
            catch (Exception)
            {
                if (throwException)
                    throw;

                return false;
            }
        }

        public static bool TryClose(this IDbConnection @this, bool throwException = false)
        {
            if (@this == null) return false;
            if (@this.State == ConnectionState.Closed) return true;
            try
            {
                @this.Close();
                return true;
            }
            catch (Exception)
            {
                if (throwException)
                    throw;

                return false;
            }
        }
    }
}