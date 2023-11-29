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
      Rainbow();
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
  Serial.println("method::Rainbow");
  for(int i=0;i<LED_COUNT;i++) {
    int pixel_index = (floor((i * 256) / LED_COUNT));
    strip.setPixelColor(i, Wheel(pixel_index & 255));
    strip.show();
    delay(10); 
  }
}

uint32_t Wheel(int pos) {
  int r;
  int g;
  int b;
  if(pos < 0 || pos > 255) {
    r = 0; g = 0; b = 0;
  } else if(pos < 85) {
    r = floor(pos * 3);
    g = floor(255 - pos * 3);
    b = 0;
  } else if(pos < 170) {
    pos = pos - 85;
    r = floor(255 - pos * 3);
    g = 0;
    b = floor(pos * 3);
  } else {
    pos = pos - 170;
    r = 0;
    g = floor(pos * 3);
    b = floor(255 - pos * 3); 
  }
  return strip.Color(g, r, b);
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
