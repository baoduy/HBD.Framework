#region

using System.Collections.Generic;
using System.Text;

#endregion

namespace HBD.Framework.Data.HtmlGeneration
{
    public class StyleCollection : Dictionary<string, string>
    {
        public void Add(StyleNames name, string value)
            => Add(name.ToStyleName(), value);

        public bool Remove(StyleNames name)
            => Remove(name.ToStyleName());

        public bool TryGetValue(StyleNames name, out string value)
            => base.TryGetValue(name.ToStyleName(), out value);

        public string this[StyleNames name] => this[name.ToStyleName()];

        public virtual bool ContainsKey(StyleNames name) => this.ContainsKey(name.ToStyleName());

        public override string ToString()
        {
            var builder = new StringBuilder();
            Generate(builder);
            return builder.ToString();
        }

        public virtual void Generate(StringBuilder builder)
        {
            foreach (var v in this)
                builder.AppendFormat("{0}:{1};", v.Key, v.Value);
        }
    }
}