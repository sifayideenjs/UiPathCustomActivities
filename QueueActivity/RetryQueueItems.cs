using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.QueueActivity
{
    public class RetryQueueItems : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<double> BeforeMinutes { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<DateTime> CurrentDateTime { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<string> Status { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (context == null) return;

            var minutes = this.BeforeMinutes.Get(context);
            var queueName = this.QueueName.Get(context);
            var currentDateTime = this.CurrentDateTime.Get(context);
            WriteLine("Current DateTime : " + currentDateTime.ToString());

            queueName = queueName.Trim();

            try
            {
                string token = string.Empty;
                bool queueFound = false;

                if (string.IsNullOrEmpty(token)) token = Get_Token();
                var queueDefinitions = OrchestratorAPIHelper.GetQueueDefinitionByName(token, queueName);
                if (queueDefinitions.Length > 0)
                {
                    queueFound = true;
                    var queueItem = queueDefinitions.First();
                    queueName = queueItem.QueueDefinitionName;
                }
                else
                {
                    WriteLine("No Queues with name '" + queueName + "' exist in Orchestrator.. Please use a valid queue name.", context);
                }

                if (queueFound)
                {
                    var queueDefinition = queueDefinitions.Single(q => q.QueueDefinitionName.ToUpper() == queueName.ToUpper());
                    queueName = queueDefinition.QueueDefinitionName;

                    var queueItems = OrchestratorAPIHelper.GetAllFailedQueueItems(token, queueDefinition.QueueDefinitionId);
                    int count = queueItems.Count();
                    if (count > 0)
                    {
                        var dateTime = currentDateTime.AddMinutes(minutes * -1);
                        WriteLine("From DateTime : " + dateTime.ToString());

                        var filteredQueueItems = queueItems.Where(q => q.StartProcessing >= dateTime);
                        count = filteredQueueItems.Count();
                        if (count > 0)
                        {
                            foreach (var qItem in filteredQueueItems)
                            {
                                OrchestratorAPIHelper.SetItemReviewStatus(token, qItem.Id, qItem.RowVersion);
                            }

                            WriteLine("Successfully retried " + count + " transaction items for queue '" + queueName + "'.", context);
                        }
                        else
                        {
                            WriteLine("No failed transaction items in past " + minutes + " minutes", context);
                        }
                    }
                    else
                    {
                        WriteLine("No top 50 failed transaction items exist for queue '" + queueName + "'.", context);
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine("An exception has been caught::" + e.ToString(), context);
            }
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

        private void WriteLine(string message, CodeActivityContext context = null)
        {
            if (context != null) this.Status.Set(context, message);
            Console.WriteLine(message);
        }
    }
}
