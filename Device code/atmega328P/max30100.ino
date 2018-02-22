#include "MAX30100_PulseOximeter.h"
#include <Wire.h>

#define REPORTING_INTERVAL_MS 3000
#define SENSOR_TIMEOUT 5000
#define READINGS_NUMBER 10
#define FILTER_WEIGHT 0.5

PulseOximeter pox;
uint32_t tsLastReport = 0;
uint32_t last_beat = 0;
int readIndex = 0;
int average_beat = 0;
int average_SpO2 = 0;
bool calculating = false;
bool initialized = false;

void onBeatDetected()
{
  last_beat = millis();
}

void ReadingProgressBar(int j)
{
  if (!calculating)
  {
    calculating = true;
    initialized = false;
  }
  Serial.print(". ");
}

void DisplayReadings()
{
  Serial.println("");
  Serial.print(average_beat);
  Serial.println(" Bpm");
  Serial.print("SpO2 ");
  Serial.print(average_SpO2);
  Serial.println("%");
}

void InitialDisplay()
{
  if (!initialized)
  {
    Serial.print("Place ur finger or Wrist on the sensor");
    initialized = true;
  }
}

void CalculateReadingsAvg(int beat, int SpO2)
{
  if (readIndex == READINGS_NUMBER)
  {
    calculating = false;
    initialized = false;
    readIndex = 0;
    DisplayReadings();
  }

  if (beat > 30 and beat < 220 and SpO2 > 50)
  {
    average_beat = FILTER_WEIGHT * (beat) + (1 - FILTER_WEIGHT) * average_beat;
    average_SpO2 = FILTER_WEIGHT * (SpO2) + (1 - FILTER_WEIGHT) * average_SpO2;
    readIndex++;
    ReadingProgressBar(readIndex);
  }
}

void MAX30100_init()
{
  //pox.setIRLedCurrent(MAX30100_LED_CURR_7_6MA);
  pox.begin();
  pox.setOnBeatDetectedCallback(onBeatDetected);
  InitialDisplay();
}

void MAX30100_Operate(int &SpO2, int &heartBeat)
{
  pox.update();
  if ((millis() - tsLastReport > REPORTING_INTERVAL_MS))
  {
    for (int i = 0; i < READINGS_NUMBER; i++)
    {
      CalculateReadingsAvg(pox.getHeartRate(), pox.getSpO2());
      pox.update();
      SmartDelay(50);
    }
    tsLastReport = millis();
  }

  if ((millis() - last_beat > SENSOR_TIMEOUT))
  {
    average_beat = 0;
    average_SpO2 = 0;
    InitialDisplay();
  }

  SpO2 = average_SpO2;
  heartBeat = average_beat;
}
