namespace API.Models
{
    public class DeviceData : Common
    {
        public string DeviceId { get; set; } // Foreign key to the Device
        public Device Device { get; set; } // Navigation property to the Device // Many-To-One relation 

        public DateTime Timestamp { get; set; } // When the data was recorded
        public int SensorValue { get; set; } // Sensor value
        public int BatteryLevel { get; set; } // Battery level when the data was recorded
        public enum Status 
        { 
            Armed,
            Disarmed,
            Alerting
        }
    }
}
