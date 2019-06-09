using System;
using System.Activities;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Etisalat.CIT.OPS.Robotics.Workflow
{
    public class TraceMessage : NativeActivity
    {
        public class WorkflowInstanceInfo : IWorkflowInstanceExtension
        {
            public IEnumerable<object> GetAdditionalExtensions()
            {
                yield break;
            }

            public void SetInstance(WorkflowInstanceProxy instance)
            {
                this.proxy = instance;
            }

            private WorkflowInstanceProxy proxy;

            public WorkflowInstanceProxy GetProxy() { return proxy; }
        }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Message { get; set; }

        [Category("Output")]
        public OutArgument<string> OutMessage { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            string message = Message.Get(context);

            //UiPath.Core.Activities.LogMessage logMessage = new UiPath.Core.Activities.LogMessage();
            //logMessage.Level = UiPath.Core.Activities.CurentLogLevel.Error;
            //logMessage.Message = message;
            //WorkflowInvoker logInvoker = new WorkflowInvoker(logMessage);
            //logInvoker.Invoke();

            WorkflowInstanceProxy proxy = context.GetExtension<WorkflowInstanceInfo>().GetProxy();
            Activity root = proxy.WorkflowDefinition;
            string trace = string.Format("{0}::{1}", root.DisplayName, message);
            Console.WriteLine(trace);
            this.OutMessage.Set(context, trace);
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.AddDefaultExtensionProvider<WorkflowInstanceInfo>(() => new WorkflowInstanceInfo());
        }
    }
}
