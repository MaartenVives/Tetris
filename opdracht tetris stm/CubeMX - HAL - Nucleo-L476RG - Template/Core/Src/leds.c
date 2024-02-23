#include "leds.h"
#include "main.h"

// Functie om de data van één byte op de 8 LED's te plaatsen.
void ByteToLeds(unsigned char data)
{	
	if(data & 0x01)
		HAL_GPIO_WritePin(LED1_GPIO_Port, LED1_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(LED1_GPIO_Port, LED1_Pin, GPIO_PIN_RESET);

	if(data & 0x02)
		HAL_GPIO_WritePin(LED2_GPIO_Port, LED2_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(LED2_GPIO_Port, LED2_Pin, GPIO_PIN_RESET);

	if(data & 0x04)
		HAL_GPIO_WritePin(LED3_GPIO_Port, LED3_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(LED3_GPIO_Port, LED3_Pin, GPIO_PIN_RESET);

	if(data & 0x08)
		HAL_GPIO_WritePin(LED4_GPIO_Port, LED4_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(LED4_GPIO_Port, LED4_Pin, GPIO_PIN_RESET);

	if(data & 0x10)
		HAL_GPIO_WritePin(LED5_GPIO_Port, LED5_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(LED5_GPIO_Port, LED5_Pin, GPIO_PIN_RESET);

	if(data & 0x20)
		HAL_GPIO_WritePin(LED6_GPIO_Port, LED6_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(LED6_GPIO_Port, LED6_Pin, GPIO_PIN_RESET);

	if(data & 0x40)
		HAL_GPIO_WritePin(LED7_GPIO_Port, LED7_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(LED7_GPIO_Port, LED7_Pin, GPIO_PIN_RESET);

	if(data & 0x80)
		HAL_GPIO_WritePin(LED8_GPIO_Port, LED8_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(LED8_GPIO_Port, LED8_Pin, GPIO_PIN_RESET);
}


void ToggleLed(uint8_t ledNumber)
{
	switch(ledNumber)
	{
		case 0:
			HAL_GPIO_TogglePin(UserLED_GPIO_Port, UserLED_Pin);
			break;
		
		case 1:
			HAL_GPIO_TogglePin(LED1_GPIO_Port, LED1_Pin);
			break;
		
		case 2:
			HAL_GPIO_TogglePin(LED2_GPIO_Port, LED2_Pin);
			break;
		
		case 3:
			HAL_GPIO_TogglePin(LED3_GPIO_Port, LED3_Pin);
			break;
		
		case 4:
			HAL_GPIO_TogglePin(LED4_GPIO_Port, LED4_Pin);
			break;
		
		case 5:
			HAL_GPIO_TogglePin(LED5_GPIO_Port, LED5_Pin);
			break;
		
		case 6:
			HAL_GPIO_TogglePin(LED6_GPIO_Port, LED6_Pin);
			break;
		
		case 7:
			HAL_GPIO_TogglePin(LED7_GPIO_Port, LED7_Pin);
			break;
		
		case 8:
			HAL_GPIO_TogglePin(LED8_GPIO_Port, LED8_Pin);
			break;
		
		default:
			break;
	}
}

void ByteToLevel(uint8_t level)
{
    if(level<16)
        ByteToLeds(0);
    else if(level < 48)
        ByteToLeds(1);
    else if(level < 80)
        ByteToLeds(3);
    else if(level < 112)
        ByteToLeds(7);
    else if(level < 144)
        ByteToLeds(15);
    else if(level < 176)
        ByteToLeds(0x1F);
    else if(level < 208)
        ByteToLeds(0x3F);
    else if(level < 240)
        ByteToLeds(0x7F);
    else
        ByteToLeds(255);
}

void SetUserLed(bool onOff)
{
	if(onOff)
		HAL_GPIO_WritePin(UserLED_GPIO_Port, UserLED_Pin, GPIO_PIN_SET);
	else
		HAL_GPIO_WritePin(UserLED_GPIO_Port, UserLED_Pin, GPIO_PIN_RESET);
}
