#include "stm32l4xx_hal.h"
#include "stdbool.h"

#if !defined(LEDS_DEFINED)
	#define LEDS_DEFINED
	
	void ByteToLeds(uint8_t data);
	void ToggleLed(uint8_t ledNumber);
	void ByteToLevel(uint8_t level);
	void SetUserLed(bool onOff);
#endif
