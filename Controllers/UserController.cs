using FlightStatusControlAPI.Interfaces;
using FlightStatusControlAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightStatusControlAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUserRepository _userRepository;
        private IConfiguration _config;

        public UserController(IFlightRepository flightRepository,IUserRepository userRepository, IConfiguration config)
        {
            _flightRepository = flightRepository;
            _userRepository = userRepository;
            _config = config;
        }
        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult GetToken([FromBody] LoginModel user)
        {
            User _user = _userRepository.Login(user.Username, user.Password);
            Role _role = _flightRepository.GetRoleById(_user.RoleId);
            _user.Password = null;

            if (_user != null)
            {
                var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                        new Claim(JwtRegisteredClaimNames.Name, user.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, _role.Code)
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.UtcNow.AddDays(30),
                    signingCredentials: signIn
                );
                var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = encodeToken });
            }
            else
            {
                return Unauthorized("Invalid username or password");
            }
        }
    }
}
