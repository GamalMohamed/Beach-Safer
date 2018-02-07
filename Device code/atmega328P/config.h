// Pin Layout configuration
#define RF_RX_PIN 4
#define RF_TX_PIN 8
#define RF_PTT_PIN 5 
#define PUSH_BUTTON_PIN 7

static RH_ASK driver(2000, RF_RX_PIN, RF_TX_PIN, RF_PTT_PIN);

void RF_init()
{
  if (!driver.init())
  {
    Serial.println("init failed");
  }
}

void MAX30100_init()
{
    //TODO
}