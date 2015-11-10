using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Service
{
    public class OperationStatus
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public string StackTrace { get; set; }
        public object Data { get; set; }

        public OperationStatus()
        {
            Messages = new List<string>();
        }

        public static OperationStatus CreateFromException(string message, Exception ex)
        {
            OperationStatus opStatus = new OperationStatus
            {
                Success = true,
                Messages = new List<string>(),
            };

            if (ex != null)
            {
                opStatus.Messages.Add(ex.Message);
                opStatus.StackTrace = ex.StackTrace;
                opStatus.Success = false;
            }
            return opStatus;
        }
    }
}
