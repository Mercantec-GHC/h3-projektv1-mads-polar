namespace API.Models
{
    public class UserDevice : Common
    {
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
