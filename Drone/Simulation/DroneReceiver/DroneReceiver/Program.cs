using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DroneReceiver
{
    class Program
    {
        //private static string PythonExe = "C:/Users/JIMMY/AppData/Local/Programs/Python/Python35-32/python.exe";
        private static string _pythonExe = "";
        private const string ConnectionString = "HostName=esp-IoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=lzL5hnUvx/Ta3fnnJDSq4/hG9eXtB/Nx+cKTsH0n7v8=";
        private const string IotHubD2CEndpoint = "messages/events";
        private static EventHubClient _eventHubClient;
        private static bool _droneCalled = false;

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken cancellationToken)
        {
            var eventHubReceiver = _eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                var eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null)
                    continue;

                var data = Encoding.UTF8.GetString(eventData.GetBytes());
                Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

                if (values["state"] == "Drowning" && !_droneCalled)
                {
                    //var progToRun = "drone.py";
                    //char[] spliter = { '\r' };

                    //var proc = new Process
                    //{
                    //    StartInfo =
                    //    {
                    //        FileName = _pythonExe,
                    //        RedirectStandardOutput = true,
                    //        UseShellExecute = false,
                    //        Arguments = progToRun
                    //    }
                    //};

                    //proc.Start();

                    //var sReader = proc.StandardOutput;
                    //var output = sReader.ReadToEnd().Split(spliter);

                    //foreach (var s in output)
                    //    Console.WriteLine(s);

                    _droneCalled = true;
                    //proc.WaitForExit();
                    Process.Start("drone.exe");
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Receiving messages...\n");
            //_pythonExe = args[0];
            _eventHubClient = EventHubClient.CreateFromConnectionString(ConnectionString, IotHubD2CEndpoint);
            var partitions = _eventHubClient.GetRuntimeInformation().PartitionIds;
            var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting...");
            };

            var tasks = partitions.Select(partition => ReceiveMessagesFromDeviceAsync(partition, cts.Token));
            Task.WaitAll(tasks.ToArray());
        }
    }
}
