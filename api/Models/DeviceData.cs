namespace API.Models
{
    public class DeviceData : Common
    {
        public string DeviceId { get; set; } // Foreign key to the Device
        public Device Device { get; set; } // Navigation property to the Device // Many-To-One relation 
        public int MotionSensorSensitivity { get; set; } // Sensor value
        public Status Status {  get; set; }
    }

    public class DeviceDataDTO
    {
        public string DeviceId { get; set; }
        public int MotionSensorSensitivity { get; set; }
        public Status Status { get; set; }
    }

    public enum Status 
    { 
        Disarmed,
        Armed,
        Alerting
    }
}
