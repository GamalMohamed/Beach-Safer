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




void PushButtonISR()
{
    // interrupts settings
    sei();                // enable interrupts
    EIMSK &= 0b11111110;   //disable int0 only

    unsigned int pinState;
    unsigned int pushCount = 0;
    unsigned int relCount =0;
   
    while(1)
    {
		pinState = digitalRead(2);
		if(pinState == 0)
		{
			pushCount++;
			if(pushCount > PUSH_INTERVAL)
            break;
		}
		else
			pushCount = 0;
	}

	while(1)
	{
		pinState = digitalRead(2);
		if(pinState == 1)
		{
			relCount++;
			if(relCount > REL_INTERVAL)
            break;
		}
		else
			relCount = 0;
	}

   
	relCount = 0;
	pushCount = 0;
	while(1)
	{
		pinState = digitalRead(2);
		if(pinState == 1)
		{
			relCount++;
			pushCount=0;

			if(relCount > 33000)   // single push detected
			{
				Serial.println("Single push detected");
			  SendRF_message("SOS1");
        lockMax = true;
        
        // enable Interrupt on PIN 2 
        EIFR &= 0b11111110;
			  EIMSK |= 0b00000001;
				return;
			}
		}
		else
		{
			pushCount++;
			relCount=0;

			if(pushCount > PUSH_INTERVAL)  // double push detected
			{ 
        // wait for release
				relCount = 0;
				while(1)
				{
					pinState = digitalRead(2);
					if(pinState == 1)
					{
						relCount++;
						if(relCount > REL_INTERVAL)
						break;
					}
					else
						relCount=0;
				} 
       
        Serial.println("Double push detected");
        // TODO: Operate Servos!
        SendRF_message("SOS2");
        lockMax = true;
        
        // enable Interrupt on PIN 2
				EIFR &= 0b11111110;
				EIMSK |= 0b00000001;
				return;
			}    
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

bool volatile lockMax = false; // HACK: Ignore Max30100 once successfully operating RF
void loop()
{
 
  if (!lockMax)
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
