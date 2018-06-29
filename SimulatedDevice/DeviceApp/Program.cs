using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimulatedDevice
{
    class Program
    {
        private const string IotHubUri = "BSafer-iothub.azure-devices.net";
        private const string DeviceKey = "Qq2948ipTbOftwNyCCzwG6SJ2UEXmfi/Y8dfNlPN1gg=";
        private const string DeviceId = "Sim3";
        private const int DbDeviceId = 1;
        private const int CustomerId = 1;
        private const int BeachId = 1;
        private const int DeviceUserId = 1;
        private const string DroneId = "Sim-Drone";
        private static string _state = "OK";
        private static int _messageId = 1;
        private static string _loc;
        private static bool _alert = false;
        private static DeviceClient _deviceClient;
        private static readonly List<string> OriginalLocs =
                                             new List<string>(){
                                                                "31.025254,29.615265", //0 Sim1
                                                                "31.019940,29.613438", //1 Sim2
                                                                "31.019459,29.600833", //2 Sim3
                                                                "31.019779,29.611176", //3 Sim4
                                                                "31.016278,29.603858", //4 Sim5
                                                                "31.023188,29.610756", //5 Sim6
                                                                "31.02153, 29.60566",  //6 Sim7
                                                                "31.01803, 29.60854",  //7 Sim8
                                                                "31.01619, 29.60768",  //8 Sim9
                                                                "31.02406, 29.60497",  //9 Sim10
																"31.013271,29.603257", //10 ESP
                                                              };
        private static readonly List<string> UpdatedLocs = 
                                             new List<string>(){
                                                                "31.021174,29.595426", // 0 Sim6`
                                                                "31.020964,29.607270"  // 1 Sim4`
                                                                };


        private static Task<MethodResponse> Vibrate(MethodRequest methodRequest, object userContext)
        {
            //Console.WriteLine("\nDrone Taking Off!!");
            Console.WriteLine("\nVIBRATING!!");
            Console.WriteLine(Encoding.UTF8.GetString(methodRequest.Data));
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes("'Function invoked successfully!'"), 200));
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            while (true)
            {
                if (_messageId > 2)
                {
                    _alert = true;
                    _state = "Drowning";
                    //_state = "SOS2";
                    //_state = "SOS1";
                    //_loc = UpdatedLocs[0];
                }
                else
                {
                    _loc = OriginalLocs[2];
                }
                //_loc = OriginalLocs[2];
                var telemetryDataPoint = new
                {
                    messageId = _messageId++,
                    deviceId = DbDeviceId,
                    location = _loc,
                    state = _state,
                    customerId = CustomerId,
                    beachId = BeachId,
                    deviceUserId = DeviceUserId,
                    droneId = DroneId
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

                await Task.Delay(5000);
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine(DeviceId + " starting..\n");
            _deviceClient = DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, DeviceKey), TransportType.Mqtt);
            //_deviceClient.SetMethodHandlerAsync("takeOff", TakeOff, null).Wait();
            _deviceClient.SetMethodHandlerAsync("vibrate", Vibrate, null).Wait();
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}
