using Mailer.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Mailer.Service
{
    public class EmailService
    {
        private const string FooterPlaceholderHtml = "FOOTER_HTML";
        private const string HeaderPlaceholderHtml = "HEADER_HTML";
        private const string BodyPlaceholderHtml = "BODY_HTML";
        private const string FooterPlaceholderTxt = "FOOTER_TXT";

        private static string GetValueByKeyFromConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void Notify(ItemModel obj)
        {
            var defaultCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("uk-UA");
            var subject = "На Bitmain нові надходження";
            if (CultureInfo.CurrentCulture.Name == "ru-RU")
            {
                subject = "На Bitmain новые поступления";
                defaultCulture = CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU");
            }

            var replacements = new Dictionary<string, string>
            {
                { "Link", obj.Link },
                { "Name", obj.Name},
                { "Price", obj.Price },
                { "Subject", subject }
            };

            // Generate the HTML based view            
            var htmlBodyTemplate = GetPatchedMailTemplateString("notify", defaultCulture, true, replacements);

            // Generate the plain text view
            var txtTemplate = GetPatchedMailTemplateString("notify", defaultCulture, false, replacements);
            using (var sr = new StreamReader(@"mails.txt"))
            {
                while (!sr.EndOfStream)
                {  
                    GetTemplateMessage(subject, sr.ReadLine(), htmlBodyTemplate, txtTemplate, defaultCulture);
                }
            }
        }

        private static void GetTemplateMessage(string subject, string email, string htmlBody, string txtBody, CultureInfo mailCulture)
        {

            var client = new SendGridClient(GetValueByKeyFromConfig("SendGrid"));
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("mailer@golovatenko.com", "Головатенко Илья, ILGOL"));

            msg.AddTo(email);

            msg.SetSubject(subject);

            // Generate the HTML based view            
            if (!String.IsNullOrEmpty(htmlBody))
            {
                var htmlReplacements = new Dictionary<string, string>();
                htmlReplacements.Add(BodyPlaceholderHtml, htmlBody);
                var htmlMasterTemplate = GetMasterMailTemplateString("master-template", true, htmlReplacements);
                msg.AddContent(MimeType.Html, htmlMasterTemplate);
            }
            var result = client.SendEmailAsync(msg);
            result.Wait();

        }

        private static string GetPatchedMailTemplateString(string templateName, CultureInfo cultInfo, bool htmlTemplate, Dictionary<string, string> replaceMents)
        {
            // load the tempalte
            var template = GetMailTemplateString(templateName, cultInfo, htmlTemplate);
            if (String.IsNullOrEmpty(template))
                return template;

            // do the replacements
            foreach (var kvp in replaceMents)
            {
                var searchKey = String.Format("{{{0}}}", kvp.Key);
                template = template.Replace(searchKey, kvp.Value);
            }

            // done 
            return template;
        }

        private static string GetMailTemplateString(string templateName, CultureInfo cultInfo, bool htmlTemplate)
        {
            // build the path 
            var templatePath = String.Format("./Templates/EmailTemplates/{0}/{1}.{2}", cultInfo.Name, templateName, htmlTemplate ? "html" : "txt");

            // try to load this one
            var template = GetTextFromFile(templatePath);

            if (!String.IsNullOrEmpty(template))
                return template;

            templatePath = String.Format("./Templates/EmailTemplates/uk-UA/.{0}.{1}", templateName, htmlTemplate ? "html" : "txt");
            return GetTextFromFile(templatePath);
        }

        private static string GetMasterMailTemplateString(string templateName, bool htmlTemplate, Dictionary<string, string> replacements)
        {
            // Matrix42.ACS.Core.MailIntegration.Templates.en_US.signup-template.html

            // build the path 
            var templatePath = String.Format("./Templates/EmailTemplates/{0}.{1}", templateName, htmlTemplate ? "html" : "txt");

            // try to load this one
            var template = GetTextFromFile(templatePath);

            if (String.IsNullOrEmpty(template))
                return template;

            // do the replacements
            foreach (var kvp in replacements)
            {
                var searchKey = String.Format("{{{0}}}", kvp.Key);
                template = template.Replace(searchKey, kvp.Value);
            }

            // done 
            return template;
        }

        private static string GetTextFromFile(string resourceName)
        {
            try
            {
                var stream = File.OpenText(resourceName);

                using (stream)
                {
                    return stream.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }

        private static string Capitalize(string str)
        {
            return !String.IsNullOrEmpty(str) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str) : String.Empty;
        }
    }
}