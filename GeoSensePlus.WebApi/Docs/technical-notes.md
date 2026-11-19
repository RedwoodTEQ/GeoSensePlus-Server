# Technical Notes

## Duplicated Point/PointGroup Names Not Allowed

Index restriction is added in PostgreSQL to `Point` and `PointGroup` entities
to prevent duplicated names in the same parent group.

See `20250718152635_2_manul.cs` and the `notice.md` file in the migration folder.

## Dynamic Routing

Purpose: when add an UNS point, it's hierachy path can be automatically turned into a REST API path.

Key implementations:
  - `public class UnsPathTransformer : DynamicRouteValueTransformer`
  - in Startup.cs:  
    `endpoints.MapDynamicControllerRoute<UnsPathTransformer>("uns/{**path}");` and  
    `services.AddTransient<UnsPathTransformer>();`

