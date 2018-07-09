///////////////////////////////////////////////
//  LCD control Library
//  functions are below
//    lcd_init()-------- initialize
//    lcd_ready()------- busy check
//    lcd_cmd(cmd)------ send command
//    lcd_data(string)-- display string
//    lcd_clear() ------ clear display
//////////////////////////////////////////////

/////////// lcd ready check function	
int lcd_ready(){
	int high,low;
	set_tris_b(Bmode | 0xF0);	//upper is input
	output_low(rs);
	output_high(rw);			//read mode
	output_high(stb);
	high=db & 0xF0;				//input upper
	output_low(stb);
	output_high(stb); 
	low=db & 0xF0;				//input lower
	output_low(stb);
	set_tris_b(Bmode);
	return(high | (low>>4));	//end check
}

////////// lcd display data function
void lcd_data(int asci){
	db = asci;					//set upper data
	output_low(rw);				//set write
	output_high(rs);			//set rs high
	output_high(stb);			//strobe
	output_low(stb);
	asci=asci<<4;
	db = asci;					//set lower data
	output_high(stb);			//strobe
	output_low(stb);
	delay_us(50);				//’Ç‰Á
	//while(bit_test(lcd_ready(),7));
}

////////// lcd command out function
void cmdout(int cmd){
	db = cmd;					//set upper data
	output_low(rw);				//set write
	output_low(rs);				//set rs low
	output_high(stb);			//strobe
	output_low(stb);
	cmd=cmd<<4;
	db = cmd;					//set lower data
	output_high(stb);			//strobe
	output_low(stb);
}
void lcd_cmd(int cmd){
	cmdout(cmd);
	delay_us(50);				//’Ç‰Á
	//while(bit_test(lcd_ready(),7)); //end check
}

//////////  lcd display clear function
void lcd_clear(){
	lcd_cmd(1);					//initialize command
}

///////// lcd initialize function
void lcd_incmd(int cmd){
	db = cmd;					//mode command
	output_low(rw);				//set write
	output_low(rs);				//set rs low
	output_high(stb);			//strobe
	output_low(stb);
	delay_us(100);
}
void lcd_init(){
	delay_ms(5);
	lcd_incmd(0x30);			//8bit mode set
	lcd_incmd(0x30);			//8bit mode set
	lcd_incmd(0x30);			//8bit mode set
	lcd_incmd(0x20);			//4bit mode set
	lcd_cmd(0x2E);				//DL=0 4bit mode
	lcd_cmd(0x08);				//disolay off C=D=B=0
	lcd_cmd(0x0D);				//display on C=D=1 B=0
	lcd_cmd(0x06);				//entry I/D=1 S=0
}

