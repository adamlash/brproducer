using System;
using System.IO;
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
        [return: ServiceBus("brevents", Connection = "ServiceBusConnection")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string Time = req.Query["name"];
            string Date = req.Query["name"];
            string DeviceId = req.Query["name"];
            string Telemetry = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            Time = Time ?? data?.Time;
            Date = Date ?? data?.Date;
            DeviceId = DeviceId ?? data?.DeviceId;
            Telemetry = Telemetry ?? data?.Telemetry;

            var responseMessage = new Device()
            {
                Time = Time,
                Date = Date,
                DeviceId = DeviceId,
                Telemetry = Telemetry,
            };
            
            log.LogInformation("C# HTTP trigger function processed a request, here is the response: " + responseMessage);
            return new JsonResult(responseMessage);
        }
    }
}
