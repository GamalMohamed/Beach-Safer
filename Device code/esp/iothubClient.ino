const char *onSuccess = "\"Successfully invoke device method\"";
const char *notFound = "\"No method found\"";
bool messageSuccess = true;

static void sendCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result, void *userContextCallback)
{
    if (IOTHUB_CLIENT_CONFIRMATION_OK == result)
    {
        Serial.println("Message sent successfully to Azure IoT Hub!");
        blinkLED();
        messageSuccess = true;
    }
    else
    {
        Serial.println("Failed to send message to Azure IoT Hub!!!");
        messageSuccess = false;
    }
    messagePending = false;
}

static bool sendMessage(IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle, char *buffer, const char *alert)
{
    IOTHUB_MESSAGE_HANDLE messageHandle = IoTHubMessage_CreateFromByteArray((const unsigned char *)buffer, strlen(buffer));
    if (messageHandle == NULL)
    {
        Serial.println("Unable to create a new IoTHubMessage.");
        return false;
    }
    else
    {
        MAP_HANDLE propMap = IoTHubMessage_Properties(messageHandle);
        if (Map_AddOrUpdate(propMap, "Alert", alert) != MAP_OK)
        {
            Serial.println("ERROR: Map_AddOrUpdate Failed!!");
            return false;
        }

        Serial.printf("Sending message: %s.\r\n", buffer);
        if (IoTHubClient_LL_SendEventAsync(iotHubClientHandle, messageHandle, sendCallback, NULL) != IOTHUB_CLIENT_OK)
        {
            Serial.println("Failed to hand over the message to IoTHubClient.");
            return false;
        }
        else
        {
            messagePending = true;
            Serial.println("IoTHubClient accepted the message for delivery.");
        }

        IoTHubMessage_Destroy(messageHandle);
    }
    return messageSuccess;
}

void start()
{
    Serial.println("Start sending data...");
    messageSending = true;
}

void stop()
{
    Serial.println("Stop sending data...");
    messageSending = false;
}

void vibrate()
{
    digitalWrite(VIBRATION_MOTOR_PIN, HIGH);
    SmartDelay(3000);
    digitalWrite(VIBRATION_MOTOR_PIN, LOW);
}

IOTHUBMESSAGE_DISPOSITION_RESULT receiveMessageCallback(IOTHUB_MESSAGE_HANDLE message, void *userContextCallback)
{
    IOTHUBMESSAGE_DISPOSITION_RESULT result;
    const unsigned char *buffer;
    size_t size;
    if (IoTHubMessage_GetByteArray(message, &buffer, &size) != IOTHUB_MESSAGE_OK)
    {
        Serial.println("Unable to IoTHubMessage_GetByteArray.");
        result = IOTHUBMESSAGE_REJECTED;
    }
    else
    {
        char *temp = (char *)malloc(size + 1);

        if (temp == NULL)
        {
            return IOTHUBMESSAGE_ABANDONED;
        }

        strncpy(temp, (const char *)buffer, size);
        temp[size] = '\0';
        Serial.printf("Receive C2D message: %s.\r\n", temp);
        free(temp);
        blinkLED();
    }
    return IOTHUBMESSAGE_ACCEPTED;
}

int deviceMethodCallback(const char *methodName, const unsigned char *payload, size_t size,
                         unsigned char **response, size_t *response_size, void *userContextCallback)
{
    Serial.printf("Trying to invoke method %s.\r\n", methodName);
    const char *responseMessage = onSuccess;
    int result = 200;

    if (strcmp(methodName, "start") == 0)
    {
        start();
    }
    else if (strcmp(methodName, "stop") == 0)
    {
        stop();
    }
    else if (strcmp(methodName, "vibrate") == 0)
    {
        vibrate();
    }
    else if (strcmp(methodName, "alterGPSlocation") == 0)
    {
        AlterGPSLocation();
    }
    else
    {
        Serial.printf("No method %s found.\r\n", methodName);
        responseMessage = notFound;
        result = 404;
    }

    *response_size = strlen(responseMessage);
    *response = (unsigned char *)malloc(*response_size);
    strncpy((char *)(*response), responseMessage, *response_size);

    return result;
}

void twinCallback(DEVICE_TWIN_UPDATE_STATE updateState, const unsigned char *payLoad, size_t size,
                  void *userContextCallback)
{
    char *temp = (char *)malloc(size + 1);
    for (int i = 0; i < size; i++)
    {
        temp[i] = (char)(payLoad[i]);
    }
    temp[size] = '\0';
    parseTwinMessage(temp);
    free(temp);
}