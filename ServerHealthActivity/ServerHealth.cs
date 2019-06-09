using Cassia;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.ServerHealthActivity
{
    public class ServerHealth : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ServerName { get; set; }

        [Category("Misc")]
        public bool ShowLog { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<DataTable> ServerHealthDataTable { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<string> ServerHealthHtmlTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var serverName = this.ServerName.Get(context);
            Console.WriteLine("Server Name: " + serverName);

            try
            {
                DataTable serverHealthDataTable = new DataTable();
                string serverHealthHtmlTable = string.Empty;
                List<ServerDetail> serverDetails = new List<ServerDetail>();
                ITerminalServicesManager manager = new TerminalServicesManager();
                using (ITerminalServer server = manager.GetRemoteServer(serverName))
                {
                    server.Open();
                    foreach (ITerminalServicesSession session in server.GetSessions())
                    {
                        NTAccount account = session.UserAccount;
                        if (account != null)
                        {
                            AddInfo("UserAccount" + " : " + account);
                            AddInfo("\t" + "ServerName" + " : " + session.Server.ServerName);
                            AddInfo("\t" + "UserName" + " : " + session.UserName);
                            AddInfo("\t" + "DomainName" + " : " + session.DomainName);
                            AddInfo("\t" + "WindowStationName" + " : " + session.WindowStationName);

                            AddInfo("\t" + "SessionId" + " : " + session.SessionId);

                            AddInfo("\t" + "CurrentTime" + " : " + session.CurrentTime);
                            AddInfo("\t" + "LoginTime" + " : " + session.LoginTime);
                            AddInfo("\t" + "LastInputTime" + " : " + session.LastInputTime);
                            AddInfo("\t" + "IdleTime" + " : " + session.IdleTime);
                            AddInfo("\t" + "ConnectTime" + " : " + session.ConnectTime);
                            AddInfo("\t" + "DisconnectTime" + " : " + session.DisconnectTime);

                            AddInfo(string.Empty);
                            AddInfo("\t" + "Processes:");

                            foreach (var process in session.GetProcesses())
                            {
                                AddInfo("\t\t" + process.ProcessName);
                            }

                            AddInfo(string.Empty);

                            serverDetails.Add(new ServerDetail(session));
                        }
                    }
                }

                if(serverDetails.Count > 0)
                {
                    serverHealthDataTable = Utility.ListToDataTable<ServerDetail>(serverDetails);
                    serverHealthHtmlTable = Utility.DataTableToHtml(serverHealthDataTable);
                }

                this.ServerHealthDataTable.Set(context, serverHealthDataTable);
                this.ServerHealthHtmlTable.Set(context, serverHealthHtmlTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION : " + ex.Message);
            }
        }

        //protected override void Execute(CodeActivityContext context)
        //{
        //    var serverName = this.ServerName.Get(context);
        //    Console.WriteLine("Server Name: " + serverName);

        //    try
        //    {
        //        ITerminalServicesManager manager = new TerminalServicesManager();
        //        using (ITerminalServer server = manager.GetRemoteServer(serverName))
        //        {
        //            server.Open();
        //            foreach (ITerminalServicesSession session in server.GetSessions())
        //            {
        //                NTAccount account = session.UserAccount;
        //                if (account != null)
        //                {
        //                    AddInfo("UserAccount" + " : " + account);
        //                    AddInfo("\t" + "ServerName" + " : " + session.Server.ServerName);
        //                    AddInfo("\t" + "UserName" + " : " + session.UserName);
        //                    AddInfo("\t" + "DomainName" + " : " + session.DomainName);
        //                    AddInfo("\t" + "WindowStationName" + " : " + session.WindowStationName);

        //                    AddInfo("\t" + "SessionId" + " : " + session.SessionId);

        //                    AddInfo("\t" + "CurrentTime" + " : " + session.CurrentTime);
        //                    AddInfo("\t" + "LoginTime" + " : " + session.LoginTime);
        //                    AddInfo("\t" + "LastInputTime" + " : " + session.LastInputTime);
        //                    AddInfo("\t" + "IdleTime" + " : " + session.IdleTime);
        //                    AddInfo("\t" + "ConnectTime" + " : " + session.ConnectTime);
        //                    AddInfo("\t" + "DisconnectTime" + " : " + session.DisconnectTime);

        //                    AddInfo(string.Empty);
        //                    AddInfo("\t" + "Processes:");

        //                    foreach (var process in session.GetProcesses())
        //                    {
        //                        AddInfo("\t\t" + process.ProcessName);
        //                    }

        //                    AddInfo(string.Empty);
        //                }
        //            }
        //        }

        //        this.ServerHealthInfo.Set(context, _serverHealthInfo);
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine("EXCEPTION : " + ex.Message);
        //    }
        //}

        private void AddInfo(string message)
        {
            if (this.ShowLog) Console.WriteLine(message);
        }
    }
}
