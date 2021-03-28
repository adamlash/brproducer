using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BladeRunner.Models;

namespace BladeRunner.PopulateTopic
{
    public static class PopulateTopic
    {
        [FunctionName("PopulateTopic")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            [ServiceBus("brevents", Connection = "ServiceBusConnection")]IAsyncCollector<Message> collector)
        {
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var scheduledDevice = JsonConvert.DeserializeObject<Device>(requestBody);
            // Convert DateTime to Something Service Bus Can Use
            DateTime surfaceTime = DateTime.Parse(scheduledDevice.Time + " " + scheduledDevice.Date);
            // Create Message
            var message = new Message(Encoding.UTF8.GetBytes(requestBody))
            {
                MessageId = Guid.NewGuid().ToString("N"),
                ScheduledEnqueueTimeUtc = surfaceTime
            };

            await collector.AddAsync(message);
            
            log.LogInformation("C# HTTP trigger function processed a request");
            return new AcceptedResult();
        }
    }
}
