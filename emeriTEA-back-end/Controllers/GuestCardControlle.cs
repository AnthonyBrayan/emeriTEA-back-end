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

    public class GuestCardControlle : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IGuestCardService _guestCardService;
        private readonly ITokenService _tokenService;
        private readonly ServiceContext _serviceContext;

        public GuestCardControlle(IConfiguration configuration, ITokenService tokenService, IGuestCardService guestCardService, ServiceContext serviceContext)
        {
            _configuration = configuration;
            _guestCardService = guestCardService;
            _tokenService = tokenService;
            _serviceContext = serviceContext;
        }


        [HttpPost(Name = "InsertGuestCart")]
        public IActionResult Post([FromBody] GuestCart guestCart)
        {
            try
            {
                var userId = _tokenService.ExtractUserIdFromToken(HttpContext);
                //var userId = _tokenService.ExtractUserIdFromAuthorizationHeader(HttpContext);
                if (userId == null)
                {

                    var guest = new Guest
                    {
                        Token = "",
                        Name_guest = string.Empty,
                        Adress = string.Empty,
                        Phone = string.Empty
                    };


                    _serviceContext.Guest.Add(guest);
                    _serviceContext.SaveChanges();

                    var token = GenerateJwtToken(guest);
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = false,
                    });

                    guest.Token = token;


                    _serviceContext.SaveChanges();


                    guestCart.Id_guest = guest.Id_guest;

                    _guestCardService.AddGuestCart(guestCart);

                    return Ok(new { message = "Producto agregado al carrito exitosamente" });
                }
                else
                {
                    var userIdInt = _tokenService.ExtractUserIdFromToken(HttpContext);
                    //var idUser = _tokenService.ExtractUserIdFromAuthorizationHeader(HttpContext);

                    guestCart.Id_guest = (int)userIdInt;

                    _guestCardService.AddGuestCart(guestCart);

                    return Ok(new { message = "Producto agregado al carrito exitosamente" });


                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting User ID: {ex.Message}");
            }
        }



        private string GenerateJwtToken(Guest guest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, guest.Id_guest.ToString()),
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
