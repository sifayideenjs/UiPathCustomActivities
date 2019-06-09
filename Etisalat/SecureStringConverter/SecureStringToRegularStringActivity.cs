using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.SecureStringActivities
{
    public class SecureStringToRegularStringActivity : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<SecureString> SecureString { get; set; }
        
        [Category("Output")]
        [RequiredArgument]
        public OutArgument<string> PlainString { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (context == null) return;

            var secureString = this.SecureString.Get(context);

            if(secureString == null)
            {
                Console.WriteLine("Input Secure String is NULL");
            }
            else
            {
                try
                {
                    var plainString = ConvertToPlainString(secureString);
                    this.PlainString.Set(context, plainString);
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception has been caught::" + e.ToString());
                }
            }
        }

        private string ConvertToPlainString(SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException("SecureString");

            IntPtr stringPointer = Marshal.SecureStringToBSTR(secureString);
            string plainString = Marshal.PtrToStringBSTR(stringPointer);
            Marshal.ZeroFreeBSTR(stringPointer);

            return plainString;
        }
    }
}