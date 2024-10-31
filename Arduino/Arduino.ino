#include "SHS.h"

    SHS shs("MAGS-OLC", "Merc1234!");

void setup()
{
    shs.begin();
}

void loop() 
{
    delay(100); // Delay to avoid constant polling
    shs.carrier.Buttons.update(); // Check button states

    // Continuously monitor motion when armed
    if (shs.isArmed) {
        shs.checkMotion();
    }

    // Button actions with toggle state
    if (shs.carrier.Buttons.onTouchDown(TOUCH0) && !shs.disarmButtonPressed) 
    { 
        shs.disarmSystem(); // Disarm system
        shs.disarmButtonPressed = true;
    } 
    else if (shs.carrier.Buttons.onTouchUp(TOUCH0)) 
    {
        shs.disarmButtonPressed = false;
    }

    if (shs.carrier.Buttons.onTouchDown(TOUCH1) && !shs.armButtonPressed) 
    { 
        shs.armSystem(); // Arm system
        shs.armButtonPressed = true;
    } 
    else if (shs.carrier.Buttons.onTouchUp(TOUCH1)) 
    {
        shs.armButtonPressed = false;
    }

    if (shs.carrier.Buttons.onTouchDown(TOUCH2) && !shs.ledButtonPressed) 
    { 
        shs.turnOffLEDs(); // Turn off LEDs
        shs.ledButtonPressed = true;
    } 
    else if (shs.carrier.Buttons.onTouchUp(TOUCH2)) 
    {
        shs.ledButtonPressed = false;
    }
}
