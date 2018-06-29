#include <ArduinoJson.h>

bool RFMsgReceived = false;
char *state;
int sendingInterval = SENDING_INTERVAL;
bool useGPSReading = false;

inline void setState(char *rf_state)
{
    state = rf_state;
    RFMsgReceived = true;
}

inline void AlterGPSLocation()
{
    useGPSReading = !useGPSReading;
}

inline bool isRFMsgReceived()
{
    return RFMsgReceived;
}

bool isItForMe(char *rfMsg, char **myMsg)
{
    char *temp = (char *)malloc(strlen(rfMsg) + 1);
    strcpy(temp, rfMsg);
    char *dId = strtok(temp, " ");
    *myMsg = strtok(NULL, " ");
    return (strcmp(dId, DEVICE_ID) == 0) ? true : false;
}

char *checkRF()
{
    char *rfMsg = getRFReading();
    if (rfMsg != "")
    {
        char *myMsg = "";
        if (isItForMe(rfMsg, &myMsg))
        {
            return myMsg;
        }
    }
    return NORMAL_STATE;
}

void readGPS(char *location)
{
    char latitude[10], longitude[10];

    if (!useGPSReading)
    {
        sprintf(location, "%s", GPS_DEFAULT_LOC);
    }
    else
    {
        if (getGPSCoordinates(latitude, longitude))
        {
            sprintf(location, "%s,%s", latitude, longitude);
        }
    }
}

// Message format : 
//{ "messageId": " ", "state": " ", "location": " ", "deviceId": , "customerId": , "beachId": ,"deviceUserId": , "droneId": " " }
char *readMessage(int messageId, char *payload)
{
    char *alert = "NA";
    char location[GPS_LOC_ACC];
    readGPS(location);
    if (!RFMsgReceived)
    {
        state = checkRF();
    }
    StaticJsonBuffer<MESSAGE_MAX_LEN> jsonBuffer;
    JsonObject &root = jsonBuffer.createObject();
    root["deviceId"] = DB_DEVICE_ID;
    root["messageId"] = messageId;
    root["customerId"] = CUSTOMER_ID;
    root["beachId"] = BEACH_ID;
    root["deviceUserId"] = DEVICE_USER_ID;
    root["droneId"] = DRONE_ID;

    if (state == NULL)
    {
        root["state"] = "NAN";
    }
    else
    {
        root["state"] = state;
        if (state != NORMAL_STATE)
        {
            RFMsgReceived = true;
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