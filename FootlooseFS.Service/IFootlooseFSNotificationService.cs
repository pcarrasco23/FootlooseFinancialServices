using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Service
{
    public interface IFootlooseFSNotificationService
    {
        bool SendPersonUpdatedNotification(int personId, string message);
    }
}
