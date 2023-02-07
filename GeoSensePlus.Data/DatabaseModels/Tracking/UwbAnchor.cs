using GeoSensePlus.Data.DatabaseModels.Base;
using GeoSensePlus.Data.DatabaseModels.Location;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Tracking;
public class UwbAnchorEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double AxisX { get; set; } = -1;
    public double AxisY { get; set; } = -1;
    public double AxisZ { get; set; } = -1;
    //[Column(TypeName = "jsonb")]
    public string Configuration { get; set; }
    public string Status { get; set; }

    /** TODO: confirm with Kai, more properties?
     * Group?
     * Owner?
     * Attribute?
     * Info? -- RL: redundant with "Description"?
     * Extra?
     */
}
public class UwbAnchor : UwbAnchorEntity, IIdAvailable<int>
{
    public int UwbAnchorID { get; set; }
    public Floorplan Floorplan { get; set; }
    public Site Site { get; set; }

    public int GetId()
    {
        return UwbAnchorID;
    }
}
