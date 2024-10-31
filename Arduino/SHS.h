#ifndef SHS_H
#define SHS_H

#include <Arduino_MKRIoTCarrier.h>
#include <SPI.h>
#include <WiFiNINA.h>
#include <ArduinoHttpClient.h>

// Device Constants
#define NUMPIXELS 5

    class SHS
{
private:
    // Variables
    uint32_t yellowColor = 0xFFFF00; // Yellow color for detected motion
    uint32_t greenColor = 0x00FF00;  // Green color for disarmed state
    uint32_t redColor = 0xFF0000;    // Red color for armed state

    bool motionDetected = false;
    const int threshold = 800;
    const int motionPin = A6;
    bool ledShowOn = false;
    int motion = 0;

    // Global timer variable
    int motionDelay = 10000;  // Delay in milliseconds (10 seconds)
    int deviceDelay = 10000;  // Delay in milliseconds (20 seconds)

    // To be deleted
    String deviceID = "41cfd26dd68d46a89a299e361454dc9c ";
    String batteryLevel = "100";

    const char *ssid;
    const char *password;

public:

    // Global state variables for toggle handling
    bool armButtonPressed = false;
    bool disarmButtonPressed = false;
    bool ledButtonPressed = false;

    bool isArmed = false;

    MKRIoTCarrier carrier;

    // Network Configuration
    WiFiSSLClient wifiClient;
    HttpClient *httpClient;

    SHS(const char *ssid, const char *password);
    void begin();
    void connectWiFi();
    void readSensors();
    void checkMotion();
    void armSystem();
    void disarmSystem();
    void turnOffLEDs();
    String boolToString(bool isArmed, bool motionDetected);
    void updateStatus();
};

#endif // SENSORDATA_H