#include "config.h"

char *MonitorHealth()
{
  // TODO [Optional]: More optimzing better to be added
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

//TODO: Convert reading Button press event to ISRs instead of normal functions polling
inline bool IsButtonPressedOnce()
{
  // TODO: In case of using internal Pull-up resistor, change it to LOW instead
  return (digitalRead(PUSH_BUTTON_PIN) == HIGH ? true : false);
}

inline bool IsButtonPressedTwice()
{
  // TODO: Check here if button pressed twice
  return false;
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

bool lockMax = false; // HACK: Ignore Max30100 once successfully operating RF
void loop()
{
  if (IsButtonPressedTwice())
  {
    // TODO: Operate Servos!
    SendRF_message("SOS2");
    lockMax = true;
  }
  else if (IsButtonPressedOnce())
  {
    SendRF_message("SOS1");
    lockMax = true;
  }
  else if (!lockMax)
  {
    char *state = MonitorHealth();
    if (state != "OK")
    {
      // TODO: Operate Servos!
      SendRF_message(state);
      lockMax = true;
    }
  }
}
