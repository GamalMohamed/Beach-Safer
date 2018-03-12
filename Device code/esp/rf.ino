#include <RH_ASK.h>
#include <SPI.h>

RH_ASK RF_driver(2000, RF_RX_PIN, RF_TX_PIN, RF_PTT_PIN);

void initRF()
{
    if (!RF_driver.init())
    {
        Serial.println("Failed to intialize RF receiver!!");
    }
}

char* getRFReading()
{
    uint8_t buf[RH_ASK_MAX_MESSAGE_LEN];
    uint8_t buflen = sizeof(buf);

    if (RF_driver.recv(buf, &buflen))
    {
        char *temp = (char *)malloc(buflen + 1);
        for (int i = 0; i < buflen; i++)
        {
            temp[i] = (char)(buf[i]);
        }
        temp[buflen] = '\0';
        return ((char *)temp);
    }
    return "";
}