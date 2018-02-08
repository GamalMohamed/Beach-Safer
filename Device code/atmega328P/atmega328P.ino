#include <RH_ASK.h>
#include <SPI.h>
#include "config.h"

static bool rf_tr = false;
char *MonitorHealth()
{
  return "OK";
}

void RF_transmit(char *msg)
{
  //char *msg = "Drowning";

  driver.send((uint8_t *)msg, strlen(msg));
  driver.waitPacketSent();
  delay(200);
}

bool IsButtonPressed()
{
  if (digitalRead(PUSH_BUTTON_PIN) == HIGH)
  {
    return true;
  }
  else
  {
    return false;
  }
}

void setup()
{
  Serial.begin(9600);

  pinMode(PUSH_BUTTON_PIN, INPUT);
  RF_init();
}

void loop()
{
  if (IsButtonPressed() || rf_tr == true)
  {
    RF_transmit("SOS");
    Serial.println("SOS");
    rf_tr = true;
  }
  else{
    Serial.println("OK");
  }
  /*char *status = MonitorHealth();
  if (status != "OK")
  {
    RF_transmit(status);
  }*/
}
