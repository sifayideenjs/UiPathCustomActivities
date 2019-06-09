using System;
using System.Activities;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Workflow
{
    public sealed class GetRootActivity : NativeActivity
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

            WorkflowInstanceProxy proxy;

            public WorkflowInstanceProxy GetProxy() { return proxy; }
        }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<Activity> RootActivity { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            WorkflowInstanceProxy proxy = context.GetExtension<WorkflowInstanceInfo>().GetProxy();
            Activity root = proxy.WorkflowDefinition;
            this.RootActivity.Set(context, root);
        }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.AddDefaultExtensionProvider<WorkflowInstanceInfo>(() => new WorkflowInstanceInfo());
        }
    }
}
