#include <TinyGPS++.h>
#include <SoftwareSerial.h>

TinyGPSPlus gps;
SoftwareSerial gps_serial(GPS_RX_PIN, GPS_TX_PIN);

void initGPS()
{
    gps_serial.begin(GPS_BAUD);
}

bool getGPSCoordinates(char *latitude, char *longitude)
{
    GPS_smartDelay(500);
    if (millis() > 5000 && gps.charsProcessed() < 10)
    {
        Serial.println(F("No GPS data received: check wiring"));
        return false;
    }

    sprintf(latitude, "%f", gps.location.lat());
    sprintf(longitude, "%f", gps.location.lng());
    return true;
}

void GPS_smartDelay(unsigned long ms)
{
    unsigned long start = millis();
    do
    {
        while (gps_serial.available())
        {
            gps.encode(gps_serial.read());
        }
    } while (millis() - start < ms);
}