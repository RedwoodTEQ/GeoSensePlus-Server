<link href="styles.css" rel="stylesheet"></link>

# GeoSensePlus.Server

## About Project

- Responsibility: the main application server of GeoSense+ system
- Published as a global tool: gsv
- NuGet page: https://www.nuget.org/packages/GeoSensePlus.Server/
- For more info about controllers, see the relevant entities' comments

## TODO

- Temporarily commented code, search: _todo_2509010311
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

Note:

  The first migration will get the following error:

  An error occurred using the connection to database 'geosenseplus' on server 'tcp://localhost:5432'.

  Still not sure why, but the migration seems to work, so just igoore it.


