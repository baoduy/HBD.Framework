using System.Collections;
using System.Collections.Generic;

namespace HBD.Framework.Data.GetSetters
{
    public class ArrayGetSetterCollection : IGetSetterCollection
    {
        public ArrayGetSetterCollection(IGetSetter header, IList<IGetSetter> data)
            : this(string.Empty, header, data)
        { }
        public ArrayGetSetterCollection(string name, IGetSetter header, IList<IGetSetter> data)
        {
            Name = name;
            Header = header;
            Data = data;
        }

        public IGetSetter Header { get; }
        private IList<IGetSetter> Data { get; }
        public string Name { get; }

        public int Count => Data.Count;
        public IEnumerator<IGetSetter> GetEnumerator() => Data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
