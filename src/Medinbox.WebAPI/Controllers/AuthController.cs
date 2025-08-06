using Medinbox.Application.Constants;
using Medinbox.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Medinbox.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT with a random role.
        /// </summary>
        /// <param name="request">Login details</param>
        /// <returns>JWT + assigned role</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest(new { message = "Invalid request body." });

                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest("Missing username or password");

                // Random role
                var roles = new[] { Roles.Reader, Roles.Writer, Roles.ReaderWriter };
                var random = new Random();
                var assignedRole = roles[random.Next(roles.Length)];

                //Generate the token
                var token = GenerateJwtToken(request.Username, assignedRole);

                // Response
                return Ok(new
                {
                    token,
                    role = assignedRole,
                    color = Roles.RoleColors[assignedRole]
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l’ajout d’un équipement");
                return StatusCode(500, new { message = "Internal server error. Please try again later." });
            }
        }
        /// <summary>
        /// Generates a signed JWT token with the given role and username.
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="role">role</param>
        /// <returns>Signed JWT token string</returns>
        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
