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
            var topicArn = ConfigurationManager.AppSettings["AWSSNSTopicArn"];

            var snsClient = new AmazonSimpleNotificationServiceClient(RegionEndpoint.USEast1);
          
            try
            {                
                var publishRequest = new Amazon.SimpleNotificationService.Model.PublishRequest();
                publishRequest.TopicArn = topicArn;
                publishRequest.Message = message;
                publishRequest.Subject = personId.ToString();

                snsClient.Publish(publishRequest);

                return true;
            }
            catch (Exception ex)
            {
                // TODO Log exception
                return false;
            }                       
        }
    }
}
