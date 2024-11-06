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
    // Arduino color variables
    uint32_t yellowColor = 0xFFFF00; // Yellow color for detected motion
    uint32_t greenColor = 0x00FF00;  // Green color for disarmed state
    uint32_t redColor = 0xFF0000;    // Red color for armed state
    bool ledShowOn = false;

    // To be deleted
    String deviceID = "9ba8ba9bc8f64c448bb34f9ad7da4c01 ";
    String batteryLevel = "100";

    // Motion 
    bool motionDetected = false;
    const int motionPin = A6;
    int motion = 0;

    // Factory Settings
    int motionDetectionDelay = 10000; // Delay in milliseconds (10 seconds)
    int motionDetectionSensitivity = 200; // How much movement needs to happen before the sensor cares
    int motionDetectionDistance = 200; // how far away the sensor can pick up movement from
    int alarmDuration = 300000; // 5 minutes in milliseconds

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
    //void setupWiFi();
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