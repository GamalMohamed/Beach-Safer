// Physical device information for board and sensor
#define DEVICE_ID "ESP12F"

// Pin layout
#define LED_PIN 2
#define VIBRATION_MOTOR_PIN 13
#define GPS_TX_PIN 14
#define GPS_RX_PIN 16
#define RF_RX_PIN 4
#define RF_TX_PIN 13
#define RF_PTT_PIN 5 

// Interval time(ms) for sending message to IoT Hub
#define INTERVAL 1000
#define MESSAGE_MAX_LEN 256

// SSID and SSID password's length should be < 32 bytes
#define SSID_LEN 32
#define PASS_LEN 32
#define CONNECTION_STRING_LEN 256


static char *ssid="Gamal";
static char *pass="12131415";
static char *connectionString="HostName=esp-IoTHub.azure-devices.net;DeviceId=ESP12F;SharedAccessKey=1YTcS/KjcfrrdYgQhybnl9t8HZZi8HepkSpxH+Kp8Es=";

static RH_ASK RF_driver(2000, RF_RX_PIN, RF_TX_PIN, RF_PTT_PIN);


// For debugging purposes only
void initSerial()
{
    Serial.begin(115200);
    Serial.setDebugOutput(true);
    Serial.println("Serial successfully initialized!");
}

void initWifi()
{
    Serial.printf("Attempting to connect to Wifi network: %s.\r\n", ssid);
    WiFi.begin(ssid, pass);
    while (WiFi.status() != WL_CONNECTED)
    {
        uint8_t mac[6];
        WiFi.macAddress(mac);
        Serial.printf("You device with MAC address %02x:%02x:%02x:%02x:%02x:%02x connects to %s failed! Waiting 10 seconds to retry.\r\n",
                mac[0], mac[1], mac[2], mac[3], mac[4], mac[5], ssid);
        WiFi.begin(ssid, pass);
        delay(10000);
    }
    Serial.printf("Succesfully connected to wifi %s!\r\n", ssid);
}

void initTime()
{
    time_t epochTime;
    configTime(0, 0, "pool.ntp.org", "time.nist.gov");

    while (true)
    {
        epochTime = time(NULL);

        if (epochTime == 0)
        {
            Serial.println("Fetching NTP epoch time failed! Waiting 2 seconds to retry...");
            delay(2000);
        }
        else
        {
            Serial.printf("Fetched NTP epoch time is: %lu.\r\n", epochTime);
            break;
        }
    }
}

void initRF()
{
    if(!RF_driver.init())
    {
        Serial.println("Failed to intialize RF receiver!!");
    }
}

void initGPS()
{
    //TODO
}

void initVibrationMotor()
{
    pinMode(VIBRATION_MOTOR_PIN, OUTPUT);
}