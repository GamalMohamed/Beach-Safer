// Physical device information for board and sensor
#define DEVICE_ID "ESP12F"

#define MAX_RF_MSG_LENGTH 80
#define RF_MSG_SENDING_PERIOD 30000

//push button constants
#define PUSH_INTERVAL 10000
#define REL_INTERVAL 5000
#define INTER_PUSHES_INTERVAL 33000

// Pin Layout configuration
#define PUSH_BUTTON_PIN 2
#define RF_RX_PIN 4
#define RF_TX_PIN 8
#define RF_PTT_PIN 5 
//#define RELEASE_SERVO_PIN ??
//#define PULL_SERVO2_PIN ??


static inline void SmartDelay(int delayPeriod)
{
  uint32_t sstart = millis();
  while (millis() - sstart <= delayPeriod)
  {
  }
}
