using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RiwiTalent.Models;
using RiwiTalent.Services.Interface;

namespace RiwiTalent.Services.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string? key;
        private readonly string? Issuer;
        private readonly string? Audience;
        public TokenRepository()
        {
            key = Environment.GetEnvironmentVariable("Key") ?? throw new ArgumentNullException("Key");;
            Issuer = Environment.GetEnvironmentVariable("Issuer") ?? throw new ArgumentNullException("Audience");
            Audience = Environment.GetEnvironmentVariable("Audience") ?? throw new ArgumentNullException("Issuer");
        }

        public string GetToken(User user)
        {
           
            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var tokenOptions = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenOptions);
            var tokenString = tokenHandler.WriteToken(securityToken);

            return tokenString;
            
        }
    }
}