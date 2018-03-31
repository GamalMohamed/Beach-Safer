#include "config.h"

bool volatile lockMax = false; // HACK: Ignore Max30100 once successfully operating RF
bool volatile double_pressed = false;

char *MonitorHealth()
{
  // TODO [Optional]: More optimzing better to be added
  int SpO2, heartBeat;
  MAX30100_Operate(SpO2, heartBeat);
  if (SpO2 > 0 && SpO2 < 90)
  {
    return DROWNING_STATE;
  }
  if (heartBeat > 0 && heartBeat < 25)
  {
    return DISTRESS_STATE;
  }

  return NORMAL_STATE;
}

void PushButtonISR()
{
  // interrupts settings
  sei();               // enable interrupts
  EIMSK &= 0b11111110; //disable int0 only

  unsigned int state = 1;
  unsigned int pushCount = 0; 
  unsigned long long relCount = 0;
  while(1)
  {
    if(state == 1)        //tracking first push
    {
      if(digitalRead(PUSH_BUTTON_PIN) == LOW) // pressed
        pushCount++;
      else
        pushCount = 0;
      if(pushCount >= PUSH_INTERVAL)
      {
        pushCount = 0;
        state = 2;
      }
    }

    else if(state == 2)   //tracking first release
    {
      if(digitalRead(PUSH_BUTTON_PIN) == HIGH) // released
        relCount++;
      else
        relCount = 0;
      if(relCount >= REL_INTERVAL)
      {
        relCount = 0;
        state = 3;
      }
    }

    else if(state == 3)   // single and double push detection
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

      
      if(relCount >= INTER_PUSHES_INTERVAL) //single push detected
      {
        relCount = 0;
        pushCount = 0;
        state = 5;
      }
      else if (pushCount >= PUSH_INTERVAL)  // double push detected
      {
        relCount = 0;
        pushCount = 0;
        state = 4;
      }
    }
  
    else if(state == 4)    // double push logic
    {
      if(digitalRead(PUSH_BUTTON_PIN) == HIGH)
        relCount++;
      else
        relCount = 0;
    
      if(relCount >= REL_INTERVAL)
      {
        relCount = 0;
        state = 6;
      
        // double push logic here
        if (!double_pressed)
        {
          Serial.println("Double push detected");
          Servo_init();
          Servo_Operate();
            
          SendRF_message(ALERT_STATE_L2);
          lockMax = true;
          double_pressed = true;
        }
      }
    }
    
    else if(state == 5)   // single push logic
    {
      state = 6;
    
      // single push logic here
     
      Serial.println("single push");
      SendRF_message(ALERT_STATE_L1);
      lockMax = true;
    }
  
    else  // state = 6 , finish state
    {
      // enable Interrupt on PIN 2
      EIFR &= 0b11111110;
      EIMSK |= 0b00000001;
      return;
    }
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
