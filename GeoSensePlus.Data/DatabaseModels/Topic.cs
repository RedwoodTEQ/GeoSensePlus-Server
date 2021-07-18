using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class TopicEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Subscription topic
    /// Used by MQTT or any other notification engines
    /// </summary>
    public class Topic : TopicEntity, IIdAvailable<int>
    {
        public int TopicId { get; set; }

        public int GetId()
        {
            return TopicId;
        }
    }
}
