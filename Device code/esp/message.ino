#include <ArduinoJson.h>

static bool rf_signal_recevied = false;
static char *state;
static char *location;
char *readRF()
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

char *readGPS()
{
    char parsedloc[GPS_LOC_ACC];
    double latitude, longitude;
    latitude = longitude = 0.0f;
    if (getGPSCoordinates(latitude, longitude))
    {
        sprintf(parsedloc, "%f,%f", latitude, longitude);
        return parsedloc;
    }
    
    return "BULLSHIT LOCATION!!";
    //return "31.093961,29.698672";
}

char *readMessage(int messageId, char *payload)
{
    char *alert = "NA";
    location = readGPS();
    if (!rf_signal_recevied)
    {
        state = readRF();
    }
    StaticJsonBuffer<MESSAGE_MAX_LEN> jsonBuffer;
    JsonObject &root = jsonBuffer.createObject();
    root["deviceId"] = DEVICE_ID;
    root["messageId"] = messageId;

    if (state == NULL)
    {
        root["state"] = "NAN";
    }
    else
    {
        root["state"] = state;
        if (state != "OK")
        {
            alert = state;
        }
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
    return alert;
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