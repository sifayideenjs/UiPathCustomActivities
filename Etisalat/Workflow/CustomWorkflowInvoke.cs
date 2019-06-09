using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xaml;

namespace Etisalat.CIT.OPS.Robotics.Workflow
{
    public class CustomWorkflowInvoke : NativeActivity
    {
        [Category("Input")]
        public InArgument<string> WorkFlowFilePath { get; set; }
        
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                var workFlowPath = WorkFlowFilePath.Get(context);
                StringBuilder xamlWFString = null;
                if (string.IsNullOrEmpty(workFlowPath))
                {
                    xamlWFString = GetResourceString("SearchGoogle.xaml");

                    StringReader stringReader = new StringReader(xamlWFString.ToString());
                    Activity wfInstance = ActivityXamlServices.Load(stringReader);
                    //WorkflowInvoker.Invoke(wfInstance);

                    //WorkflowApplication workFlowApp = new WorkflowApplication(wfInstance);
                    //workFlowApp.Run();

                    WorkflowInvoker invoker = new WorkflowInvoker(wfInstance);
                    var actual = invoker.Invoke();
                    foreach(var key in actual.Keys)
                    {
                        Console.WriteLine(key);
                    }
                }
                else
                {
                    xamlWFString = GetXamlString(workFlowPath);

                    StringReader xmlReader = new StringReader(xamlWFString.ToString());
                    //XamlXmlReader xmlReader = new XamlXmlReader(workFlowPath);

                    Activity wfInstance = ActivityXamlServices.Load(xmlReader);

                    //WorkflowApplication workFlowApp = new WorkflowApplication(wfInstance);
                    //workFlowApp.Run();

                    WorkflowInvoker invoker = new WorkflowInvoker(wfInstance);
                    var actual = invoker.Invoke();
                    foreach (var key in actual.Keys)
                    {
                        Console.WriteLine(key);
                    }
                }
                Thread.Sleep(10000);
            }
            catch (Exception e)
            {
                string logMessage = e.ToLogString(Environment.StackTrace);
                ShowLog("Stack Trace: " + logMessage);

                ShowLog("Exception: " + e.Message);

                if (e.InnerException != null)
                {
                    ShowLog("Inner Exception::" + e.InnerException.Message);
                }
            }
        }

        private StringBuilder GetXamlString(string workFlowFilePath)
        {
            string result = string.Empty;
            StringBuilder xamlWFString = new StringBuilder();
            StreamReader xamlStreamReader = new StreamReader(workFlowFilePath);
            while (result != null)
            {
                result = xamlStreamReader.ReadLine();
                if (result != null)
                {
                    xamlWFString.Append(result);
                }
            }
            return xamlWFString;
        }

        private StringBuilder GetResourceString(string filename)
        {
            string result = string.Empty;
            StringBuilder xamlWFString = new StringBuilder();
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Etisalat.CIT.OPS.Robotics." + filename))
            {
                using (StreamReader xamlStreamReader = new StreamReader(stream))
                {
                    while (result != null)
                    {
                        result = xamlStreamReader.ReadLine();
                        if (result != null)
                        {
                            xamlWFString.Append(result);
                        }
                    }
                }
            }
            return xamlWFString;
        }

        private void ShowLog(string message)
        {
            Console.WriteLine(message);
        }

        static ICollection<InArgumentInfo> GetInArgumentsInfos(Activity activity)
        {
            var properties = activity.GetType()
                .GetProperties()
                .Where(p => typeof(InArgument).IsAssignableFrom(p.PropertyType))
                .ToList();

            var argumentsCollection = new Collection<InArgumentInfo>();

            foreach (var property in properties)
            {
                var descAttribute = property
                    .GetCustomAttributes(false)
                    .OfType<DescriptionAttribute>()
                    .FirstOrDefault();

                string description = descAttribute != null && !string.IsNullOrEmpty(descAttribute.Description) ?
                    descAttribute.Description :
                    string.Empty;

                bool isRequired = property
                    .GetCustomAttributes(false)
                    .OfType<RequiredArgumentAttribute>()
                    .Any();

                argumentsCollection.Add(new InArgumentInfo
                {
                    InArgumentName = property.Name,
                    InArgumentDescription = description,
                    InArgumentIsRequired = isRequired
                });
            }

            return argumentsCollection;
        }
    }

    class InArgumentInfo
    {
        public string InArgumentName { get; set; }
        public string InArgumentDescription { get; set; }
        public bool InArgumentIsRequired { get; set; }
    }
}
