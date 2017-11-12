/*
ESP-COAP Server
*/

#include <ESP8266WiFi.h>
#include <coap_server.h>
#include <ArduinoJson.h>
#include "DHT.h"


#define DHT_PIN 4
#define DHT_TYPE DHT11

DHT dht(DHT_PIN, DHT_TYPE);

// CoAP server endpoint url callback
void callback_monitoring(coapPacket &packet, IPAddress ip, int port, int obs);

coapServer coap;

//WiFi connection info
const char* ssid = "******";
const char* password = "******";

// CoAP server endpoint URL
void callback_monitoring(coapPacket *packet, IPAddress ip, int port,int obs) {
  float h = dht.readHumidity();
  float t = dht.readTemperature();
  
  // Check if any reads failed and exit early (to try again).
  if (isnan(h) || isnan(t)) {
    Serial.println("Failed to read from DHT sensor!");
    return;
  }

  DynamicJsonBuffer jsonBuffer;
  JsonObject& obj = jsonBuffer.createObject();
  obj["humid"] = h;
  obj["temp"] = t;

  String resp;
  obj.printTo(resp);

  char* buf;
  resp.toCharArray(buf, resp.length());

  coap.sendResponse(buf);
}

void setup() {
  yield();
  //serial begin
  Serial.begin(115200);

  WiFi.begin(ssid, password);
  Serial.println(" ");

  // Connect to WiFi network
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    //delay(500);
    yield();
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
  // Print the IP address
  Serial.println(WiFi.localIP());
  coap.server(callback_monitoring, "monitor");

  // start coap server/client
  coap.start();
  // coap.start(5683);
}

void loop() {
  coap.loop();
  delay(1000);


}
