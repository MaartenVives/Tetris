#include "usart2.h"
#include "stdio.h"

// FILE is typedef’d in stdio.h.
FILE __stdout;
FILE __stdin;

// Redirect printf() to USART2.
int fputc(int ch, FILE *f)
{
	extern UART_HandleTypeDef huart2;
	
	HAL_UART_Transmit(&huart2, (uint8_t*)&ch, 1, HAL_MAX_DELAY);
	return ch;
}

// Redirect scanf() to USART2.
int fgetc(FILE *f)
{
	extern UART_HandleTypeDef huart2;
	int ch = 0;
	
	HAL_UART_Receive(&huart2, (uint8_t*)&ch, 1, HAL_MAX_DELAY);
	return ch;
}
