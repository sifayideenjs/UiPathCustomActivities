using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.QueueActivity
{
    public class PushToQueue : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<DataTable> QueueDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<string> Status { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (context == null) return;

            var queueDataTable = this.QueueDataTable.Get(context);
            var queueName = this.QueueName.Get(context);

            if (queueDataTable is null)
            {
                WriteLine("Queue Data Table is NULL or EMPTY, Please check the workflow.", context);
            }
            else if (string.IsNullOrEmpty(queueName))
            {
                WriteLine("Queue Name is NULL or EMPTY, Please check the workflow.", context);
            }
            else
            {
                try
                {
                    string token = string.Empty;
                    bool queueFound = false;

                    if (string.IsNullOrEmpty(token)) token = Get_Token();
                    var queueDefinitions = OrchestratorAPIHelper.GetQueueDefinitions(token);
                    if (queueDefinitions.Length > 0)
                    {
                        queueFound = queueDefinitions.Any(q => q.Name.ToUpper() == queueName.ToUpper());
                        if (queueFound == false)
                        {
                            WriteLine("Queue name does not exist. Please use a valid queue name.", context);
                        }
                        else
                        {
                            var queueItem = queueDefinitions.Single(q => q.Name.ToUpper() == queueName.ToUpper());
                            queueName = queueItem.Name;
                        }
                    }
                    else
                    {
                        WriteLine("No Queues with name '" + queueName + "' exist in Orchestrator.", context);
                    }

                    if(queueFound)
                    {
                        //Method 1:
                        var dictionaryList = ExcelUtility.DataTableToDictionaryList(queueDataTable);

                        foreach (var dictionary in dictionaryList)
                        {
                            Dictionary<string, object> queueDictionay = new Dictionary<string, object>();
                            foreach (var key in dictionary.Keys)
                            {
                                queueDictionay.Add(key, dictionary[key]);
                            }

                            var specificContent = GetDynamicObject(queueDictionay);
                            Add_to_Queue(token, queueName, specificContent);
                        }

                        //Method 2:
                        //var transactionItems = ExcelUtility.ExcelToList<TransactionItem>(queueInputFile);
                        //foreach (var transactionItem in transactionItems)
                        //{
                        //    transactionItem.SpecificData = transactionItem.SpecificData.TrimStart(("{\"DynamicProperties\":").ToCharArray());
                        //    transactionItem.SpecificData = transactionItem.SpecificData.TrimEnd('}');
                        //    transactionItem.SpecificData = "{\"" + transactionItem.SpecificData + "}";
                        //    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(transactionItem.SpecificData);
                        //    Dictionary<string, object> queueDictionay = new Dictionary<string, object>();
                        //    foreach (var key in dictionary.Keys)
                        //    {
                        //        queueDictionay.Add(key, dictionary[key]);
                        //    }

                        //    var specificContent = GetDynamicObject(queueDictionay);
                        //    Add_to_Queue(token, queueName, specificContent);
                        //}

                        WriteLine("Successfully added transaction items to queue '" + queueName + "'.", context);
                    }
                }
                catch (Exception e)
                {
                    WriteLine("An exception has been caught::" + e.ToString(), context);
                }
            }
        }

        private void Add_to_Queue(string token, string queueName, object specificContent)
        {
            OrchestratorAPIHelper.AddQueue(token, queueName, specificContent);
        }

        private string Get_Token()
        {
            var loginModel = new
            {
                tenancyName = string.Empty,
                usernameOrEmailAddress = "username",
                password = "password"
            };

            string token = OrchestratorAPIHelper.GetToken(loginModel);
            return token;
        }

        private dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new MiDynamicObject(properties);
        }

        private void WriteLine(string message, CodeActivityContext context = null)
        {
            if (context != null) this.Status.Set(context, message);
            Console.WriteLine(message);
        }
    }
}
