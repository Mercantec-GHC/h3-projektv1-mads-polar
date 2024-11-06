namespace API.Models
{
    public class Device : Common
    {
        public Status DeviceStatus { get; set; } // Armed, Disarmed, or Alarming
        public string DeviceLocation { get; set; } // Where the sensor is located
        public int MotionSensorSensitivity { get; set; } // Sensor value
        public List<UserDevice> UserDevices { get; set; }

    }

    public class CreateDeviceDTO
    {
        public string DeviceLocation { get; set; } // Where the sensor is located
        public int MotionSensorSensitivity { get; set; }
    }

    public class PutDeviceDTO
    {
        public Status DeviceStatus { get; set; } // Armed, Disarmed, or Alarming

        //public string DeviceLocation { get; set; }
    }

    public enum Status
    {
        Disarmed,
        Armed,
        Alerting
    }
}
