using GeoSensePlus.Data.DatabaseModels.Base;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location;
public class LevelEntity
{
}

public class Level : LevelEntity, IIdAvailable<int>
{
    public int LevelId { get; set; }

    public Building Building { get; set; }

    public List<Floorplan> Floorplans { get; set; }

    public int GetId()
    {
        return LevelId;
    }
}
