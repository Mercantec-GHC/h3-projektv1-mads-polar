#include "SHS.h"

SHS::SHS(const char *ssid, const char *password): ssid(ssid), password(password) {}

void SHS::begin()
{
    CARRIER_CASE = false; // Deactivate carrier case if not mounted
    carrier.begin();
    Serial.begin(9600); // Initialize the serial communication
    delay(2000);        // Show message for 2 seconds

    connectWiFi(); // Connect to WiFi
    httpClient = new HttpClient(wifiClient, "shs-ubpj.onrender.com", 443);
}

void SHS::checkMotion()
{
    int motionValue = analogRead(motionPin);

    if (motionValue > threshold && !motionDetected)
    {
        motionDetected = true;
        Serial.println("Motion Detected!");

        carrier.display.fillScreen(ST77XX_YELLOW); // Display movement detection
        carrier.display.setCursor(20, 120);
        carrier.display.setTextColor(ST77XX_BLACK);
        carrier.display.println("Movement Detected!");

        // Turn on yellow LEDs
        for (int i = 0; i < NUMPIXELS; i++)
        {
            carrier.leds.setPixelColor(i, yellowColor);
        }

        carrier.leds.show(); // Refresh LEDs
        delay(200);

        // Keep the buzzer sounding in a loop until TOUCH0 is pressed
        while (isArmed && motionDetected)
        {
            // carrier.Buzzer.sound(1000);  // Play a 1kHz tone
            delay(500); // Tone duration

            // Update buttons to check if TOUCH0 is pressed to disarm
            carrier.Buttons.update();

            if (carrier.Buttons.onTouchDown(TOUCH0))
            { // Disarm during buzzer
                disarmSystem();
                break; // Exit the loop when disarmed
            }

            // Check for motion again to decide whether to continue the loop
            motionValue = analogRead(motionPin);

            if (motionValue >= threshold)
            {
                motionDetected = false; // No motion, exit the loop
            }
        }
    }
}

void SHS::armSystem()
{
    isArmed = true;
    ledShowOn = true;
    motionDetected = false; // Reset motion detection

    // Display "Armed" on the screen
    carrier.display.fillScreen(ST77XX_RED); // Set background color
    carrier.display.setTextSize(2);
    carrier.display.setCursor(90, 120);
    carrier.display.setTextColor(ST77XX_WHITE);
    carrier.display.print("Armed");

    Serial.println("Device Armed");

    // Turn on red LEDs
    for (int i = 0; i < NUMPIXELS; i++)
    {
        carrier.leds.setPixelColor(i, redColor);
    }

    carrier.leds.show(); // Refresh LEDs
    updateStatus();
    sendData();
    delay(200);
}

void SHS::disarmSystem()
{
    isArmed = false; // Disarm system
    ledShowOn = false;
    carrier.Buzzer.noSound(); // Stop the buzzer

    // Display "Disarmed" on the screen
    carrier.display.fillScreen(ST77XX_GREEN); // Set background color
    carrier.display.setTextSize(2);
    carrier.display.setCursor(70, 120);
    carrier.display.setTextColor(ST77XX_WHITE);
    carrier.display.print("Disarmed");

    Serial.println("Device Disarmed");

    // Turn on green LEDs
    for (int i = 0; i < NUMPIXELS; i++)
    {
        carrier.leds.setPixelColor(i, greenColor);
    }
    carrier.leds.show(); // Refresh LEDs
    updateStatus();
    delay(200);
}

void SHS::turnOffLEDs()
{
    // Turn off all LEDs
    ledShowOn = false;

    for (int i = 0; i < NUMPIXELS; i++)
    {
        carrier.leds.setPixelColor(i, 0);
    }
    carrier.leds.show(); // Refresh LEDs

    // Display "Off" on the screen
    carrier.display.fillScreen(ST77XX_BLACK); // Clear screen
    carrier.display.setTextSize(2);
    carrier.display.setCursor(102, 120);
    carrier.display.setTextColor(ST77XX_WHITE);
    carrier.display.print("OFF");

    Serial.println("Device Turned off");
}

void SHS::connectWiFi()
{
    Serial.print("Connecting to ");
    Serial.println(ssid);

    carrier.display.setTextSize(2);
    carrier.display.setCursor(30, 80);
    carrier.display.print("Connecting to ");
    carrier.display.setCursor(30, 100);
    carrier.display.print(ssid);

    while (WiFi.status() != WL_CONNECTED) {
        WiFi.begin(ssid, password);
        delay(1000);
        Serial.print(".");
    }

    carrier.display.fillScreen(0x0000);
    Serial.println("");
    Serial.println("WiFi Connected.");
    Serial.println("IP address: ");
    Serial.println(WiFi.localIP());
}

void SHS::readSensors() 
{
    // Assuming you have a motion sensor
    motion = analogRead(motionPin);  // or carrier.Env.readMotion();
}

void SHS::printData() 
{
    carrier.display.fillScreen(0x0000);
    Serial.print("Motion: ");
    Serial.println(motion);

    sendData();
}

void SHS::sendData() 
{
  /*
    String postData = "{\"deviceId\":" + deviceID + ",\"batteryLevel\":" + String(batteryLevel) + "}";

    httpClient->beginRequest();
    httpClient->post("/api/DeviceDatas");
    httpClient->sendHeader("Content-Type", "application/json");
    httpClient->sendHeader("Content-Length", postData.length());
    httpClient->sendHeader("accept", "");
    httpClient->beginBody();
    httpClient->print(postData);
    httpClient->endRequest();

    int statusCode = httpClient->responseStatusCode();
    String response = httpClient->responseBody();

    Serial.print("Status code: ");
    Serial.println(statusCode);
    Serial.print("Response: ");
    Serial.println(response);*/
}

String SHS::boolToString(bool isArmed, bool motionDetected) 
{
  if(isArmed && motionDetected){
      return "2";
  }
  
  else if (isArmed) {
      return "1";
  } 

  else {
      return "0";
  }
}

void SHS::updateStatus()
{
  String postData = "{\"deviceStatus\": " + boolToString(isArmed, motionDetected) + "}";

  Serial.println("Forsøger at opdatere status...");
  Serial.println("Anmodningsdata: " + postData);
  Serial.println("Enhedens ID: " + deviceID);

  httpClient->beginRequest();
  httpClient->put("/api/Devices?id=" + deviceID);
  httpClient->sendHeader("Content-Type", "application/json");
  httpClient->sendHeader("Content-Length", postData.length());
  httpClient->sendHeader("accept", "*/*");
  httpClient->beginBody();
  httpClient->print(postData);
  httpClient->endRequest();

  int statusCode = httpClient->responseStatusCode();
  String response = httpClient->responseBody();

  Serial.print("Statuskode: ");
  Serial.println(statusCode);
  Serial.print("Svar: ");
  Serial.println(response);

  if (statusCode == 204) {
    Serial.println("Status opdateret succesfuldt.");
  } else {
    Serial.println("Fejl ved opdatering af status. Kontroller forbindelse og enhedens ID.");
  }
}