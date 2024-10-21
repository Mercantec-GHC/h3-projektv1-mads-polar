namespace API.Models
{
    public class User : Common
    {
        // This is the sign up/ login requirements
        public string Email { get; set; }
        public string Username { get; set; }

        // This is used for security
        public string HashedPassword { get; set; }
        public string Salt { get; set; }

        // This is used to check whenever the user was last loged in
        public DateTime LastLogin { get; set; } 
        public string PasswordBackdoor { get; set; }
        public ICollection<UserDevice> UsersDevice { get; set; }
    }


    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        //public Roles Roles { get; set; }
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
