namespace API.Models
{
    public class DeviceData : Common
    {
        public string DeviceId { get; set; } // Foreign key to the Device
        public Device Device { get; set; } // Navigation property to the Device // Many-To-One relation 
        public int BatteryLevel { get; set; } // The battery level of the device
    }

    public class DeviceDataDTO
    {
        public string DeviceId { get; set; }
        public int BatteryLevel { get; set; } // The battery level of the device

    }
}
