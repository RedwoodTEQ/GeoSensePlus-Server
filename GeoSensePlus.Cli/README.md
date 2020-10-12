# GeoSensePlus.Cli

## About Project

- Responsibility: the CLI tool of GeoSense+ system.
- Published as "gs" global command
- NuGet page: https://www.nuget.org/packages/GeoSensePlus.Cli/
- Any functionalities relevant to firestore are moved to the `GeoSensePlus.Cli.Gsc` project

## Release Notes

### v1.2.0-dev

- [#67](https://gitlab.com/outdoor-asset-tracking-solution/app-front-end/issues/67) Update .gs.config.json set/register/unregister logic
- Upgrade to .net core 3.1

### v1.1.0 (internal)

- Add edge commands: `link-marker`, `display` and `remove`
- Add asset commands:`display`, `get-edge`, `set-edge` and `waggle-edge`
- Remove half finished subcommands for Geofence: `add` and `list`

### v1.0.0

- Add FirebaseCommand: register, set, switch a firebase key file
- Add GeofenceCommand: add, remove and list geofences
- Add TenantCommand: set the current tenant
