#include "config.h"

char *MonitorHealth()
{
  int SpO2, heartBeat;
  MAX30100_Operate(SpO2, heartBeat);
  if (SpO2 > 0 && SpO2 < 90)
  {
    return "Drowning";
  }
  if (heartBeat > 0 && heartBeat <= 25)
  {
    return "Distress";
  }

  return "OK";
}

inline bool IsButtonPressedOnce()
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

void SendRF_message(char *msg)
{
  char rf_msg[MAX_RF_MSG_LENGTH];
  sprintf(rf_msg, "%s %s", DEVICE_ID, msg);
  uint32_t sstart = millis();
  while (millis() - sstart <= RF_MSG_SENDING_PERIOD)
  {
    RF_transmit(rf_msg);
    Serial.println(rf_msg);
  }
}

void setup()
{
  Serial.begin(115200);

  pinMode(PUSH_BUTTON_PIN, INPUT);

  MAX30100_init();
  RF_init();
}

void loop()
{
  if (IsButtonPressedOnce())
  {
    SendRF_message("SOS1");
  }

  char *state = MonitorHealth();
  if (state != "OK")
  {
    SendRF_message(state);
  }
}
