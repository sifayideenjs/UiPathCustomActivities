using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HashCode.SecureStringConverter.Activities
{
    public class RegularStringToSecureStringActivity : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> PlainString { get; set; }

        [Category("Input")]
        public bool MakeReadOnly { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<SecureString> SecureString { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (context == null) return;

            var plainString = this.PlainString.Get(context);
            var makeReadOnly = this.MakeReadOnly;

            if (string.IsNullOrEmpty(plainString))
            {
                Console.WriteLine("Input Plain String is NULL or EMPTY");
            }
            else
            {
                try
                {
                    var secureString = ConvertToSecureString(plainString, makeReadOnly);
                    this.SecureString.Set(context, secureString);
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception has been caught::" + e.ToString());
                }
            }
        }

        private SecureString ConvertToSecureString(string plainString, bool makeReadOnly)
        {
            if (plainString == null)
                throw new ArgumentNullException("PlainString");

            var secureString = new SecureString();

            foreach (char c in plainString)
                secureString.AppendChar(c);

            if(makeReadOnly) secureString.MakeReadOnly();
            return secureString;
        }
    }
}