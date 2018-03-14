#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>
#include <WiFiUdp.h>
#include <AzureIoTHub.h>
#include <AzureIoTProtocol_MQTT.h>
#include <AzureIoTUtility.h>
#include "config.h"

bool messagePending = false;
bool messageSending = true;
bool alertSending = false;
unsigned long lastReport = 0;
unsigned int messageCount = 1;
IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle;


void blinkLED()
{
    digitalWrite(LED_PIN, LOW);
    SmartDelay(500);
    digitalWrite(LED_PIN, HIGH);
}

void initAll()
{
    pinMode(LED_PIN, OUTPUT);
    initRF();
    initGPS();
    initVibrationMotor();

    initSerial();
    delay(2000);
    initWifi();
    initTime();
}

void ConfigureIoTHubConnection()
{
    iotHubClientHandle = IoTHubClient_LL_CreateFromConnectionString(connectionString, MQTT_Protocol);
    if (iotHubClientHandle == NULL)
    {
        Serial.println("Failed on IoTHubClient_CreateFromConnectionString.");
        while (1)
            ;
    }

    IoTHubClient_LL_SetOption(iotHubClientHandle, "product_info", "ESP8266-12F");
    IoTHubClient_LL_SetMessageCallback(iotHubClientHandle, receiveMessageCallback, NULL);
    IoTHubClient_LL_SetDeviceMethodCallback(iotHubClientHandle, deviceMethodCallback, NULL);
    IoTHubClient_LL_SetDeviceTwinCallback(iotHubClientHandle, twinCallback, NULL);
}

void setup()
{
    initAll();

    ConfigureIoTHubConnection();
}

void loop()
{
    if (!isRFMsgReceived())
    {
        char *staate = checkRF();
        if (staate != NORMAL_STATE)
        {
            setState(staate);
            alertSending = true;
        }
    }

    if ((millis() - lastReport > SENDING_INTERVAL) || alertSending)
    {
        if (!messagePending && messageSending)
        {
            char messagePayload[MESSAGE_MAX_LEN];
            char *alert = readMessage(messageCount, messagePayload);
            sendMessage(iotHubClientHandle, messagePayload, alert);
            messageCount++;
            SmartDelay(1000);
        }
        lastReport = millis();
    }
    IoTHubClient_LL_DoWork(iotHubClientHandle);
    SmartDelay(10);
}
