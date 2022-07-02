using Document_Request.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Document_Request.Static
{
    public static class TokenGenerator
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(User user, Login request)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        public static string CreateToken(User user, IConfiguration config)
        {
            List<Claim> claims = new List<Claim>
      {
         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
         new Claim(ClaimTypes.Email, user.Username),
         new Claim(ClaimTypes.GivenName, user.FirstName),
         new Claim(ClaimTypes.Surname, user.LastName),
         new Claim(ClaimTypes.Role, user.Role),
         new Claim(ClaimTypes.Gender, user.Sex)

      };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
               config.GetSection("JWT:Key").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                  issuer: config.GetSection("JWT:Issuer").Value,
                  audience: config.GetSection("JWT:Audience").Value,
                  claims: claims,
                  expires: DateTime.UtcNow.AddDays(5),
                  signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public static string CreateRandomPasswordToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(128));
        }
    }
}
