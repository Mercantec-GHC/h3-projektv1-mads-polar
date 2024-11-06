#include "SHS.h"

// 
SHS shs("MAGS-OLC", "Merc1234!");

// This 
void setup()
{
    shs.begin();
}

//
void loop() 
{
    delay(100); // Delay to avoid constant polling
    shs.carrier.Buttons.update(); // Check button states

    // Disarms the system
    if (shs.carrier.Buttons.onTouchDown(TOUCH0) && !shs.disarmButtonPressed) 
    { 
        shs.disarmSystem(); // Disarm system
        shs.disarmButtonPressed = true;
    } 

    // Arms the system
    if (shs.carrier.Buttons.onTouchDown(TOUCH1) && !shs.armButtonPressed) 
    { 
        shs.armSystem(); // Arm system
        shs.armButtonPressed = true;
    } 

    // This is only used to turn off the LED's and Display (Only works when device is disarmed otherwise the system keeps having inputs so it wont turn off)
    if (shs.carrier.Buttons.onTouchDown(TOUCH2) && !shs.ledButtonPressed) 
    { 
        shs.turnOffLEDs(); // Turn off LEDs
        shs.ledButtonPressed = true;
    } 
}
