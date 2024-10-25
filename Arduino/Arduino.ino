#include "SHS.h"

    SHS shs("MAGS-OLC", "Merc1234!");

// To be deleted
String deviceID = "41cfd26dd68d46a89a299e361454dc9c";
String batteryLevel = "100";

void setup()
{
    shs.begin();
}

void loop()
{
    delay(100);                   // Delay to avoid constant polling
    shs.carrier.Buttons.update(); // Check button states

    shs.readSensors();
    shs.printData();

    // Continuously monitor motion when armed
    if (shs.isArmed)
    {
        shs.checkMotion();
    }

    // Button actions
    if (shs.carrier.Buttons.onTouchDown(TOUCH0))
    {
        // Disarm system
        shs.disarmSystem();
    }

    if (shs.carrier.Buttons.onTouchDown(TOUCH1))
    {
        // Arm system
        shs.armSystem();
    }

    if (shs.carrier.Buttons.onTouchDown(TOUCH2))
    {
        // Turn off LEDs
        shs.turnOffLEDs();
    }
}