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
    public class SSHRunCommand : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<SshClient> SshClient { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> CommandText { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<string> Result { get; set; }

        [Category("Output")]
        public OutArgument<string> Error { get; set; }

        [Category("Output")]
        public OutArgument<int> ExitStatus { get; set; }

        [Category("Output")]
        public OutArgument<TimeSpan> CommandTimeOut { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            SshClient ssh = this.SshClient.Get(context);
            if (ssh != null)
            {
                if (ssh.IsConnected)
                {
                    try
                    {
                        string commandText = this.CommandText.Get(context);
                        SshCommand command = ssh.RunCommand(commandText);
                        if(command != null)
                        {
                            this.Result.Set(context, command.Result);
                            Console.WriteLine("Result: " + command.Result);

                            this.ExitStatus.Set(context, command.ExitStatus);
                            Console.WriteLine("Exit Status: " + command.ExitStatus);

                            this.CommandTimeOut.Set(context, command.CommandTimeout);
                            Console.WriteLine("Command Timeout: " + command.CommandTimeout);

                            string error = command.Error;
                            this.Error.Set(context, error);
                            if (!string.IsNullOrEmpty(error))
                            {
                                Console.WriteLine("Error: " + error);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unable to execute the SSH Command on Host");
                        }
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
