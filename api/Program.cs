using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            IConfiguration Configuration = builder.Configuration;

            string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("DefaultConnection");

            builder.Services.AddDbContext<AppDBContext>(options => options.UseNpgsql(connectionString));

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                string issuer = Configuration["JwtSettings:Issuer"] ?? Environment.GetEnvironmentVariable("Issuer");
                string audience = Configuration["JwtSettings:Audience"] ?? Environment.GetEnvironmentVariable("Audience");
                string key = Configuration["JwtSettings:Key"] ?? Environment.GetEnvironmentVariable("Key");

                // Debugging: Output the values of these settings
                Console.WriteLine($"Issuer: {issuer}");
                Console.WriteLine($"Audience: {audience}");
                Console.WriteLine($"Key: {key}");

                // Check if any value is null or empty to understand the issue
                if (string.IsNullOrEmpty(issuer))
                    Console.WriteLine("Warning: Issuer is null or empty.");
                if (string.IsNullOrEmpty(audience))
                    Console.WriteLine("Warning: Audience is null or empty.");
                if (string.IsNullOrEmpty(key))
                    Console.WriteLine("Warning: Key is null or empty.");

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });


            builder.Services.AddAuthentication();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
