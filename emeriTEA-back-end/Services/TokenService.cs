using emeriTEA_back_end.IServices;
using Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace emeriTEA_back_end.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int? ExtractUserIdFromToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Cookies["jwtToken"];


            Console.WriteLine("Token received: " + token);

            if (token != null)
            {
                var userId = DecodeUserIdFromToken(token);
                return userId;
            }
            return null;
        }

        public int? ExtractUserIdFromAuthorizationHeader(HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var userId = DecodeUserIdFromToken(token);
                return userId;
            }

            return null;
        }


        private int? DecodeUserIdFromToken(string token)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"])),
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();


                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            catch (SecurityTokenExpiredException ex)
            {

                Console.WriteLine("The token has expired: " + ex.Message);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {

                Console.WriteLine("Token signature is invalid: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error decoding or validating token: " + ex.Message);
            }

            return null;
        }
    }
}
