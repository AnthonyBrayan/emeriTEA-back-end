using Data;
using emeriTEA_back_end.IServices;
using Entities;
using Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using emeriTEA_back_end.Services;

namespace emeriTEA_back_end.Controllers
{
    [EnableCors("AllowAll")]
    [Route("[controller]/[action]")]
      
    public class AdministradorControlle : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdministradorService _administradorService;
        private readonly ITokenService _tokenService;
        private readonly ServiceContext _serviceContext;

        public AdministradorControlle(IConfiguration configuration, IAdministradorService administradorService, ITokenService tokenService, ServiceContext serviceContext)
        {
            _configuration = configuration;
            _administradorService = administradorService;
            _tokenService = tokenService;
            _serviceContext = serviceContext;
        }

        [HttpPost(Name = "InsertAdministrador")]
        public IActionResult Post([FromBody] Administrador administrador)
        {
            try
            {
                Console.WriteLine("Id_administrador received: " + administrador.Id_Administrador);
                var userId = _tokenService.ExtractUserIdFromToken(HttpContext);
                //var userId = _tokenService.ExtractUserIdFromAuthorizationHeader(HttpContext);

                if (userId == null)
                {

                    return Unauthorized("User is not authenticated.");
                }
                else
                {
                    var existingUserWithSameEmail = _serviceContext.Administrador.FirstOrDefault(u => u.Email == administrador.Email);

                    if (existingUserWithSameEmail != null)
                    {
                        return StatusCode(409, "A administrador with the same email address already exists.");
                    }
                    else
                    {

                        administrador.Password = BCrypt.Net.BCrypt.HashPassword(administrador.Password);

                        return Ok(_administradorService.InsertAdministrador(administrador));
                    }
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting role ID: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestModel loginRequest)
        {
            try
            {
                var administrador = _serviceContext.Administrador.FirstOrDefault(u => u.Email == loginRequest.Email);

                if (administrador != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, administrador.Password))
                {
                    var token = GenerateJwtToken(administrador);

                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,

                    });

                    return Ok(new { Token = token});

                }
                else
                {
                    return StatusCode(401, "Credenciales incorrectas");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al iniciar sesión: {ex.Message}");
            }
        }

        private string GenerateJwtToken(Administrador administrador)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, administrador.Id_Administrador.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
