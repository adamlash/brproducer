using System;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BladeRunner.Models;
using BladeRunner.Devices;
using System.Net.Http;



namespace BladeRunner.IoTDevice
{

    public static class IotDevice
    {
        private static string s_connectionString1 = System.Environment.GetEnvironmentVariable("IoTHubConnection", EnvironmentVariableTarget.Process);

        [FunctionName("IotDevice")]
        public static async void Run([ServiceBusTrigger("brevents", "TestingSub", Connection = "ServiceBusConnection")]string mySbMsg, ILogger log)
        {
            // Construct Message
            var scheduledDevice = JsonConvert.DeserializeObject<Device>(mySbMsg);
            using var message = new StringContent(mySbMsg, Encoding.UTF8, "application/json");
            // Generate Token
            var token = GenerateToken.generateSasToken("bladerunner-producerhub.azure-devices.net/devices", s_connectionString1, "IoTDeviceFunction", 5);
            var url = $"https://bladerunner-producerhub.azure-devices.net/devices/{scheduledDevice.DeviceId}/messages/events?api-version=2019-03-30";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await client.PostAsync(url, message);
            
            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
            
        }
    }
}
