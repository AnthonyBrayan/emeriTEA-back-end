using Data;
using emeriTEA_back_end.IServices;
using Entities;
using Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Cors;

namespace emeriTEA_back_end.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("[controller]/[action]")]
    public class AdministradorControlle : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdministradorService _administradorService;
        private readonly ServiceContext _serviceContext;

        public AdministradorControlle(IConfiguration configuration, IAdministradorService administradorService, ServiceContext serviceContext)
        {
            _configuration = configuration;
            _administradorService = administradorService;
            _serviceContext = serviceContext;
        }

        [HttpPost(Name = "InsertAdministrador")]
        public IActionResult Post([FromBody] Administrador administrador)
        {
            try
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

                    ////// Establece el token en una cookie y luego responde con el token
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false, // Para mayor seguridad, marca la cookie como httpOnly
                                          // Otras opciones de cookie si es necesario
                    });

                    return Ok(new { Token = token});

                    //return StatusCode(200, "Inicio de sesión exitoso");

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
                    // Otros claims si es necesario
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Duración del token
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
