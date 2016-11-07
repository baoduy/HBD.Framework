using System;

namespace HBD.Framework.Core
{
    public class AssemblyStringBuilder
    {
        protected AssemblyStringBuilder()
        {
        }

        public string FullTypeName { get; set; }
        public string AssemblyFileName { get; set; }

        public static AssemblyStringBuilder Parse(string fullTypeAndAsemblyName)
        {
            Guard.ArgumentIsNotNull(fullTypeAndAsemblyName, "FullTypeAndAsemblyName");
            var errorString = $"The full Type and Assembly Name of '{fullTypeAndAsemblyName}' is not correct.";

            string fullTypeName;
            var assemblyFileName = string.Empty;

            //If fullTypeAndAsemblyName contains TypeName and AssemblyName.
            if (fullTypeAndAsemblyName.Contains(","))
            {
                var array = fullTypeAndAsemblyName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length != 2)
                    throw new ArgumentException(errorString);

                fullTypeName = array[0];
                assemblyFileName = array[1];
            }
            //fullTypeAndAsemblyName contains TypeName only.
            else fullTypeName = fullTypeAndAsemblyName;

            return new AssemblyStringBuilder { FullTypeName = fullTypeName, AssemblyFileName = assemblyFileName };
        }

        public static implicit operator string(AssemblyStringBuilder value)
            => $"{value.FullTypeName},{value.AssemblyFileName}";

        public static implicit operator AssemblyStringBuilder(string value) => Parse(value);
    }
}