namespace API.Models
{
    public class Device : Common
    {
        public string Name { get; set; } // fx. Living room sensor
        public string DeviceStatus { get; set; } // Armed, Disarmed, or Alarming
        public string Location { get; set; } // Where the sensor is located
        public DateTime LastTriggered { get; set; } // The last time the sensor went off
        public int BatteryLevel { get; set; } // The battery level of the device
        public bool IsOnline { get; set; } // Used to check if the device is connected to the server or not 
        
        public List<DeviceData> DeviceData { get; set; }
        public List<UserDevice> UserDevices { get; set; }

    }

    public class DeviceDTO
    {
        public string Name { get; set; } // Device name provided by the user, "Living room sensor"
        public string DeviceState { get; set; } // Armed, Disarmed, or Alarming
        public string Location { get; set; } // User-provided location for the device, "Living room"
    }

    public class DeviceStatusDTO
    {
        public string DeviceState { get; set; } // Armed, Disarmed, or Alarming
        public string Location { get; set; } // User-provided location for the device, "Living room"
        public DateTime LastTriggered { get; set; } // The last time the device was triggred
        public int BatteryLevel { get; set; } // The battery level of the device
        public bool IsOnline { get; set; } // Used to check if the device is connected to the server or not 
    }

}
