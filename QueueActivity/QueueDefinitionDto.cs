using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.QueueActivity
{
    public class QueueDefinitionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxNumberOfRetries { get; set; }
        public bool AcceptAutomaticallyRetry { get; set; }
        public bool ArchiveItems { get; set; }
        public string CreationTime { get; set; }
        public int Id { get; set; }
    }
}
