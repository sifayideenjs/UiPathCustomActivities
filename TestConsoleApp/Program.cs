using System;
using System.Activities;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GetFiles();

            //try
            //{
            //    string workFlowPath = @"C:\Users\sifhameed\Documents\UiPath\PlayGround\SearchGoogle.xaml";
            //    StringBuilder xamlWFString = GetXamlString(workFlowPath);

            //    StringReader xmlReader = new StringReader(xamlWFString.ToString());
            //    //XamlXmlReader xmlReader = new XamlXmlReader(workFlowPath);

            //    Activity wfInstance = ActivityXamlServices.Load(xmlReader);

            //    //WorkflowApplication workFlowApp = new WorkflowApplication(wfInstance);
            //    //workFlowApp.Run();

            //    WorkflowInvoker invoker = new WorkflowInvoker(wfInstance);
            //    var actual = invoker.Invoke();
            //    foreach (var key in actual.Keys)
            //    {
            //        Console.WriteLine(key);
            //    }
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //Console.ReadLine();
        }

        private static StringBuilder GetXamlString(string workFlowFilePath)
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

        private static void GetFiles()
        {
            string searchDirectory = @"C:\Users\sifhameed\Desktop\Test\salam";
            string[] pdfFiles = Directory.GetFiles(searchDirectory, "*.pdf");
            //var prefixes = pdfFiles.GroupBy(x => Path.GetFileNameWithoutExtension(x).Split('_')[0]).Select(y => new { Prefix = y.Key, Count = y.Count() });
            //foreach (var prefix in prefixes)
            //{
            //    Console.WriteLine("Account Number: {0}, Count: {1}", prefix.Prefix, prefix.Count);
            //}

            //var groups = pdfFiles.GroupBy(x => Path.GetFileNameWithoutExtension(x).Split('_')[0]);
            //var groupGreater3 = groups.Where(y => y.Count() == 3);
            //var accounts = groupGreater3.Select(z => z.Key);

            //accounts.ToList().ForEach(x =>
            //    {
            //        Console.WriteLine("Account Number: {0}", x);
            //    }
            //);

            //foreach (var x in accounts)
            //{
            //    Console.WriteLine("Account Number: {0}", x);
            //}

            //var prefixes = pdfFiles.GroupBy(x => Path.GetFileNameWithoutExtension(x).Split('_')[0]).Where(y => y.Count() == 3).Select(y => y.Key);
            //foreach (var prefix in prefixes)
            //{
            //    Console.WriteLine("Account Number: {0}", prefix);
            //}
        }
    }
}
