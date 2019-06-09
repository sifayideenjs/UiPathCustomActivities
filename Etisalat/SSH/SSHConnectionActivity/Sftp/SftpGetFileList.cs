﻿using Renci.SshNet;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Sftp
{
    public class SftpGetFileList : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<SftpClient> SftpClient { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> RemoteDirectory { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public InArgument<string[]> RemoteFilePaths { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            SftpClient sftp = this.SftpClient.Get(context);
            if (sftp != null)
            {
                if (sftp.IsConnected)
                {
                    try
                    {
                        string remoteDirectory = this.RemoteDirectory.Get(context);
                        Console.WriteLine("Remote Directory: " + remoteDirectory);

                        int index = 1;
                        var files = sftp.ListDirectory(remoteDirectory);
                        foreach (var file in files)
                        {
                            Console.WriteLine(index.ToString() + "." + file.Name);
                            index++;
                        }

                        this.RemoteFilePaths.Set(context, files.ToArray());
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
