# TODO

## Working

- Add basic "ValuePoint" implementation
- [MQTT] Test the upgraded MQTTNet code, may need to build a test tool
- [Test] Add an integration test project

---

- [Demo] Forward value between external OPC UA Server tags, external MQTT topics and local data points
- [Demo] Expose DataPoint values via builtin OPC UA Server and MQTTNet
- [DevOps] CI/CD with version number updating

## Nexting

- [OPC_UA] Client: Create a simple desktop UI, tech options (pick from left to right):  
  React Native desktop, Blazor PWA, Angular + Electron, Avalonia UI, Blazor Hybrid (with WinForm or WPF)
- [OPC_UA] Client: Refactor to create a OPC UA client library
- [OPC_UA] Add server integration

## Waiting

- [Bug] When close OPC UA Winform window, the program will still run in the background
- Add doc for MessageProcessing

- [LoRaWAN] Recover TTN integration (for `gsttn` in `docker-compose.yml`, need to move the key to system vars)
