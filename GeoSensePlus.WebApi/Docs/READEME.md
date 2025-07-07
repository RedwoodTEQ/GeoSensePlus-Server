<link href="styles.css" rel="stylesheet"></link>

# GeoSensePlus.Server

## About Project

- Responsibility: the main application server of GeoSense+ system
- Published as a global tool: gsv
- NuGet page: https://www.nuget.org/packages/GeoSensePlus.Server/
- For more info about controllers, see the relevant entities' comments

## TODO

- Add an angular front end project
- Test Dockerfile
- Test controllers
- Change MongoController<> to a util class
- Change API methods to async

---
You can use `<todo>` and `<note>` to highlight TODOs and notes.

Example: <note>test note</note> <todo>test todo</todo>

## Initial Setup

- Startup PostgreSQL server  
  Approach 1: Use Docker
  ```
  docker run --name postgres -e POSTGRES_PASSWORD=postgres -d -p 5432:5432 postgres
  ```

  Approach 2: Use Docker Compose
  ```
  Navigate to project "docker-compose-more", do:
  docker-compose -f postgresql.yml up
  ```

- Database migration
  ```
  Navigate to project "GeoSensePlus.Data", execute:
  update-database.bat
  ```

## Release Notes

### v1.2.0-dev

Working:

Done:

- Upgrade to .net9
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

### v1.1.1

- When a new message is received, publish the raw message to "RawMessage" MQTT topic
- Add REST API endpoint: `/healthz`
- [#65](https://gitlab.com/outdoor-asset-tracking-solution/app-front-end/issues/65) Dockerize services and orchestrate with docker-compose
- Upgrade to .net core 3.1

### v1.1.0 (internal)

- Bug fix: [#56](https://gitlab.com/outdoor-asset-tracking-solution/app-front-end/issues/56) Edge timestamp is not updated when processing a new asset report message

### v1.0.0

- Integrate REST & gRPC channels into message engine
- Add MQTT channel without integrating with message engine
