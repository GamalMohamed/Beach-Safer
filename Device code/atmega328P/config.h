// Physical device information for board and sensor
#define DEVICE_ID "ESP12F"

#define MAX_RF_MSG_LENGTH 80
#define RF_MSG_SENDING_PERIOD 30000

// Pin Layout configuration
#define PUSH_BUTTON_PIN 7

static inline void SmartDelay(int delayPeriod)
{
  uint32_t sstart = millis();
  while (millis() - sstart <= delayPeriod)
  {
  }
}
