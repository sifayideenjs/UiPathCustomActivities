using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Reporting
{
    public class MailMessageToHtml : CodeActivity
    {
        [Category("Input"), RequiredArgument]
        public InArgument<MailMessage> Mail { get; set; }

        [Category("Output")]
        public OutArgument<string> Html { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                MailMessage mail = this.Mail.Get(context);
                string htmlTable = ExportMailToHtml(mail);
                this.Html.Set(context, htmlTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string ExportMailToHtml(MailMessage mail)
        {
            string Htmltext = string.Empty;
            int index = 1;
            foreach (AlternateView altView in mail.AlternateViews)
            {
                Htmltext += ExtractAlternateView(altView);
                Console.WriteLine(index.ToString() + ". " + Htmltext);
            }
            return Htmltext;
        }

        public string ExtractAlternateView(AlternateView altView)
        {
            var dataStream = altView.ContentStream;
            byte[] byteBuffer = new byte[dataStream.Length];
            return System.Text.Encoding.ASCII.GetString(byteBuffer, 0, dataStream.Read(byteBuffer, 0, byteBuffer.Length));
        }
    }
}