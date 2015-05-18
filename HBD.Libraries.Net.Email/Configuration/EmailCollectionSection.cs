using HBD.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace HBD.Libraries.Net.Email.Configuration
{
    [ConfigurationCollection(typeof(EmailTemplateCollectionElement),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
    public class EmailCollectionSection : ConfigurationSectionBase
    {
        const string _defaultTemplate = "defaultTemplate";
        const string _emailTemplates = "emailTemplates";

        public override string SectionName
        {
            get { return "HBD.Configuration.EmailCollectionSection"; }
        }

        [ConfigurationProperty(_defaultTemplate, IsRequired = false)]
        public string DefaultTemplate
        { get { return this[_defaultTemplate] as string; } }

        [ConfigurationProperty(_emailTemplates, IsRequired = false, IsDefaultCollection=true)]
        public EmailTemplateCollectionElement EmailTemplates
        { get { return this[_emailTemplates] as EmailTemplateCollectionElement; } }
    }
}
