using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace BladeRunner.TestTopic
{
    public static class TestTopic
    {
        [FunctionName("TestTopic")]
        [return: ServiceBus("testtopic", Connection = "ServiceBusConnection")]
        public static string Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            return requestBody;
        }
    }
}
