using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HashCode.MailMessageActivities
{
    public class GetHtmlBody : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<MailMessage> MailMessage { get; set; }

        [Category("Input")]
        public bool TrimHtml { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<string> HtmlBody { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (context == null) return;

            var mailMessage = this.MailMessage.Get(context);
            bool trimHtml = this.TrimHtml;

            if (mailMessage == null)
            {
                Console.WriteLine("Input Mail Message is NULL");
            }
            else
            {
                try
                {
                    string htmlBody = ExportMailToHtml(mailMessage);
                    if(trimHtml)
                    {
                        string trimmedHtmlBody = GetTrimmedBodyText(htmlBody);
                        this.HtmlBody.Set(context, trimmedHtmlBody);
                    }
                    else
                    {
                        this.HtmlBody.Set(context, htmlBody);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception has been caught::" + e.ToString());
                }
            }
        }

        private string ExportMailToHtml(MailMessage mail)
        {
            string Htmltext = string.Empty;
            foreach (AlternateView altView in mail.AlternateViews)
            {
                Htmltext += ExtractAlternateView(altView);
            }
            return Htmltext;
        }

        private string ExtractAlternateView(AlternateView altView)
        {
            var dataStream = altView.ContentStream;
            byte[] byteBuffer = new byte[dataStream.Length];
            return System.Text.Encoding.ASCII.GetString(byteBuffer, 0, dataStream.Read(byteBuffer, 0, byteBuffer.Length));
        }

        private string GetTrimmedBodyText(string HTMLBody)
        {
            string trimmedHTMLBody = HTMLBody;
            int start = 0;
            start = HTMLBody.IndexOf("<img", start);
            while (start > 0)
            {
                int count = HTMLBody.IndexOf('>', start);
                string str = HTMLBody.Substring(start);
                string actualImgTag = str.Substring(0, str.IndexOf(">") + 1);
                string trimImgTag = actualImgTag.Replace("cid:", "");
                int intAtPosition = 0;
                intAtPosition = trimImgTag.IndexOf("@");
                while (intAtPosition > 0)
                {
                    string strAt = trimImgTag.Substring(trimImgTag.IndexOf("@"), 18);
                    trimImgTag = trimImgTag.Replace(strAt, "");
                    intAtPosition = trimImgTag.IndexOf("@");
                }
                trimmedHTMLBody = trimmedHTMLBody.Replace(actualImgTag, trimImgTag);
                start = HTMLBody.IndexOf("<img", start + 1);
            }
            return trimmedHTMLBody;
        }
    }
}