using System;
using System.Text;
using Microsoft.Azure.Devices.Client;
using System.Text.Json;
using System.Threading;

namespace BladeRunner.Devices
{
    class DeviceFacade
    {
        public DeviceFacade(string DeviceConnectionString)
        {
            //Create a device connection.
            this.Device = DeviceClient.CreateFromConnectionString(DeviceConnectionString);
        }

        public DeviceClient Device { get; }
    }
}
