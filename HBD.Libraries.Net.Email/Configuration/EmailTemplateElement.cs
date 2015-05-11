using HBD.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace HBD.Libraries.Net.Email.Configuration
{
    public class EmailTemplateElement : ConfigurationElementBase
    {
        const string _emailTo = "emailTo";
        const string _ccTo = "ccTo";
        const string _bccTo = "bccTo";
        const string _subject = "subject";
        const string _body = "body";
        const string _isBodyHtml = "isBodyHtml";

        [ConfigurationProperty(_emailTo, IsRequired = true)]
        public string EmailTo
        { get { return this[_emailTo] as string; } }

        [ConfigurationProperty(_ccTo, IsRequired = false)]
        public string CcTo
        { get { return this[_ccTo] as string; } }

        [ConfigurationProperty(_bccTo, IsRequired = false)]
        public string BccTo
        { get { return this[_bccTo] as string; } }

        [ConfigurationProperty(_subject, IsRequired = true)]
        public string Subject
        { get { return this[_subject] as string; } }

        [ConfigurationProperty(_body, IsRequired = false)]
        public string Body
        { get { return this[_body] as string; } }

        [ConfigurationProperty(_isBodyHtml, IsRequired = false)]
        public bool IsBodyHtml
        {
            get
            {
                var s = this[_isBodyHtml] as string;
                if (string.IsNullOrEmpty(s))
                    return false;
                return s.Equals(bool.TrueString, StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}
