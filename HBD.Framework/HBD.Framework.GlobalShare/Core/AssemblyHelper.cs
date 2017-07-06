#region using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#endregion

namespace HBD.Framework.Core
{

#if !NETSTANDARD1_6
    public static class AssemblyHelper
    {
        private static readonly IDictionary<string, WeakReference<Type>> TypeCacher =
            new Dictionary<string, WeakReference<Type>>();

        public static Assembly GetAssembly(string assemblyFileName)
        {
            if (assemblyFileName.IsNullOrEmpty()) return null;
            //Load From File
            Assembly assemble = null;
            try
            {
                assemble = assemblyFileName.EndsWith("dll")
                    ? Assembly.LoadFrom(assemblyFileName)
                    : Assembly.Load(assemblyFileName);
            }
            catch
            {
                if (assemblyFileName.EndsWith("dll")) return assemble;

                var fullPath = Path.GetFullPath($"{assemblyFileName}.dll");
                if (File.Exists(fullPath))
                    assemble = Assembly.LoadFrom(fullPath);
            }

            if (assemble != null) return assemble;

            try
            {
                return
                    AppDomain.CurrentDomain.GetAssemblies()
                        .FirstOrDefault(a => a.GetName().Name.EqualsIgnoreCase(assemblyFileName));
            }
            catch
            {
                // ignored
                return null;
            }
        }

        /// <summary>
        ///     Load Type from dll file.
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static Type GetTypeFromAssembly(AssemblyStringBuilder typeInfo)
        {
            Guard.ArgumentIsNotNull(typeInfo, nameof(typeInfo));
            var assembly = GetAssembly(typeInfo.AssemblyFileName);
            return assembly?.GetType(typeInfo.FullTypeName, false);
        }

        public static Type GetTypeInAllAssembliesFiles(string typeName)
            => (from dll in
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.AllDirectories)
                let assembly = GetAssembly(dll)
                let type = assembly?.GetType(typeName, false)
                where type != null
                select type).FirstOrDefault();

        /// <summary>
        ///     Get Type by TypeName. the format of type name "TypeName,AssemblyName" or "TypeName" is accepted.
        ///     Bellow search pattern will be executed.
        ///     1. Search Type in running assemblies.
        ///     2. Search in the file based on Assembly in the fullTypeName if pattern 1 failed.
        ///     3. Search in all dll files if pattern 2 failed.
        /// </summary>
        /// <param name="fullTypeName">The fullTypeName Format is "TypeName,AssemblyName"</param>
        /// <returns></returns>
        public static Type GetType(string fullTypeName)
        {
            Guard.ArgumentIsNotNull(fullTypeName, "FullTypeName");

            Type type = null;
            //Get from Cache
            if (TypeCacher.ContainsKey(fullTypeName))
                TypeCacher[fullTypeName].TryGetTarget(out type);

            //Search Type if cannot found in Cache.
            if (type == null)
                type = Type.GetType(fullTypeName)
                       ?? GetTypeFromAssembly(AssemblyStringBuilder.Parse(fullTypeName))
                       ?? GetTypeInAllAssembliesFiles(fullTypeName);

            //Save to Cache.
            if (type != null)
                TypeCacher[fullTypeName] = new WeakReference<Type>(type);

            return type;
        }

        public static Type[] GetTypes(string assemblyName)
        {
            var assembly = GetAssembly(assemblyName);
            return assembly?.GetTypes();
        }

        /// <summary>
        ///     Find all Types in current Domain that inherited from inheritedTypes.
        /// </summary>
        /// <returns></returns>
        public static TypeFinder FindAllTypes() => new TypeFinder();

        /// <summary>
        ///     Find all Types in current Domain that inherited from T.
        /// </summary>
        /// <typeparam name="T">inherited type</typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> FindAllTypes<T>() => FindAllTypes().InheritedFrom(typeof(T));
    }

#endif
}