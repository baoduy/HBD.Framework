using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HBD.Framework.Core
{
    public static class Guard
    {
        public static void ArgumentNotNull( object obj, string name )
        {
            bool isNull = false;
            if ( obj is string )
                isNull = string.IsNullOrEmpty( obj as string );
            else isNull = obj == null || obj == DBNull.Value;

            if ( isNull )
                throw new ArgumentNullException( name );
        }

        public static void ArgumentIs<T>( object obj, string name )
        {
            ArgumentIs( obj, typeof( T ), name );
        }

        public static void ArgumentIs( object obj, Type expectedType, string name )
        {
            Guard.ArgumentNotNull( obj, name );
            if ( !expectedType.IsAssignableFrom( obj.GetType() ) )
                throw new ArgumentException( string.Format( "{0} must be an instance of {1}", name, expectedType.FullName ) );
        }

        public static void GreaterThanOrEqualsZero( int value, string name )
        {
            if ( value <= 0 )
                throw new ArgumentNullException( name );
        }

        public static void PathExisted( string path )
        {
            Guard.ArgumentNotNull( path, "path" );

            if ( !PathExtension.IsPathExisted( path ) )
                throw new PathNotFoundException( path );
        }

        public static void MustBeValueType( object obj, string name )
        {
            if ( obj != null && !( obj.GetType().IsValueType || obj is string ) )
                throw new ArgumentException( string.Format( "{0} must be Value Type", name ) );
        }

        public static void MustBeValuesType( IEnumerable<object> collection )
        {
            Guard.ArgumentNotNull( collection, "Collection" );
            foreach ( var obj in collection )
            {
                if ( obj != null && !( obj.GetType().IsValueType || obj is string ) )
                    throw new ArgumentException( string.Format( "Object in collection must be Value Type" ) );
            }
        }
    }
}
