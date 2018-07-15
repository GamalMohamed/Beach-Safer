#r "Microsoft.Azure.NotificationHubs"
#r "Newtonsoft.Json"

using System;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;

public static async Task Run(string myQueueItem, NotificationHubClient notifications, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}"); 

    log.Info($"Sending notification");    
    dynamic msg = JsonConvert.DeserializeObject(myQueueItem);
    string gcmNotification = "{\"data\": {\"message\": {" +
                                        "MessageId:"+msg.messageId + ',' +
                                        "DeviceId:"+msg.deviceId + ','+
                                        "Location:\""+msg.location + "\","+
                                        "State:\""+msg.state+"\""+ 
                                        "}}}";
    log.Info($"{gcmNotification}");
    await notifications.SendGcmNativeNotificationAsync(gcmNotification, ""+msg.beachId);      
}