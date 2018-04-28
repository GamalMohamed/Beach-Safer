// Physical device information for board and sensor
#define DEVICE_ID "777"
#define DB_DEVICE_ID "10"

// Pin layout
#define LED_PIN 2
#define VIBRATION_MOTOR_PIN 4
#define GPS_RX_PIN 13
#define GPS_TX_PIN 12
#define RF_RX_PIN 5
#define RF_TX_PIN 2
#define RF_PTT_PIN -1

// Interval time(ms) for sending message to IoT Hub
#define SENDING_INTERVAL 15000
#define MESSAGE_MAX_LEN 256
#define GPS_LOC_ACC 25
#define GPS_BAUD 9600
#define GPS_DEFAULT_LOC "31.013271,29.603257"

#define NORMAL_STATE "OK"

// SSID and SSID password's length should be < 32 bytes
#define SSID_LEN 32
#define PASS_LEN 32
#define CONNECTION_STRING_LEN 256

static char *ssid = "Jimmy2";
static char *pass = "12345678900";
static char *connectionString = "HostName=esp-IoTHub.azure-devices.net;DeviceId=777;SharedAccessKey=5qrBA6/Yd69tdMlZFUb70IpRNpPJFEPDBKfKYB99fa8=";

static unsigned int messageCount = 1;

static inline void SmartDelay(int delayPeriod)
{
    uint32_t sstart = millis();
    while (millis() - sstart <= delayPeriod)
    {
        delay(0); // HACK: To overcome esp watch dog timeout
    }
}

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

void initVibrationMotor()
{
    pinMode(VIBRATION_MOTOR_PIN, OUTPUT);
}
