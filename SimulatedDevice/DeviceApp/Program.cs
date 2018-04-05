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
        private const string DeviceKey = "kVaUpdJ+OTzAKjs448umoP+3NynNuC0NIesXZSdxxcE=";
        private const string DeviceId = "Sim5";
        private const int DbDeviceId = 5;
        private static string _state = "OK";
        private static DeviceClient _deviceClient;
        private static int _messageId = 1;
        private static bool _alert = false;
        private static string _loc;
        private static readonly List<string> OriginalLocs =
                                             new List<string>(){
                                                                "31.025254,29.615265", //0 Sim1
                                                                "31.019940,29.613438", //1 Sim2
                                                                "31.019459,29.600833", //2 Sim4
                                                                "31.019779,29.611176", //3 Sim5
                                                                "31.016278,29.603858", //4 Sim9
																"31.013271,29.603257", //5 ESP
																"31.023188,29.610756"  //6 Extra!
                                                              };
        private static readonly List<string> UpdatedLocs = 
                                             new List<string>(){
                                                                "31.021174,29.595426", // 0 Sim4`
                                                                "31.020964,29.607270"  // 1 Sim5`
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
                if (_messageId > 2)
                {
                    //_alert = true;
                    //_state = "Drowning";
                    _loc = UpdatedLocs[1];
                }
                else
                {
                    _loc = OriginalLocs[3];
                }
                var telemetryDataPoint = new
                {
                    messageId = _messageId++,
                    deviceId = DbDeviceId,
                    location = _loc,
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
