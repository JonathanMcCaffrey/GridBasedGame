
const int left = 7;
const int forward = 6;
const int right = 5;
const int fire = 4;


void setup() {
  Serial.begin(57600);
  
  pinMode(left, INPUT);
  pinMode(forward, INPUT);
  pinMode(right, INPUT);
  pinMode(fire, INPUT);
  
  digitalWrite(left, HIGH);
  digitalWrite(forward, HIGH);
  digitalWrite(right, HIGH);
  digitalWrite(fire, HIGH);
  
}

void loop() {
  serialMessage(left);
  serialMessage(forward);
  serialMessage(right);
  serialMessage(fire); 
}

void serialMessage(int pin) {
  if(digitalRead(pin) == LOW) {
    Serial.write(pin);
    Serial.flush();
    delay(20);  
  }
}



