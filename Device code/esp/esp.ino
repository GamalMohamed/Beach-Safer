#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>
#include <WiFiUdp.h>
#include <AzureIoTHub.h>
#include <AzureIoTProtocol_MQTT.h>
#include <AzureIoTUtility.h>
#include <RH_ASK.h>
#include <SPI.h> 
#include "config.h"

static bool messagePending = false;
static bool messageSending = true;
static unsigned int messageCount = 1;
static int sendingInterval = INTERVAL;
static IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle;

void blinkLED()
{
    digitalWrite(LED_PIN, LOW);
    delay(500);
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
        while (1);
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
    if (!messagePending && messageSending)
    {
        char messagePayload[MESSAGE_MAX_LEN];
        readMessage(messageCount, messagePayload);
        sendMessage(iotHubClientHandle, messagePayload);
        messageCount++;
        delay(sendingInterval);
    }
    IoTHubClient_LL_DoWork(iotHubClientHandle);
    delay(10);
}