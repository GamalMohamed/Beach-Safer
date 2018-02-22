// Physical device information for board and sensor
#define DEVICE_ID "ESP12F"

#define MAX_RF_MSG_LENGTH 80
#define RF_MSG_SENDING_PERIOD 30000

// Pin Layout configuration
#define PUSH_BUTTON_PIN 7
#define RF_RX_PIN 4
#define RF_TX_PIN 8
#define RF_PTT_PIN 5 
//#define SERVO1_PIN ??
//#define SERVO2_PIN ??

static inline void SmartDelay(int delayPeriod)
{
  uint32_t sstart = millis();
  while (millis() - sstart <= delayPeriod)
  {
  }
}
