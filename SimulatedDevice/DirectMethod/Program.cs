using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace DirectMethod
{
    class Program
    {
        private static ServiceClient _serviceClient;
        private static string connectionString =
            "HostName=esp-IoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=lzL5hnUvx/Ta3fnnJDSq4/hG9eXtB/Nx+cKTsH0n7v8=";

        private static async Task InvokeMethod()
        {
            var methodInvocation = new CloudToDeviceMethod("vibrate") { ResponseTimeout = TimeSpan.FromSeconds(30) };

            var response = await _serviceClient.InvokeDeviceMethodAsync("ESP12F", methodInvocation);

            Console.WriteLine("Response status: {0}", response.Status);
        }

        static void Main(string[] args)
        {
            _serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            while (true)
            {
                Console.Write("Write method to be invoked: ");
                var userinput = Console.ReadLine();
                if (userinput == "vib")
                {
                    InvokeMethod().Wait();
                }
                else if (userinput=="quit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Unknown command..Try again");
                }
            }
        }
    }
}
