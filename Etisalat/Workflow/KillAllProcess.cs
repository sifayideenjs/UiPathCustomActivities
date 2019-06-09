using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Process
{
    public enum BROWERTYPE
    {
        ID = 0,
        CHROME,
        FIREFOX
    }

    public class KillAllProcess : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public BROWERTYPE BrowserType { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string processName = string.Empty;
            switch (BrowserType)
            {
                case BROWERTYPE.ID:
                    {
                        processName = "iexplore";
                        break;
                    }
                case BROWERTYPE.CHROME:
                    {
                        processName = "chrome";
                        break;
                    }
                case BROWERTYPE.FIREFOX:
                    {
                        processName = "firefox";
                        break;
                    }
            }

            Console.WriteLine("Kill Process: " + processName);

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            Console.WriteLine("Current User: " + userName);

            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            for (int p = 0; p < processes.Length; p++)
            {
                var process = processes[p];
                var pName = process.ProcessName;
                var pUserName = process.StartInfo.EnvironmentVariables["username"];

                Console.WriteLine(process.ToString() + " - " + pName + " - " + pUserName);

                if (pName.ToLower() == processName.ToLower() && pUserName.ToLower() == userName.ToLower())
                {
                    processes[p].Kill();
                    Console.WriteLine("Status: KILLED");
                }
                else
                {
                    Console.WriteLine("Status: NOT KILLED");
                }

                Console.WriteLine(string.Empty);
            }
        }
    }
}
