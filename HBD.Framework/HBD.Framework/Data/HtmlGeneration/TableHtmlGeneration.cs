#region

using HBD.Framework.Core;
using HBD.Framework.Data.GetSetters;
using System;
using System.Linq;
using System.Text;

#endregion

namespace HBD.Framework.Data.HtmlGeneration
{
    public class TableHtmlGeneration : HtmlGenerationBase
    {
        private readonly IGetSetterCollection _data;

        public TableHtmlGeneration(IGetSetterCollection data)
        {
            Guard.ArgumentIsNotNull(data, nameof(data));
            _data = data;
        }

        public bool ApplyDefaultStyleIfEmpty { get; set; } = true;

        public override string Generate()
        {
            if (ApplyDefaultStyleIfEmpty)
                InitialDefaultStyle();

            var tableStyle = TableStyle.ToString();
            var headerStyle = HeaderStyle.ToString();
            var rowStyle = RowStyle.ToString();
            var oddRowStyle = AlternativeRowStyle.ToString();
            var footerStyle = FooterStyle.ToString();

            if (oddRowStyle.IsNullOrEmpty())
                oddRowStyle = rowStyle;

            var rows = _data.ToList();

            Func<int, bool> isFooterIndex = index => index == rows.Count - 1;
            Func<int, bool> isOddRowIndex = index => index % 2 != 0;
            Func<int, string> getStyle = index =>
            {
                if (isFooterIndex(index) && footerStyle.IsNotNullOrEmpty())
                    return footerStyle;
                return isOddRowIndex(index) ? oddRowStyle : rowStyle;
            };

            var builder = new StringBuilder();
            builder.AppendFormat("<table style='{0}'>", tableStyle);

            //Header
            builder.Append("<thead>");
            builder.Append("<tr>");

            foreach (var val in _data.Header)
                builder.AppendFormat("<th style='{0}'>{1}</th>", headerStyle, val);

            builder.Append("</tr>");
            builder.Append("</thead>");

            for (var i = 0; i < rows.Count; i++)
            {
                var getter = rows[i];

                builder.Append(isFooterIndex(i) ? "<tfoot>" : "<tbody>");
                builder.Append("<tr>");

                foreach (var g in getter)
                    builder.AppendFormat("<td style='{0}'>{1}</td>", getStyle(i), g);

                builder.Append("</tr>");
                builder.Append(isFooterIndex(i) ? "</tfoot>" : "</tbody>");
            }

            builder.Append("</table>");
            return builder.ToString();
        }

        protected virtual void InitialDefaultStyle()
        {
            if (TableStyle.Count <= 0)
            {
                TableStyle.Add(StyleNames.Border, "1px solid black");
                TableStyle.Add(StyleNames.BorderCollapse, "collapse"); //separate
                TableStyle.Add(StyleNames.BorderSpacing, "0");
                TableStyle.Add(StyleNames.Width, "100%");
            }

            if (HeaderStyle.Count == 0)
            {
                HeaderStyle.Add(StyleNames.Border, "1px solid black");
                HeaderStyle.Add(StyleNames.BorderCollapse, "collapse"); //separate
                HeaderStyle.Add(StyleNames.BorderSpacing, "0");
                HeaderStyle.Add(StyleNames.Padding, "5px 5px 5px 5px");
            }

            if (RowStyle.Count == 0)
            {
                RowStyle.Add(StyleNames.Border, "1px solid black");
                RowStyle.Add(StyleNames.BorderCollapse, "collapse"); //separate
                RowStyle.Add(StyleNames.BorderSpacing, "0");
                RowStyle.Add(StyleNames.Padding, "5px 5px 5px 5px");
                RowStyle.Add(StyleNames.BackgroundColor, "#F0F0F0");
            }

            if (AlternativeRowStyle.Count == 0)
            {
                AlternativeRowStyle.Add(StyleNames.Border, "1px solid black");
                AlternativeRowStyle.Add(StyleNames.BorderCollapse, "collapse"); //separate
                AlternativeRowStyle.Add(StyleNames.BorderSpacing, "0");
                AlternativeRowStyle.Add(StyleNames.Padding, "5px 5px 5px 5px");
            }
        }
    }
}