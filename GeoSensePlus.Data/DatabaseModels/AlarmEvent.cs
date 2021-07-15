﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class AlarmEventEntity
    {
        /// <summary>
        /// Values:
        /// 4: fatal
        /// 3: failure or exception
        /// 2: warning
        /// 1: information with state change
        /// 0: information without state change
        /// </summary>
        public string Severity { get; set; }

        public string Source { get; set; }

        /// <summary>
        /// Lifecycle state: acknowledge, restore, clear, etc
        /// </summary>
        public string State { get; set; }

        public string Description { get; set; }
    }

    public class AlarmEvent : AlarmEventEntity
    {
        public int AlarmEventId { get; set; }
    }
}
