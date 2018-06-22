#include "config.h"

bool volatile lockMax = false; // HACK: Ignore Max30100 once successfully operating RF
bool volatile double_pressed = false;
int dangerCounter_SpO2 = 0;
int dangerCounter_HeartRate = 0;

unsigned int volatile pushCount = 0; 
unsigned long long volatile relCount = 0;

char *MonitorHealth()
{
  int SpO2, heartBeat;
  MAX30100_Operate(SpO2, heartBeat);
  if (SpO2 > 0 && SpO2 < 90)
  {
    dangerCounter_SpO2++;
    if(dangerCounter_SpO2 >= 3)
    {
      return DROWNING_STATE;
    }
  }
  if (heartBeat > 0 && heartBeat < 30 )
  {
    dangerCounter_HeartRate++;
    if(dangerCounter_HeartRate >= 3)
    {
      return DISTRESS_STATE;
    }
  }

  return NORMAL_STATE;
}

void track_push()
{
  while(1){
    if(digitalRead(PUSH_BUTTON_PIN) == LOW) // pressed
      pushCount++;
    else
      pushCount = 0;
    if(pushCount >= PUSH_INTERVAL)
    {
      pushCount = 0;
      return;
    }
  }
}

void track_release()
{
  while(1){
    if(digitalRead(PUSH_BUTTON_PIN) == HIGH) // released
      relCount++;
    else
      relCount = 0;
    if(relCount >= REL_INTERVAL)
    {
      relCount = 0;
      return;
    } 
  }
}

bool check_new_push()
{
  while(1)
  {
    if(digitalRead(PUSH_BUTTON_PIN) == LOW) // pressed
    {
      pushCount++;
      relCount = 0;
    }
    else
    {
      relCount++;
      pushCount = 0;
    }
    
    if(relCount >= INTER_PUSHES_INTERVAL) // new push didn't occur
    {
      relCount = 0;
      pushCount = 0;
      return false;
    }
    else if (pushCount >= PUSH_INTERVAL)  // new push detected
    {
      relCount = 0;
      pushCount = 0;
      return true;
    }  
  }
}


void PushButtonISR()
{
  sei();               // enable interrupts
  EIMSK &= 0b11111110; //disable int0 only
  
  // tracking first push
  track_push();
  track_release();
  
  if(check_new_push() == false) // single push detected
  {
    // single push logic
    Serial.println("single push");
    SendRF_message(ALERT_STATE_L1);
    lockMax = true;
    // end
    // enable Interrupt on PIN 2
    EIFR &= 0b11111110;
    EIMSK |= 0b00000001;
    return;
  }
  track_release();
  
  if(check_new_push() == false)  // double push detected
  {
    // double push logic
    if (!double_pressed)
    {
      Serial.println("Double push detected");
      Servo_init();
      Servo_Operate();
            
      SendRF_message(ALERT_STATE_L2);
      lockMax = true;
      double_pressed = true;
    }
    // end
    // enable Interrupt on PIN 2
    EIFR &= 0b11111110;
    EIMSK |= 0b00000001;
    return;
  }
  track_release();
  
  // triple push logic
  Serial.println("triple push detected");
  Servo_init();
  Servo_Operate();
  // end
  // enable Interrupt on PIN 2
  EIFR &= 0b11111110;
  EIMSK |= 0b00000001;
  return;
    
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

  // push button initialization
  pinMode(PUSH_BUTTON_PIN, INPUT_PULLUP);
  sei();
  attachInterrupt(digitalPinToInterrupt(2), PushButtonISR, FALLING);

  MAX30100_init();
  RF_init();
}

void loop()
{
  if (!lockMax)
  {
    char *state = MonitorHealth();
    if (state != NORMAL_STATE)
    {
      Servo_init();
      Servo_Operate();
      
      SendRF_message(state);
      lockMax = true;
    }
  }
}
