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
        IE = 0,
        CHROME,
        FIREFOX
    }

    public class KillAllProcess : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public BROWERTYPE BrowserType { get; set; }

        [Category("Misc")]
        public bool ShowLog { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            bool canShowLog = this.ShowLog;
            string processName = string.Empty;
            switch (BrowserType)
            {
                case BROWERTYPE.IE:
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

            DisplayLog("Kill Process: " + processName, canShowLog);

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            userName = userName.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Last();
            DisplayLog("Current User: " + userName, canShowLog);

            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            var matchedProceses = processes.Where(p => p.ProcessName.ToLower() == processName.ToLower() && p.StartInfo.EnvironmentVariables["username"].ToLower() == userName.ToLower()).ToList();
            int pCount = matchedProceses.Count;
            if(pCount == 0)
            {
                DisplayLog("Unable to find current process running under name: " + processName.ToUpper(), canShowLog);
            }
            else
            {
                for (int p = 0; p < pCount; p++)
                {
                    var process = matchedProceses[p];
                    var pName = process.ProcessName;
                    var pUserName = process.StartInfo.EnvironmentVariables["username"];

                    DisplayLog((p + 1).ToString() + " - " + pName + " - " + pUserName, canShowLog);

                    try
                    {
                        process.Kill();
                        DisplayLog("Status: KILLED", canShowLog);
                    }
                    catch (Exception e2)
                    {
                        DisplayLog("Status: FAILED TO KILL", canShowLog);
                        DisplayLog("EXCEPTION: " + e2.Message, canShowLog);
                    }

                    DisplayLog(string.Empty, canShowLog);
                }
            }
        }

        private void DisplayLog(string message, bool canShowLog)
        {
            if(canShowLog) Console.WriteLine(message);
        }
    }
}
