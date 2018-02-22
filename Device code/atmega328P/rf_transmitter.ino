#include <RH_ASK.h>
#include <SPI.h>

RH_ASK RF_driver(2000, RF_RX_PIN, RF_TX_PIN, RF_PTT_PIN);

void RF_init()
{
  if (!RF_driver.init())
  {
    Serial.println("init failed");
  }
}

void RF_transmit(char *msg)
{
  RF_driver.send((uint8_t *)msg, strlen(msg));
  RF_driver.waitPacketSent();
  SmartDelay(200);
}