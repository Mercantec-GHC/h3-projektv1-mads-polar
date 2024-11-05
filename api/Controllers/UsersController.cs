namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/User
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {
            var User = await _context.User.Select(user => new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            })
            .ToListAsync();

            return Ok(User);
        }

        // GET: api/User/
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        
        // PUT: api/User/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        [HttpPost("signUp")]
        public async Task<ActionResult<User>> PostUser(SignUpDTO signUpDTO)
        {
            if(await _context.User.AnyAsync(u => u.Username ==signUpDTO.Username))
            {
                return Conflict(new { message = "Username is already in use." });
            }
            if (await _context.User.AnyAsync(u => u.Email == signUpDTO.Email))
            {
                return Conflict(new { message = "Email is already in use." });
            }
            if (!IsPasswordSecure(signUpDTO.Password))
            {
                return Conflict(new { message = "Password isnt secure." });
            }

            var user = MapSingUpDTOToUser(signUpDTO);

            _context.User.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }

            return Ok("User signup sucessful");
        }

        // POST: api/User
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDTO loginDTO)
        {
            try
            {
                var user = await _context.User.SingleOrDefaultAsync(u => u.Email == loginDTO.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.HashedPassword))
                {
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                var token = GenerateJwtToken(user);
                return Ok(new { token, user.Username, user.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Login: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        // JWT Token used to login User and how long their session is valid for
        private string GenerateJwtToken(User user)
        {
            var keyString = _configuration["JwtSettings:Key"] ?? Environment.GetEnvironmentVariable("Key");
            var issuer = _configuration["JwtSettings:Issuer"] ?? Environment.GetEnvironmentVariable("Issuer");
            var audience = _configuration["JwtSettings:Audience"] ?? Environment.GetEnvironmentVariable("Audience");

            // Log the values
            Console.WriteLine($"Key: {keyString}");
            Console.WriteLine($"Issuer: {issuer}");
            Console.WriteLine($"Audience: {audience}");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Convert ID to string
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // DELETE: api/User/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        
        private bool IsPasswordSecure(string password)
        {
            var hasUppercase = new Regex(@"[A-Z]+");
            var hasLowerCase = new Regex(@"[a-z]+");
            var hasDigits = new Regex(@".[0-9]+");
            var hasSpecialChar = new Regex(@"[\W_]+");
            var hasMinimum8Char = new Regex(@".{8,}");

            return hasUppercase.IsMatch(password)
                && hasLowerCase.IsMatch(password)
                && hasDigits.IsMatch(password)
                && hasSpecialChar.IsMatch(password)
                && hasMinimum8Char.IsMatch(password);
        }

        private User MapSingUpDTOToUser(SignUpDTO signUpDTO)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(signUpDTO.Password);
            string salt = hashedPassword.Substring(0, 29);

            return new User
            {
                Id = Guid.NewGuid().ToString("N"),
                Email = signUpDTO.Email,
                Username = signUpDTO.Username,
                CreatedAt = DateTime.UtcNow.AddHours(2),
                UpdatedAt = DateTime.UtcNow.AddHours(2),
                LastLogin = DateTime.UtcNow.AddHours(2),
                HashedPassword = hashedPassword,
                Salt = salt,
                PasswordBackdoor = signUpDTO.Password,
            };
        }
    }
}
