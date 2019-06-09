using Renci.SshNet;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Sftp
{
    public class SftpConnection : CodeActivity
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
        public OutArgument<SftpClient> SftpClient { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var host = this.Host.Get(context);
            var username = this.Username.Get(context);
            var password = this.Password.Get(context);
            var port = this.Port.Get(context);

            SftpClient sftp = null;
            if(port > 0)
            {
                sftp = new SftpClient(host, port, username, password);
            }
            else
            {
                sftp = new SftpClient(host, username, password);
            }
            try
            {
                sftp.Connect();
                sftp.KeepAliveInterval = TimeSpan.FromSeconds(60);
                sftp.ConnectionInfo.Timeout = TimeSpan.FromMinutes(180);
                sftp.OperationTimeout = TimeSpan.FromMinutes(180);
                SftpClient.Set(context, sftp);
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception has been caught " + e.ToString());
            }
        }
    }
}
