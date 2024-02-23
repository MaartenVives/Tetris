#include "stm32l4xx_hal.h"
#include "ad.h"

// De blokkerende versie van de AD-conversie.
uint16_t GetAdValue(ADC_HandleTypeDef* hadc)
{
	uint32_t temp = 0;
	
	HAL_ADC_Start(hadc);
	if(HAL_ADC_PollForConversion(hadc, 100) == HAL_OK)
		temp = HAL_ADC_GetValue(hadc);
	else
		temp = 0;
	
	return (uint16_t)temp;
}
