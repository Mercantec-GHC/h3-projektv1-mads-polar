﻿using static API.Models.UserDTO;

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

        public ICollection<Device> Devices { get; set; }
        public ICollection<UserDevice> UserDevices { get; set; }

        // Constructor to initialize collections
        public User()
        {
            Devices = new List<Device>(); // Initialize the collection for Devices
            UserDevices = new List<UserDevice>(); // Initialize the collection for UserDevices
        }
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
        
        }// The different roles
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
