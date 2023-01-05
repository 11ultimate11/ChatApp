
using ChatAppAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatAppAPI.Helpers
{
    public class JwtService : IJwtService
    {
        private readonly static byte[] IV = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public CredentialsModel Credentials(ClaimsIdentity claims)
        {
            var username = claims.FindFirst(o => o.Type == ClaimTypes.Email)?.Value;
            var password = claims.FindFirst(o => o.Type == ClaimTypes.Hash)?.Value;
            var personID = claims.FindFirst(o => o.Type == ClaimTypes.Actor)?.Value;
            return new CredentialsModel()
            {
                Username = username,
                Password = password,
                PersonID = int.Parse(personID!),
            };
        }
        public string GenerateToken(CredentialsModel loginCredentials)
        {
            if (_configuration is null) return string.Empty;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration!["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, loginCredentials.Username!),
                new Claim(ClaimTypes.Hash, loginCredentials.Password!.ToString()),
                new Claim(ClaimTypes.Actor, loginCredentials.PersonID.ToString()),
            };
            //TODO token expiration time back to 1 min or less
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(35),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
