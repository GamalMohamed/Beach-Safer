#include <TinyGPS++.h>
#include <SoftwareSerial.h>

TinyGPSPlus gps;
SoftwareSerial gps_serial(GPS_RX_PIN, GPS_TX_PIN);

void initGPS()
{
    gps_serial.begin(GPS_BAUD);
}

bool getGPSCoordinates(double &latitude, double &longitude)
{
    GPS_smartDelay(500);
    if (millis() > 5000 && gps.charsProcessed() < 10)
    {
        Serial.println(F("No GPS data received: check wiring"));
        return false;
    }

    latitude = gps.location.lat();
    longitude = gps.location.lng();
    Serial.print("Latitude: ");
    Serial.println(latitude, 5);
    Serial.print("Longitude: ");
    Serial.println(longitude, 5);

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