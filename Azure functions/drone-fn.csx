using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;

public static void Run(string myQueueItem, TraceWriter log)
{
    log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
    
    var connectionString = "HostName=BSafer-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=i+dBICYQh0AbtOAmyzANGCBHo4jWnLuD5VeoYmxTjmI=";
    dynamic msg = JsonConvert.DeserializeObject(myQueueItem);
    string DroneId = msg.droneId;
    var methodName = "takeOff";

    var serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
    var methodInvocation = new CloudToDeviceMethod(methodName) { ResponseTimeout = TimeSpan.FromSeconds(30) };
    methodInvocation.SetPayloadJson(myQueueItem);
    var response = serviceClient.InvokeDeviceMethodAsync(DroneId, methodInvocation);
    log.Info($"Response status: {response.Status}");
}