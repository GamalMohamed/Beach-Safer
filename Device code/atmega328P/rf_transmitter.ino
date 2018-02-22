#include <RH_ASK.h>
#include <SPI.h>

#define RF_RX_PIN 4
#define RF_TX_PIN 8
#define RF_PTT_PIN 5 


RH_ASK driver(2000, RF_RX_PIN, RF_TX_PIN, RF_PTT_PIN);

void RF_init()
{
  if (!driver.init())
  {
    Serial.println("init failed");
  }
}

void RF_transmit(char *msg)
{
  driver.send((uint8_t *)msg, strlen(msg));
  driver.waitPacketSent();
  SmartDelay(200);
}