// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using AuthApi.Models;
using Microsoft.Data.SqlClient;


namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            string query = "SELECT COUNT(*) FROM Login WHERE Username = @username AND Password = @password";
            using SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", login.Username);
            cmd.Parameters.AddWithValue("@password", login.Password);

            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
                return Ok(new { message = "Login successful" });

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
