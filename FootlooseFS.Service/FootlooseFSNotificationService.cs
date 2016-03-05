using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon;
using System.Configuration;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;

namespace FootlooseFS.Service
{
    public class FootlooseFSNotificationService : IFootlooseFSNotificationService
    {
        public bool SendPersonUpdatedNotification(int personId, string message)
        {
            return true;              
        }
    }
}
