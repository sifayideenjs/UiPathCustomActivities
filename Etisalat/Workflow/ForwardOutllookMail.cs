using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using Etisalat.CIT.OPS.Robotics;

namespace Etisalat.CIT.OPS.Robotics.Outlook
{
    public class ForwardOutllookMail : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<MailMessage> MailMessage { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> To { get; set; }

        [Category("Input")]
        public InArgument<string> CC { get; set; }

        [Category("Input")]
        public InArgument<string> BCC { get; set; }

        [Category("Optional")]
        public bool ShowOriginalMessage { get; set; }

        [Category("Optional")]
        public InArgument<string> ForwardMessage { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<MailMessage> ForwardMail { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            MailMessage forwardMail = MailMessage.Get(context);
            if (forwardMail != null)
            {
                string html = ExportMailToHtml(forwardMail);

                string strHeader = "<p>";

                string fMessage = ForwardMessage.Get(context);
                if (!string.IsNullOrEmpty(fMessage))
                {
                    fMessage = TextToHtml(fMessage);
                    strHeader = fMessage + "<br>";
                }

                if (ShowOriginalMessage)
                {
                    strHeader = strHeader + "<br><br>" + "-----Original Message-----" + "<br>";
                    strHeader += "From: " + forwardMail.From.DisplayName + "<br>";
                    strHeader += "Sent: " + forwardMail.Headers["Date"] + "<br>";
                    strHeader += "To: " + forwardMail.To + "<br>";
                    strHeader += "Subject: " + forwardMail.Subject + "<br><br>";
                }

                forwardMail.Subject = "FW: " + forwardMail.Subject;
                forwardMail.IsBodyHtml = true;
                forwardMail.Body = strHeader + GetTrimmedBodyText(html) + "</p>";

                string to = To.Get(context);
                if (!string.IsNullOrEmpty(to))
                {
                    to = to.Replace(",", ";");
                    var toSplits = to.Split(';');
                    foreach (var t in toSplits)
                    {
                        forwardMail.To.Add(new MailAddress(t));
                    }
                }

                string cc = CC.Get(context);
                if (!string.IsNullOrEmpty(cc))
                {
                    cc = cc.Replace(",", ";");
                    var ccSplits = cc.Split(';');
                    foreach (var cs in ccSplits)
                    {
                        forwardMail.CC.Add(new MailAddress(cs));
                    }
                }

                string bcc = BCC.Get(context);
                if (!string.IsNullOrEmpty(bcc))
                {
                    bcc = bcc.Replace(",", ";");
                    var bccSplits = bcc.Split(';');
                    foreach (var b in bccSplits)
                    {
                        forwardMail.Bcc.Add(new MailAddress(b));
                    }
                }

                this.ForwardMail.Set(context, forwardMail);
            }
            else
            {
                Console.WriteLine("No Mail Message Found");
            }
        }

        private string GetTrimmedBodyText(string strHTMLBody)
        {
            string strTrimmedHTMLBody = strHTMLBody;
            int start = 0;
            start = strHTMLBody.IndexOf("<img", start);
            while (start > 0)
            {
                int count = strHTMLBody.IndexOf('>', start);
                string str = strHTMLBody.Substring(start);
                string strActualImgTag = str.Substring(0, str.IndexOf(">") + 1);
                string strTrimImgTag = strActualImgTag.Replace("cid:", "");
                int intAtPosition = 0;
                intAtPosition = strTrimImgTag.IndexOf("@");
                while (intAtPosition > 0)
                {
                    string strAt = strTrimImgTag.Substring(strTrimImgTag.IndexOf("@"), 18);
                    strTrimImgTag = strTrimImgTag.Replace(strAt, "");
                    intAtPosition = strTrimImgTag.IndexOf("@");
                }
                strTrimmedHTMLBody = strTrimmedHTMLBody.Replace(strActualImgTag, strTrimImgTag);
                start = strHTMLBody.IndexOf("<img", start + 1);
            }
            return strTrimmedHTMLBody;
        }

        private string ExportMailToHtml(MailMessage mail)
        {
            string Htmltext = string.Empty;
            //int index = 1;
            foreach (AlternateView altView in mail.AlternateViews)
            {
                Htmltext += ExtractAlternateView(altView);
                //Console.WriteLine(index.ToString() + ". " + Htmltext);
            }
            return Htmltext;
        }

        private string ExtractAlternateView(AlternateView altView)
        {
            var dataStream = altView.ContentStream;
            byte[] byteBuffer = new byte[dataStream.Length];
            return System.Text.Encoding.ASCII.GetString(byteBuffer, 0, dataStream.Read(byteBuffer, 0, byteBuffer.Length));
        }

        private static string TextToHtml(string text)
        {
            string result = string.Empty;
            //string[] splits = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            //string[] splits = text.Split(Environment.NewLine.ToCharArray());
            foreach (var line in text.SplitToLines())
            {
                result += "<span style='font-family:\"Neo Tech\",sans-serif'>" + line + " </span><br>";
            }

            return result;
        }
    }
}
