using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HBD.Framework.Core
{
    public static class AssemblyExtension
    {
        public static Type GetType( string typeFullName )
        {
            Guard.ArgumentNotNull( typeFullName, "TypeFullName" );

            var type = Type.GetType( typeFullName );
            if ( type != null )
                return type;
            if ( !typeFullName.Contains( "," ) )
                return null;

            var array = typeFullName.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
            var typeName = array[0];
            var dllName = array[1];

            //Load From File
            Assembly assemble = null;

            try
            {
                assemble = Assembly.Load( dllName );
            }
            catch
            {
                try
                {
                    var fullPath = PathExtension.GetFullPath( string.Format( "{0}.dll", dllName ) );
                    assemble = Assembly.LoadFile( fullPath );
                }
                catch ( Exception ex )
                {
                    throw ex;
                }
            }

            if ( assemble != null )
                type = assemble.GetType( typeName, true );

            return type;
        }
    }
}
