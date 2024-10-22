namespace API.Models
{
    public class UserDevice : Common
    {
        public string UserId { get; set; } // Foreign key to User
        public string DeviceId { get; set; } // Foreign key to Device

        public User User { get; set; } // Navigation property to User
        public Device Device { get; set; } // Navigation property to Device
        public DeviceData DeviceData { get; set; } // Navigation property to DeviceData (if needed)
    }
}
