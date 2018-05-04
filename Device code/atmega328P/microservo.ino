#include <ServoTimer2.h>

ServoTimer2 servo;
void Servo_init()
{
  servo.attach(SERVO_PIN);
  servo.write(1200);
  SmartDelay(1000);
}

void Servo_Operate()
{
  servo.write(800);
  SmartDelay(1000);
  servo.write(2500);
  SmartDelay(1000);
  servo.detach();
  Serial.println("DDDDDDDDDDD");
}
