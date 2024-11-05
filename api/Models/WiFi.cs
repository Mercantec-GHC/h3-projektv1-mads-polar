namespace API.Models
{
    public class WiFi : Common
    {
        public string WiFiName { get; set; }
        public string WiFiPassword { get; set; }
        public string DeviceId { get; set; }
        public Device Device { get; set; }
    }

    public class WiFiDTO
    {
        public string WiFiName { get; set; }
        public string WiFiPassword { get; set; }
        public string DeviceId { get; set; }
    }
}
