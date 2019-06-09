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
    public class SSHDisconnection : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<SshClient> SshClient { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            SshClient ssh = this.SshClient.Get(context);
            if(ssh != null)
            {
                if (ssh.IsConnected)
                {
                    try
                    {
                        ssh.Disconnect();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception has been caught " + e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("SSH Connection is not opened");
                }
            }
            else
            {
                Console.WriteLine("SSH Client is null");
            }
        }
    }
}
