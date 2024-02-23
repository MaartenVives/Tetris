#include "buttons.h"
#include "main.h"

bool SW1Active(void)
{
	// SW1 actief?
	if(HAL_GPIO_ReadPin(SW1_GPIO_Port, SW1_Pin) == GPIO_PIN_RESET)
			return true;
	else
			return false;
}		
	
bool SW2Active(void)
{
	// SW2 actief?
	if(HAL_GPIO_ReadPin(SW2_GPIO_Port, SW2_Pin) == GPIO_PIN_RESET)
			return true;
	else
			return false;
}

bool SW3Active(void)
{
	// SW3 actief?
	if(HAL_GPIO_ReadPin(SW3_GPIO_Port, SW3_Pin) == GPIO_PIN_RESET)
			return true;
	else
			return false;
}

bool SW4Active(void)
{
	// SW4 actief?
	if(HAL_GPIO_ReadPin(SW4_GPIO_Port, SW4_Pin) == GPIO_PIN_RESET)
			return true;
	else
			return false;
}

bool UserButtonActive(void)
{
	// User button actief?
	if(HAL_GPIO_ReadPin(UserButton_GPIO_Port, UserButton_Pin) == GPIO_PIN_RESET)
			return true;
	else
			return false;
}
