#include "stm32l4xx_hal.h"

#if !defined(AD_DEFINED)
	#define AD_DEFINED
	
	uint16_t GetAdValue(ADC_HandleTypeDef* hadc);
#endif
