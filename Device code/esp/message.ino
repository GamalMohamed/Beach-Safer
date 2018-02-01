#include <ArduinoJson.h>

char* readRF_status()
{
    // TODO
    return "Dummy status";
}

char* readGPS_location()
{
    // TODO
    return "Dummy location";
}

void readMessage(int messageId, char *payload)
{
    char* status = readRF_status();
    char* location = readGPS_location();
    StaticJsonBuffer<MESSAGE_MAX_LEN> jsonBuffer;
    JsonObject &root = jsonBuffer.createObject();
    root["deviceId"] = DEVICE_ID;
    root["messageId"] = messageId;

    // NAN is not the valid json, change it to NULL
    if (status==NULL)
    {
        root["status"] = NULL;
    }
    else
    {
        root["status"] = status;
    }

    if (location==NULL)
    {
        root["location"] = NULL;
    }
    else
    {
        root["location"] = location;
    }

    root.printTo(payload, MESSAGE_MAX_LEN);
}

void parseTwinMessage(char *message)
{
    StaticJsonBuffer<MESSAGE_MAX_LEN> jsonBuffer;
    JsonObject &root = jsonBuffer.parseObject(message);
    if (!root.success())
    {
        Serial.printf("Parse %s failed.\r\n", message);
        return;
    }

    if (root["desired"]["interval"].success())
    {
        interval = root["desired"]["interval"];
    }
    else if (root.containsKey("interval"))
    {
        interval = root["interval"];
    }
}