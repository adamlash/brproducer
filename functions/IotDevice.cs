using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using BladeRunner.Models;


namespace BladeRunner.IoTDevice
{

    public static class IotDevice
    {
        private static string s_connectionString1 = System.Environment.GetEnvironmentVariable("IoTHubConnection", EnvironmentVariableTarget.Process);
        private static readonly TransportType s_transportType = TransportType.Mqtt;
        private static DeviceClient adeviceClient = DeviceClient.CreateFromConnectionString(s_connectionString1, "aaaa", s_transportType);
        private static DeviceClient bdeviceClient = DeviceClient.CreateFromConnectionString(s_connectionString1, "bbbb", s_transportType);
        private static DeviceClient cdeviceClient = DeviceClient.CreateFromConnectionString(s_connectionString1, "cccc", s_transportType);
        private static DeviceClient ddeviceClient = DeviceClient.CreateFromConnectionString(s_connectionString1, "dddd", s_transportType);
        private static DeviceClient edeviceClient = DeviceClient.CreateFromConnectionString(s_connectionString1, "eeee", s_transportType);

        [FunctionName("IotDevice")]
        public static async void Run([ServiceBusTrigger("brevents", "IoTDevices", Connection = "ServiceBusConnection")]string mySbMsg, ILogger log)
        {
            var scheduledDevice = JsonConvert.DeserializeObject<Device>(mySbMsg);
            using var message = new Message(Encoding.ASCII.GetBytes(mySbMsg))
            {
                ContentType = "application/json",
                ContentEncoding = "utf-8",
            };

            switch (scheduledDevice.DeviceId)
            {
                case "aaaa":
                    await adeviceClient.SendEventAsync(message);
                    log.LogInformation($"Message Sent to {scheduledDevice.DeviceId}");
                    await Task.Delay(1000);
                    break;
                case "bbbb":
                    await bdeviceClient.SendEventAsync(message);
                    log.LogInformation($"Message Sent to {scheduledDevice.DeviceId}");
                    await Task.Delay(1000);
                    break;
                case "cccc":
                    await cdeviceClient.SendEventAsync(message);
                    log.LogInformation($"Message Sent to {scheduledDevice.DeviceId}");
                    await Task.Delay(1000);
                    break;
                case "dddd":
                    await ddeviceClient.SendEventAsync(message);
                    log.LogInformation($"Message Sent to {scheduledDevice.DeviceId}");
                    await Task.Delay(1000);
                    break;
                case "eeee":
                    await edeviceClient.SendEventAsync(message);
                    log.LogInformation($"Message Sent to {scheduledDevice.DeviceId}");
                    await Task.Delay(1000);
                    break;                                                            
            }   
            log.LogInformation($"Message Sent to IotHub: {mySbMsg}");
            
        }
    }
}
