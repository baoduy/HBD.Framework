using HBD.Framework.Configuration;
using HBD.Libraries.Net.Email.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;
using HBD.Framework.Core;
using HBD.Framework.Log;

namespace HBD.Libraries.Net.Email
{
    public static class EmailManager
    {
        const string _defaultEmailNotDefine = "The DefaultTemplate property is empty and more than 1 email in EmailTemplates collection.";
        const string _defaultEmailNotFound = "The DefaultTemplate {0} is not found";
        const string _templateNotFound = "The Email Template {0} is not found";
        const string _emailTemplateSectionNotFound = "The HBD.Configuration.EmailCollectionSection is not found in Config file.";
        const string _emailTemplateCollectionEmpty = "The EmailCollection is empty.";

        static EmailCollectionSection _emailTemplates = null;
        public static EmailCollectionSection EmailTemplates
        {
            get
            {
                if (EmailManager._emailTemplates == null)
                    _emailTemplates = HBD.Framework.Configuration.ConfigurationManager.GetSection<EmailCollectionSection>();
                return _emailTemplates;
            }
        }

        public static bool IsEmail(string emailAddress)
        {
            try
            {
                return Regex.IsMatch(emailAddress,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static void AddEmail(string emailAddresses, MailAddressCollection collection)
        {
            if (string.IsNullOrEmpty(emailAddresses))
                return;

            foreach (var a in emailAddresses.Split(';', ','))
            {
                if (IsEmail(a))
                    collection.Add(a);
                else
                {
                    //[Todo] Write Log
                }
            }
        }

        private static void VerifyEmailTemplates()
        {
            if (EmailTemplates == null)
                throw new Exception(_emailTemplateSectionNotFound);
            if (EmailTemplates.EmailTemplates.Count == 0)
                throw new Exception(_emailTemplateCollectionEmpty);
        }
        /// <summary>
        /// Send email to all configed templates
        /// </summary>
        public static void SendAll(params string[] attachments)
        {
            VerifyEmailTemplates();
            foreach (EmailTemplateElement template in EmailTemplates.EmailTemplates)
                EmailManager.Send(template, attachments);
        }

        /// <summary>
        /// Send email to default template
        /// </summary>
        public static void Send(params string[] attachments)
        {
            VerifyEmailTemplates();
            var emails = EmailManager.EmailTemplates;
            EmailTemplateElement template = null;

            if (string.IsNullOrEmpty(emails.DefaultTemplate))
            {
                if (emails.EmailTemplates.Count == 1)
                    template = emails.EmailTemplates[0];
                else throw new ArgumentException(_defaultEmailNotDefine);
            }
            else
            {
                template = emails.EmailTemplates.Cast<EmailTemplateElement>().FirstOrDefault(e => e.Name == emails.DefaultTemplate);
                if (template == null)
                    throw new ArgumentException(string.Format(_defaultEmailNotFound, emails.DefaultTemplate));
            }

            EmailManager.Send(template, attachments);
        }

        public static void Send(string emailTemplateName, params string[] attachments)
        {
            VerifyEmailTemplates();
            var emailTemplate = EmailTemplates.EmailTemplates[emailTemplateName];
            if (emailTemplate == null)
                throw new ArgumentException(string.Format(_templateNotFound, emailTemplateName));

            EmailManager.Send(emailTemplate, attachments);
        }

        public static void Send(int emailTemplateIndex, params string[] attachments)
        {
            VerifyEmailTemplates();
            var emailTemplate = EmailTemplates.EmailTemplates[emailTemplateIndex];
            if (emailTemplate == null)
                throw new ArgumentException(string.Format(_templateNotFound, emailTemplateIndex));

            EmailManager.Send(emailTemplate, attachments);
        }

        /// <summary>
        /// Send email to special template
        /// </summary>
        /// <param name="emailTemplate"></param>
        public static void Send(EmailTemplateElement emailTemplate, params string[] attachments)
        {
            Guard.ArgumentNotNull(emailTemplate, "emailTemplate");

            var mailSetting = HBD.Framework.Configuration.ConfigurationManager.OpenConfiguration().GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            try
            {
                var mail = new System.Net.Mail.MailMessage()
                {
                    Subject = emailTemplate.Subject,
                    Body = emailTemplate.Body,
                    IsBodyHtml = emailTemplate.IsBodyHtml
                };

                AddEmail(emailTemplate.EmailTo, mail.To);
                AddEmail(emailTemplate.CcTo, mail.CC);
                AddEmail(emailTemplate.BccTo, mail.Bcc);

                if (attachments != null)
                {
                    foreach (var f in attachments)
                        if (System.IO.File.Exists(f))
                            mail.Attachments.Add(new Attachment(f));
                        else LogManager.WriteError(string.Format("File '{}' not found", f));
                }

                ApplyArgumentFields(mail);

                new SmtpClient().Send(mail);
            }
            catch (Exception ex)
            { throw ex; }
        }

        private static void ApplyArgumentFields(MailMessage email)
        {
            var currentCulture = HBD.Framework.Configuration.ConfigurationManager.GetDefaultCulture();

            email.Subject = email.Subject.Replace(Constants.ArgumentFields.Today, DateTime.Today.ToString(currentCulture.DateTimeFormat.ShortDatePattern));
            email.Subject = email.Subject.Replace(Constants.ArgumentFields.Now, DateTime.Now.ToString(currentCulture.DateTimeFormat.LongDatePattern));

            email.Body = email.Body.Replace(Constants.ArgumentFields.Today, DateTime.Today.ToString(currentCulture.DateTimeFormat.ShortDatePattern));
            email.Body = email.Body.Replace(Constants.ArgumentFields.Now, DateTime.Now.ToString(currentCulture.DateTimeFormat.LongDatePattern));
        }
    }
}
