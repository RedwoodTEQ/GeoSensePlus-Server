# Release Notes
## v1.2.0-dev

Working:

Done:

- Upgrade to .net10
- Rename "CoordinationTagsController" to "UwbTagsController"
- Rename "MetricsController" to "InfluxdbController"
- Add more REST API controllers (not fully implemented)
- Implement API: 
  - get /system/info
  - get /system/firebase-key
  - post /metrics
  - get /metrics/metric/unit/range
- rename api /textdata to /api/message
- Upgrade MqttNet package
- Add support for InfluxDB
- Add support for OpenAPI

**Non-CLI part**

- Add docker support
- Add dapr batch script `serve-dapr.bat`

## v1.1.1

- When a new message is received, publish the raw message to "RawMessage" MQTT topic
- Add REST API endpoint: `/healthz`
- [#65](https://gitlab.com/outdoor-asset-tracking-solution/app-front-end/issues/65) Dockerize services and orchestrate with docker-compose
- Upgrade to .net core 3.1

## v1.1.0 (internal)

- Bug fix: [#56](https://gitlab.com/outdoor-asset-tracking-solution/app-front-end/issues/56) Edge timestamp is not updated when processing a new asset report message

## v1.0.0

- Integrate REST & gRPC channels into message engine
- Add MQTT channel without integrating with message engine

