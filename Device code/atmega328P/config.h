// Physical device information for board and sensor
#define DEVICE_ID "777"

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
#define RF_PTT_PIN -1 
#define SERVO_PIN 12

#define NORMAL_STATE "OK"
#define ALERT_STATE_L1 "SOS1"
#define ALERT_STATE_L2 "SOS2"
#define DROWNING_STATE "Drowning"
#define DISTRESS_STATE "Distress"


static inline void SmartDelay(int delayPeriod)
{
  uint32_t sstart = millis();
  while (millis() - sstart <= delayPeriod)
  {
  }
}
