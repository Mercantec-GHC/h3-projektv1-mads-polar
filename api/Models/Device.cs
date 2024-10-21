namespace API.Models
{
    public class Device : Common
    {
        public string Status { get; set; } // 
        public bool State { get; set; } //Armed, disarmed or detecting motion
        public ICollection<DeviceData> DeviceData { get; set; }
    }

    public class DeviceDTO
    {
        public string Status { get; set; } // 
        public bool State { get; set; } //Armed, disarmed or detecting motion
    }
}
