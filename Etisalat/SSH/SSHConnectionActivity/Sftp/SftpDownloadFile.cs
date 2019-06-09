using Renci.SshNet;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Sftp
{
    public class SftpDownloadFile : CodeActivity
    {
        [Category("Input")]
        public InArgument<SftpClient> SftpClient { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> RemoteFilePath { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> LocalFilePath { get; set; }

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
        [RequiredArgument]
        public InArgument<int> Port { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            SftpClient sftp = this.SftpClient.Get(context);
            if (sftp != null)
            {
                if(sftp.IsConnected)
                {
                    try
                    {
                        string pathRemoteFile = this.RemoteFilePath.Get(context);
                        Console.WriteLine("Downloading from {0}", pathRemoteFile);

                        string pathLocalFile = this.LocalFilePath.Get(context);
                        Console.WriteLine("Saving to {0}", pathLocalFile);

                        using (Stream fileStream = File.OpenWrite(pathLocalFile))
                        {
                            sftp.DownloadFile(pathRemoteFile, fileStream);
                        }
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
                var host = this.Host.Get(context);
                var username = this.Username.Get(context);
                var password = this.Password.Get(context);
                var port = this.Port.Get(context);

                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    try
                    {
                        client.KeepAliveInterval = TimeSpan.FromSeconds(60);
                        client.ConnectionInfo.Timeout = TimeSpan.FromMinutes(180);
                        client.OperationTimeout = TimeSpan.FromMinutes(180);
                        client.Connect();
                        bool connected = client.IsConnected;

                        string remotePath = this.RemoteFilePath.Get(context);
                        Console.WriteLine("Downloading from {0}", remotePath);

                        string localPath = this.LocalFilePath.Get(context);
                        Console.WriteLine("Saving to {0}", remotePath);

                        var file = File.OpenWrite(localPath);
                        client.DownloadFile(remotePath, file);

                        file.Close();
                        client.Disconnect();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An exception has been caught " + e.ToString());
                    }
                }
            }
        }
    }
}
