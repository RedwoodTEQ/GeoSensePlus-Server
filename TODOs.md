# TODO

## Working

- [OPC UA] Client: Refactor to create a OPC UA client library
- Add basic "DataPoint" implementation
- [OPC UA] Client: Create a simple desktop UI, tech options (pick from left to right):  
  React Native desktop, Blazor PWA, Angular + Electron, Avalonia UI, Blazor Hybrid (with WinForm or WPF)

---

- [MQTT] Test the upgraded MQTTNet code, may need to build a test tool
- [Demo] Forward value between external OPC UA Server tags, external MQTT topics and local data points
- [Demo] Expose DataPoint values via builtin OPC UA Server and MQTTNet

## Nexting

- [OPC UA] Add server integration
- CI/CD with version number updating

## Waiting

- [Bug] When close OPC UA Winform window, the program will still run in the background
- Add doc for MessageProcessing
