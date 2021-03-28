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
            // Create Message
            var scheduledDevice = JsonConvert.DeserializeObject<Device>(requestBody);
            var message = new Message(Encoding.UTF8.GetBytes(requestBody))
            {
                MessageId = Guid.NewGuid().ToString("N"),
                ScheduledEnqueueTimeUtc = DateTime.UtcNow + TimeSpan.FromMinutes(1)
            };


            // var responseMessage = new Device()
            // {
            //     Time = Time,
            //     Date = Date,
            //     DeviceId = DeviceId,
            //     Telemetry = Telemetry,
            // };

            await collector.AddAsync(message);
            
            log.LogInformation("C# HTTP trigger function processed a request");
            return new AcceptedResult();
        }
    }
}
