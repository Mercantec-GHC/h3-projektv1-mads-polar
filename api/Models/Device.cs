namespace API.Models
{
    public class Device : Common
    {
        public string Status { get; set; } //Armed, disarmed or detecting motion
        public string DeviceId { get; set; }
    }

    public class DeviceDTO
    {
        public string DeviceId { get; set }
        public string Status { get; set; }
    }
    }
}
