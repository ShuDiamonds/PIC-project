#line 1 "tasks.c"
#line 1 "tasks.c"

#line 64 "tasks.c"
 

 
#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 

#line 4 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"

#line 6 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"

#line 9 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
 

#line 16 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
double atof (const auto char *s);

#line 28 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
signed char atob (const auto char *s);


#line 39 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
int atoi (const auto char *s);

#line 47 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
long atol (const auto char *s);

#line 58 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
unsigned long atoul (const auto char *s);


#line 71 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
char *btoa (auto signed char value, auto char *s);

#line 83 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
char *itoa (auto int value, auto char *s);

#line 95 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
char *ltoa (auto long value, auto char *s);

#line 107 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
char *ultoa (auto unsigned long value, auto char *s);
 


#line 112 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
 

#line 116 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
#line 118 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"


#line 124 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
int rand (void);

#line 136 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
 
void srand (auto unsigned int seed);
 
#line 140 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
#line 149 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"

#line 151 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stdlib.h"
#line 67 "tasks.c"

#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"

#line 3 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"


#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
 

#line 4 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"

typedef unsigned char wchar_t;


#line 10 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
 
typedef signed short int ptrdiff_t;
typedef signed short int ptrdiffram_t;
typedef signed short long int ptrdiffrom_t;


#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
 
typedef unsigned short int size_t;
typedef unsigned short int sizeram_t;
typedef unsigned short long int sizerom_t;


#line 34 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
 
#line 36 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"


#line 41 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
 
#line 43 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"

#line 45 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
#line 5 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"

#line 7 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"


#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
#line 22 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"


#line 25 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
#line 27 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"

 

#line 39 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
void *memcpy (auto void *s1, auto const void *s2, auto size_t n);


#line 55 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
void *memmove (auto void *s1, auto const void *s2, auto size_t n);


#line 67 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strcpy (auto char *s1, auto const char *s2);


#line 83 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strncpy (auto char *s1, auto const char *s2, auto size_t n);


#line 97 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strcat (auto char *s1, auto const char *s2);


#line 113 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strncat (auto char *s1, auto const char *s2, auto size_t n);


#line 128 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char memcmp (auto const void *s1, auto const void *s2, auto size_t n);


#line 141 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char strcmp (auto const char *s1, auto const char *s2);


#line 147 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 


#line 161 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char strncmp (auto const char *s1, auto const char *s2, auto size_t n);


#line 167 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 


#line 183 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
void *memchr (auto const void *s, auto unsigned char c, auto size_t n);


#line 199 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strchr (auto const char *s, auto unsigned char c);


#line 210 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
size_t strcspn (auto const char *s1, auto const char *s2);


#line 222 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strpbrk (auto const char *s1, auto const char *s2);


#line 238 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strrchr (auto const char *s, auto unsigned char c);


#line 249 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
size_t strspn (auto const char *s1, auto const char *s2);


#line 262 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strstr (auto const char *s1, auto const char *s2);


#line 305 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strtok (auto char *s1, auto const char *s2);


#line 321 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
void *memset (auto void *s, auto unsigned char c, auto size_t n);


#line 339 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
#line 341 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"


#line 349 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
size_t strlen (auto const char *s);


#line 358 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strupr (auto char *s);


#line 367 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strlwr (auto char *s);



 

#line 379 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom void *memcpypgm (auto far  rom void *s1, auto const far  rom void *s2, auto sizerom_t n);


#line 389 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
void *memcpypgm2ram (auto void *s1, auto const far  rom void *s2, auto sizeram_t n);


#line 398 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom void *memcpyram2pgm (auto far  rom void *s1, auto const void *s2, auto sizeram_t n);


#line 407 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom void *memmovepgm (auto far  rom void *s1, auto const far  rom void *s2, auto sizerom_t n);


#line 417 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
void *memmovepgm2ram (auto void *s1, auto const far  rom void *s2, auto sizeram_t n);


#line 426 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom void *memmoveram2pgm (auto far  rom void *s1, auto const void *s2, auto sizeram_t n);


#line 434 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strcpypgm (auto far  rom char *s1, auto const far  rom char *s2);


#line 443 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strcpypgm2ram (auto char *s1, auto const far  rom char *s2);


#line 451 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strcpyram2pgm (auto far  rom char *s1, auto const char *s2);


#line 460 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strncpypgm (auto far  rom char *s1, auto const far  rom char *s2, auto sizerom_t n);


#line 470 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strncpypgm2ram (auto char *s1, auto const far  rom char *s2, auto sizeram_t n);


#line 479 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strncpyram2pgm (auto far  rom char *s1, auto const char *s2, auto sizeram_t n);


#line 487 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strcatpgm (auto far  rom char *s1, auto const far  rom char *s2);


#line 496 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strcatpgm2ram (auto char *s1, auto const far  rom char *s2);


#line 504 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strcatram2pgm (auto far  rom char *s1, auto const char *s2);


#line 513 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strncatpgm (auto far  rom char *s1, auto const far  rom char *s2, auto sizerom_t n);


#line 523 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strncatpgm2ram (auto char *s1, auto const far  rom char *s2, auto sizeram_t n);


#line 532 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strncatram2pgm (auto far  rom char *s1, auto const char *s2, auto sizeram_t n);


#line 541 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char memcmppgm (auto far  rom void *s1, auto const far  rom void *s2, auto sizerom_t n);


#line 551 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char memcmppgm2ram (auto void *s1, auto const far  rom void *s2, auto sizeram_t n);


#line 560 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char memcmpram2pgm (auto far  rom void *s1, auto const void *s2, auto sizeram_t n);


#line 568 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char strcmppgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 577 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char strcmppgm2ram (auto const char *s1, auto const far  rom char *s2);


#line 585 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char strcmpram2pgm (auto const far  rom char *s1, auto const char *s2);


#line 594 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char strncmppgm (auto const far  rom char *s1, auto const far  rom char *s2, auto sizerom_t n);


#line 604 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char strncmppgm2ram (auto char *s1, auto const far  rom char *s2, auto sizeram_t n);


#line 613 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
signed char strncmpram2pgm (auto far  rom char *s1, auto const char *s2, auto sizeram_t n);


#line 622 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *memchrpgm (auto const far  rom char *s, auto const unsigned char c, auto sizerom_t n);


#line 631 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strchrpgm (auto const far  rom char *s, auto unsigned char c);


#line 639 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
sizerom_t strcspnpgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 647 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
sizerom_t strcspnpgmram (auto const far  rom char *s1, auto const char *s2);


#line 655 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
sizeram_t strcspnrampgm (auto const char *s1, auto const far  rom char *s2);


#line 663 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strpbrkpgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 671 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strpbrkpgmram (auto const far  rom char *s1, auto const char *s2);


#line 679 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strpbrkrampgm (auto const char *s1, auto const far  rom char *s2);


#line 688 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
 


#line 696 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
sizerom_t strspnpgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 704 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
sizerom_t strspnpgmram (auto const far  rom char *s1, auto const char *s2);


#line 712 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
sizeram_t strspnrampgm (auto const char *s1, auto const far  rom char *s2);


#line 720 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strstrpgm (auto const far  rom char *s1, auto const far  rom char *s2);


#line 729 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strstrpgmram (auto const far  rom char *s1, auto const char *s2);


#line 737 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strstrrampgm (auto const char *s1, auto const far  rom char *s2);


#line 745 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strtokpgm (auto far  rom char *s1, auto const far  rom char *s2);


#line 754 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
char *strtokpgmram (auto char *s1, auto const far  rom char *s2);


#line 762 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strtokrampgm (auto far  rom char *s1, auto const char *s2);


#line 771 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom void *memsetpgm (auto far  rom void *s, auto unsigned char c, auto sizerom_t n);


#line 778 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *struprpgm (auto far  rom char *s);


#line 785 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
far  rom char *strlwrpgm (auto far  rom char *s);


#line 792 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
 
sizerom_t strlenpgm (auto const far  rom char *s);

#line 796 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"

#line 798 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"

#line 805 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
#line 814 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"

#line 816 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/string.h"
#line 68 "tasks.c"



#line 72 "tasks.c"
 
#line 74 "tasks.c"

 
#line 1 "./include/FreeRTOS.h"

#line 64 "./include/FreeRTOS.h"
 


#line 68 "./include/FreeRTOS.h"


#line 71 "./include/FreeRTOS.h"
 
#line 1 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
 

#line 10 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"

#line 20 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"

#line 34 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"

#line 41 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
#line 45 "C:/Program Files (x86)/Microchip/mplabc18/v3.43/h/stddef.h"
#line 72 "./include/FreeRTOS.h"



#line 86 "./include/FreeRTOS.h"
 
  

#line 92 "./include/FreeRTOS.h"

 
#line 1 "./include/projdefs.h"

#line 64 "./include/projdefs.h"
 


#line 68 "./include/projdefs.h"


#line 72 "./include/projdefs.h"
 
typedef void (*TaskFunction_t)( void * );

#line 76 "./include/projdefs.h"
#line 77 "./include/projdefs.h"

#line 79 "./include/projdefs.h"
#line 80 "./include/projdefs.h"
#line 81 "./include/projdefs.h"
#line 82 "./include/projdefs.h"

 
#line 85 "./include/projdefs.h"
#line 86 "./include/projdefs.h"
#line 87 "./include/projdefs.h"

#line 89 "./include/projdefs.h"



#line 94 "./include/FreeRTOS.h"


 



#line 100 "./include/FreeRTOS.h"
 

#line 103 "./include/FreeRTOS.h"
#line 104 "./include/FreeRTOS.h"

 
#line 1 "./include/portable.h"

#line 64 "./include/portable.h"
 


#line 68 "./include/portable.h"
 


#line 72 "./include/portable.h"


#line 76 "./include/portable.h"
 
#line 81 "./include/portable.h"

#line 86 "./include/portable.h"

#line 90 "./include/portable.h"

#line 94 "./include/portable.h"

#line 98 "./include/portable.h"

#line 102 "./include/portable.h"


#line 1 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"

#line 64 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
 


#line 68 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"


#line 77 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
 

 
#line 81 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 82 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 83 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 84 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 85 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 86 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 87 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"

typedef uint8_t  StackType_t;
typedef signed char BaseType_t;
typedef unsigned char UBaseType_t;

#line 93 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 96 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
	typedef uint32_t TickType_t;
#line 98 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 99 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
 

 
#line 103 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 104 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 105 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 106 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
 

 
#line 110 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 111 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"

 

#line 114 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 115 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"


#line 119 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
 

#line 126 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 127 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
 

 
extern void vPortYield( void );
#line 132 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
 

 
#line 136 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 137 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
 

 

#line 142 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 143 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"



#line 147 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"
#line 148 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"

#line 150 "./include/../../Source/portable/MPLAB/PIC18F/portmacro.h"

#line 104 "./include/portable.h"

#line 106 "./include/portable.h"

#line 110 "./include/portable.h"

#line 114 "./include/portable.h"

#line 118 "./include/portable.h"

#line 122 "./include/portable.h"

#line 126 "./include/portable.h"

#line 130 "./include/portable.h"

#line 134 "./include/portable.h"

#line 138 "./include/portable.h"

#line 142 "./include/portable.h"

#line 146 "./include/portable.h"

#line 150 "./include/portable.h"

#line 154 "./include/portable.h"

#line 158 "./include/portable.h"

#line 162 "./include/portable.h"

#line 166 "./include/portable.h"

#line 170 "./include/portable.h"

#line 174 "./include/portable.h"

#line 178 "./include/portable.h"

#line 182 "./include/portable.h"

#line 186 "./include/portable.h"

#line 190 "./include/portable.h"

#line 194 "./include/portable.h"

#line 198 "./include/portable.h"

#line 202 "./include/portable.h"

#line 206 "./include/portable.h"

#line 210 "./include/portable.h"

#line 214 "./include/portable.h"

#line 218 "./include/portable.h"

#line 222 "./include/portable.h"

#line 226 "./include/portable.h"

#line 230 "./include/portable.h"

#line 234 "./include/portable.h"

#line 238 "./include/portable.h"

#line 242 "./include/portable.h"

#line 246 "./include/portable.h"


#line 249 "./include/portable.h"
#line 254 "./include/portable.h"


#line 257 "./include/portable.h"
#line 262 "./include/portable.h"

#line 267 "./include/portable.h"
#line 268 "./include/portable.h"

#line 272 "./include/portable.h"
#line 274 "./include/portable.h"
#line 275 "./include/portable.h"
#line 276 "./include/portable.h"

#line 280 "./include/portable.h"

#line 284 "./include/portable.h"


#line 289 "./include/portable.h"

#line 293 "./include/portable.h"

#line 297 "./include/portable.h"

#line 301 "./include/portable.h"

#line 305 "./include/portable.h"

#line 309 "./include/portable.h"

#line 313 "./include/portable.h"


#line 319 "./include/portable.h"
 
#line 323 "./include/portable.h"

#line 325 "./include/portable.h"
#line 327 "./include/portable.h"

#line 329 "./include/portable.h"
#line 331 "./include/portable.h"

#line 333 "./include/portable.h"
#line 335 "./include/portable.h"

#line 337 "./include/portable.h"
#line 338 "./include/portable.h"
#line 339 "./include/portable.h"

#line 343 "./include/portable.h"


#line 346 "./include/portable.h"
#line 347 "./include/portable.h"

#line 351 "./include/portable.h"

#line 1 "./include/mpu_wrappers.h"

#line 64 "./include/mpu_wrappers.h"
 


#line 68 "./include/mpu_wrappers.h"


#line 70 "./include/mpu_wrappers.h"
 

#line 75 "./include/mpu_wrappers.h"
#line 127 "./include/mpu_wrappers.h"
#line 130 "./include/mpu_wrappers.h"
#line 135 "./include/mpu_wrappers.h"
#line 141 "./include/mpu_wrappers.h"
#line 143 "./include/mpu_wrappers.h"

#line 145 "./include/mpu_wrappers.h"
#line 146 "./include/mpu_wrappers.h"
#line 147 "./include/mpu_wrappers.h"

#line 149 "./include/mpu_wrappers.h"


#line 152 "./include/mpu_wrappers.h"

#line 352 "./include/portable.h"



#line 359 "./include/portable.h"
 
#line 361 "./include/portable.h"
#line 363 "./include/portable.h"
	StackType_t *pxPortInitialiseStack( StackType_t *pxTopOfStack, TaskFunction_t pxCode, void *pvParameters ) ;
#line 365 "./include/portable.h"


#line 368 "./include/portable.h"
 
void *pvPortMalloc( size_t xSize ) ;
void vPortFree( void *pv ) ;
void vPortInitialiseBlocks( void ) ;
size_t xPortGetFreeHeapSize( void ) ;
size_t xPortGetMinimumEverFreeHeapSize( void ) ;


#line 378 "./include/portable.h"
 
BaseType_t xPortStartScheduler( void ) ;


#line 385 "./include/portable.h"
 
void vPortEndScheduler( void ) ;


#line 394 "./include/portable.h"
 
#line 396 "./include/portable.h"
#line 399 "./include/portable.h"

#line 403 "./include/portable.h"

#line 405 "./include/portable.h"

#line 106 "./include/FreeRTOS.h"



#line 112 "./include/FreeRTOS.h"
 


