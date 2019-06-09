using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;

namespace Etisalat.CIT.OPS.Robotics.Outlook
{
    public class ReadOutlookMail : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<MailMessage> MailMessage { get; set; }

        [Category("Output")]
        public OutArgument<string> To { get; set; }

        [Category("Output")]
        public OutArgument<string> From { get; set; }

        [Category("Output")]
        public OutArgument<List<string>> CC { get; set; }

        [Category("Output")]
        public OutArgument<List<string>> BCC { get; set; }

        [Category("Output")]
        public OutArgument<string> DateTime { get; set; }

        [Category("Output")]
        public OutArgument<string> Subject { get; set; }

        [Category("Output")]
        public OutArgument<string> Body { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            MailMessage mailMessage = MailMessage.Get(context);
            if (mailMessage != null)
            {
                this.To.Set(context, mailMessage.To.ToString());
                this.From.Set(context, mailMessage.From.ToString());

                List<string> ccs = new List<string>();
                foreach(var cc in mailMessage.CC)
                {
                    ccs.Add(cc.ToString());
                }
                this.CC.Set(context, ccs);

                List<string> bccs = new List<string>();
                foreach (var bcc in mailMessage.Bcc)
                {
                    bccs.Add(bcc.ToString());
                }
                this.BCC.Set(context, bccs);

                this.DateTime.Set(context, mailMessage.Headers["Date"]);
                this.Subject.Set(context, mailMessage.Subject.ToString());
                this.Body.Set(context, mailMessage.Body.ToString());
            }
        }
    }
}
