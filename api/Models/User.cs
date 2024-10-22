using static API.Models.UserDTO;

namespace API.Models
{
    // This is the sign up/ login requirements
    public class User : Common
    {
        public string Email { get; set; }
        public string Username { get; set; }

        // This is used for security
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public Role Role { get; set; } // 0 = User, 1 = Admin

        public DateTime LastLogin { get; set; } // This is used to check whenever the user was last loged in
        public string PasswordBackdoor { get; set; }

        public List<UserDevice> UserDevices { get; set; }
    }


    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public enum Role
        {
            User,
            Admin
        
        }
    }


    public class LoginDTO
    {
        // This is the login requirements
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SignUpDTO
    {
        // This is the sign up requirements
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
