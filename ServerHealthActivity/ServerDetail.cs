using Cassia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.ServerHealthActivity
{
    public class ServerDetail
    {
        public ServerDetail(ITerminalServicesSession session)
        {
            this.CurrentTime = session.CurrentTime.ToString();
            this.Server = session.Server.ServerName;
            this.WindowStationName = session.WindowStationName;
            this.UserAccount = session.UserAccount.ToString();
            this.DomainName = session.DomainName;
            this.UserName = session.UserName;
            this.SessionId = session.SessionId.ToString();
            this.IdleTime = session.IdleTime.ToString();
            this.LoginTime = session.LoginTime.ToString();
            this.LastInputTime = session.LastInputTime.ToString();
            this.DisconnectTime = session.DisconnectTime.ToString();
            this.ConnectTime = session.ConnectTime.ToString();
            this.ClientName = session.ClientName;
            this.ClientBuildNumber = session.ClientBuildNumber.ToString();

            foreach (var process in session.GetProcesses())
            {
                this.Process += process.ProcessName;
                this.Process += "\n";
            }
        }
        
        public string Server { get; }
        public string SessionId { get; }
        public string UserAccount { get; }
        public string DomainName { get; }
        public string UserName { get; }
        public string WindowStationName { get; }
        public string ClientName { get; }
        public string ClientBuildNumber { get; }
        public string CurrentTime { get; }
        public string ConnectTime { get; }
        public string LoginTime { get; }
        public string LastInputTime { get; }
        public string DisconnectTime { get; }
        public string IdleTime { get; }
        public string Process { get; }
    }
}
