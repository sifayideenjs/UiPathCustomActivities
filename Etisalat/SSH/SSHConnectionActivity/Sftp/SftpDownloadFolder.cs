using Renci.SshNet;
using Renci.SshNet.Sftp;
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
    public class SftpDownloadFolder : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<SftpClient> SftpClient { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> SourceRemoteFolder { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> DestinationLocalFolder { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<bool> IncludeSubFolders { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            SftpClient sftp = this.SftpClient.Get(context);
            if (sftp != null)
            {
                if (sftp.IsConnected)
                {
                    try
                    {
                        string source = this.SourceRemoteFolder.Get(context);
                        string destination = this.DestinationLocalFolder.Get(context);
                        bool recursive = this.IncludeSubFolders.Get(context);
                        DownloadDirectory(sftp, source, destination, recursive);
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

        /// <summary>
        /// Downloads a remote directory into a local directory
        /// </summary>
        /// <param name="client"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void DownloadDirectory(SftpClient client, string source, string destination, bool recursive = false)
        {
            // List the files and folders of the directory
            var files = client.ListDirectory(source);

            // Iterate over them
            foreach (SftpFile file in files)
            {
                // If is a file, download it
                if (!file.IsDirectory && !file.IsSymbolicLink)
                {
                    DownloadFile(client, file, destination);
                }
                // If it's a symbolic link, ignore it
                else if (file.IsSymbolicLink)
                {
                    Console.WriteLine("Symbolic link ignored: {0}", file.FullName);
                }
                // If its a directory, create it locally (and ignore the .. and .=) 
                //. is the current folder
                //.. is the folder above the current folder -the folder that contains the current folder.
                else if (file.Name != "." && file.Name != "..")
                {
                    var dir = Directory.CreateDirectory(Path.Combine(destination, file.Name));
                    // and start downloading it's content recursively :) in case it's required
                    if (recursive)
                    {
                        DownloadDirectory(client, file.FullName, dir.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// Downloads a remote file through the client into a local directory
        /// </summary>
        /// <param name="client"></param>
        /// <param name="file"></param>
        /// <param name="directory"></param>
        private void DownloadFile(SftpClient client, SftpFile file, string directory)
        {
            Console.WriteLine("Downloading {0}", file.FullName);

            using (Stream fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
            {
                client.DownloadFile(file.FullName, fileStream);
            }
        }
    }
}
