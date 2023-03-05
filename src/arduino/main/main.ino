#include <Adafruit_NeoPixel.h>

#define LED_PIN 6
#define LED_COUNT 50
Adafruit_NeoPixel strip(LED_COUNT, LED_PIN, NEO_GRB + NEO_KHZ800); 
bool buttonState = false;
bool on = false;
int BUTTON_PIN = 0; // Using RX pin for general boolean input

void setup()
{
  Serial.begin(9600);
  pinMode(BUTTON_PIN, INPUT);
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'
}

void loop()
{
  buttonState = digitalRead(BUTTON_PIN);
  if(buttonState == false) {
    if(!on) {
      on = true;
      Serial.println("down");
      Serial.println("on");
      Magenta();
      delay(50); //debounce
    } else {
      on = false;
      Serial.println("up");
      Serial.println("off");
      Off();
      delay(50); //debounce
    }
  } else {
    /**
     * occurs on on UP button event
    if(on) {
      on = false;
      Serial.println("up");
      Serial.println("off");
      Off();
      delay(50); //debounce
    }
    */
  }
}

void Rainbow() {

}

void Rainbow_Spin() {
  
}

void Off() {
  Serial.println("method::Off");
  for(int i=0;i<LED_COUNT;i++){
    strip.setPixelColor(i, strip.Color(0,0,0)); 
    strip.show(); 
    delay(10); 
  }
}

void Magenta() {
  Serial.println("method::White");
  for(int i=0;i<LED_COUNT;i++){
    strip.setPixelColor(i, strip.Color(0,255,255)); 
    strip.show(); 
    delay(10); 
  }
}
