using Renci.SshNet;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.SSH
{
    public class SSHConnection : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Host { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Username { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Password { get; set; }

        [Category("Input")]
        public InArgument<int> Port { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<SshClient> SshClient { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var host = this.Host.Get(context);
            var username = this.Username.Get(context);
            var password = this.Password.Get(context);
            var port = this.Port.Get(context);

            SshClient ssh = null;
            if(port > 0)
            {
                ssh = new SshClient(host, port, username, password);
            }
            else
            {
                ssh = new SshClient(host, username, password);
            }
            try
            {
                ssh.Connect();
                SshClient.Set(context, ssh);
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has been caught " + e.ToString());
            }
        }
    }
}
