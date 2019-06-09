using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Dialog
{
    public class FadingMessageBox : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Message { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Title { get; set; }

        [Category("Input")]
        public InArgument<int> FadingTimeOut { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string message = Message.Get(context);
            string title = Title.Get(context);
            int fadingTimeOut = FadingTimeOut.Get(context);

            FadingMessageBoxDialog messageBox = new FadingMessageBoxDialog(message, title);
            messageBox.Show();
            //if(fadingTimeOut == 0)
            //{
            //    Thread.Sleep(5000);
            //}
            //else
            //{
            //    Thread.Sleep(fadingTimeOut);
            //}

            //messageBox.HideMe();
        }
    }
}
