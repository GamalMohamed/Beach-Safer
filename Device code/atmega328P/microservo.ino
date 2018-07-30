#include <ServoTimer2.h>

ServoTimer2 servo;
void Servo_init()
{
  servo.attach(SERVO_PIN);
  servo.write(1300);
  SmartDelay(1000);
}

void Servo_Operate()
{ 
  servo.write(0);
  SmartDelay(800);
  servo.write(2200);
  SmartDelay(1000);
  servo.detach();
  Serial.println("DDDDDDDDDDD");
}

void Servo_reset()
{
   servo.attach(SERVO_PIN);
   servo.write(1200);
   SmartDelay(1000);
   servo.detach();
}
