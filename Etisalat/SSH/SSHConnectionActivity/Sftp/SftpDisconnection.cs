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
    public class SftpDisconnection : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<SftpClient> SftpClient { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            SftpClient sftp = this.SftpClient.Get(context);
            if(sftp != null)
            {
                if (sftp.IsConnected)
                {
                    try
                    {
                        sftp.Disconnect();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception has been caught " + e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Sftp Connection is not opened");
                }
            }
            else
            {
                Console.WriteLine("Sftp Client is null");
            }
        }
    }
}
