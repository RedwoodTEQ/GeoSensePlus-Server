using System;

namespace GeoSensePlus.App.ProgressTracking.Messages
{
    /** The original JSON format
     *  {
     *      timestamp: <timestamp>
     *      user: {
     *          id: "<an unique ID>", 
     *          props: {
     *              battery: 0.81
     *              device_type: 1   //1=phone App    2=wareable device
     *          }
     *      },
     *      tag: {
     *          id: "<an unique ID>", 
     *          props: {
     *              signal: <some number>,
     *              battery: 0.93
     *          }
     *      }
     *  }
     */
    public class IndoorArrivalMessage
    {
        public enum UserDeviceTypes
        {
            ePhoneApp = 1,
            eWearableDevice = 2,
        }

        public DateTime TimeStamp { get; set; }

        public string UserDeviceId { get; set; }
        public double UserDeviceBatteryLevel { get; set; }
        public UserDeviceTypes UserDeviceType { get; set; }

        public string TagId { get; set; }
        public double TagSignal { get; set; }
        public double TagBatteryLevel { get; set; }
    }
}
