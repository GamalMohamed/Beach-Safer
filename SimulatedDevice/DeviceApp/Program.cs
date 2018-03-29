using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace SimulatedDevice
{
    class Program
    {
        private const string IotHubUri = "esp-IoTHub.azure-devices.net";
        private const string DeviceKey = "kZ6ZTWce5WKQsyPW5l7voL5r4Ng4rP10IrVAzcAOQIk=";
        private const string DeviceId = "Sim9";
        private const int DbDeviceId = 9;
        private static string _state = "OK";
        private static DeviceClient _deviceClient;
        private static int _messageId = 1;
        private static bool _alert = false;
        private static readonly List<string> Locs = new List<string>(){ "31.025254,29.615265",
                                                                "31.019940,29.613438",
                                                                "31.019459,29.600833",
                                                                "31.019779,29.611176",
                                                                "31.016278,29.603858",
																"31.013271,29.603257",
																"31.023188,29.610756"
                                                              };


        private static Task<MethodResponse> SayLoL(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine("\nLOOOOOOOOOOOOL!!");
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes("'Function invoked successfully!'"), 200));
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            while (true)
            {
                if (_messageId > 4)
                {
                    _alert = true;
                    _state = "Drowning";
                }
                var telemetryDataPoint = new
                {
                    messageId = _messageId++,
                    deviceId = DbDeviceId,
                    location = Locs[4],
                    state = _state
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("Alert", _alert ? "Drowning" : "NA");

                //var message = new Message(Encoding.UTF32.GetBytes(messageString))
                //{
                //    ContentType = "application/json",
                //    ContentEncoding = "utf-32"
                //};

                await _deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} >> Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(15000);
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine(DeviceId + " starting..\n");
            _deviceClient = DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, DeviceKey), TransportType.Mqtt);
            _deviceClient.SetMethodHandlerAsync("SayLoL", SayLoL, null).Wait();
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}
