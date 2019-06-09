using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.QueueActivity
{
    public class QueueItemDto
    {
        public int QueueDefinitionId { get; set; }
        public object OutputData { get; set; }
        public string Status { get; set; }
        public string ReviewStatus { get; set; }
        public object ReviewerUserId { get; set; }
        public string Key { get; set; }
        public object Reference { get; set; }
        public string ProcessingExceptionType { get; set; }
        public object DueDate { get; set; }
        public string Priority { get; set; }
        public object DeferDate { get; set; }
        public DateTime StartProcessing { get; set; }
        public DateTime EndProcessing { get; set; }
        public int SecondsInPreviousAttempts { get; set; }
        public int? AncestorId { get; set; }
        public int RetryNumber { get; set; }
        public string SpecificData { get; set; }
        public DateTime CreationTime { get; set; }
        public object Progress { get; set; }
        public string RowVersion { get; set; }
        public int Id { get; set; }
        public ProcessingException ProcessingException { get; set; }
        public object SpecificContent { get; set; }
        public object Output { get; set; }
    }

    public class ProcessingException
    {
        public string Reason { get; set; }
        public string Details { get; set; }
        public string Type { get; set; }
        public object AssociatedImageFilePath { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
