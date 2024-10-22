namespace API.Models
{
    public class Device : Common
    {
        public string DeviceStatus { get; set; } // Armed, Disarmed, or Alarming
        public string DeviceLocation { get; set; } // Where the sensor is located
        public int BatteryLevel { get; set; } // The battery level of the device
        
        public List<DeviceData> DeviceData { get; set; }
        public List<UserDevice> UserDevices { get; set; }

    }

    public class CreateDeviceDTO
    {
        public string DeviceLocation { get; set; } // Where the sensor is located
    }

    public class DeviceStatusDTO
    {
        public string DeviceStatus { get; set; } // Armed, Disarmed, or Alarming
        public string DeviceLocation { get; set; } // Where the sensor is located
        public int BatteryLevel { get; set; } // The battery level of the device
    }

}
