#include <ArduinoJson.h>

static bool rf_signal_recevied = false;
static char* status;
static char* location;
char *readRF_status()
{
    uint8_t buf[RH_ASK_MAX_MESSAGE_LEN];
    uint8_t buflen = sizeof(buf);

    if (RF_driver.recv(buf, &buflen))
    {
        char *temp = (char *)malloc(buflen + 1);
        for (int i = 0; i < buflen; i++)
        {
            temp[i] = (char)(buf[i]);
        }
        temp[buflen] = '\0';
        rf_signal_recevied = true;
        return ((char *)temp);
    }

    return "OK";
}

char *readGPS_location()
{
    // TODO
    return "31.112666,29.653975";
}

void readMessage(int messageId, char *payload)
{
    location = readGPS_location();
    if (!rf_signal_recevied)
    {
        status = readRF_status();
    }
    StaticJsonBuffer<MESSAGE_MAX_LEN> jsonBuffer;
    JsonObject &root = jsonBuffer.createObject();
    root["deviceId"] = DEVICE_ID;
    root["messageId"] = messageId;

    if (status == NULL)
    {
        root["status"] = "NAN";
    }
    else
    {
        root["status"] = status;
    }

    if (location == NULL)
    {
        root["location"] = "NAN";
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
        sendingInterval = root["desired"]["interval"];
    }
    else if (root.containsKey("interval"))
    {
        sendingInterval = root["interval"];
    }
}