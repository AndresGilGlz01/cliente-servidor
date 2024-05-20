using Microsoft.IdentityModel.Tokens;
using project_api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace project_api.Helpers
{
    public class JwtTokenGenerator
    {
        
        private readonly IConfiguration configuration;
        public JwtTokenGenerator(IConfiguration conf)
        {
   
           
            configuration = conf;
        }
        public string GetToken(Departamentos dep,string role)
        {
            var Iss = configuration["JwtSettings:Issuer"];
            var aud = configuration["JwtSettings:Audience"];
            var key = configuration["JwtSettings:SecretKey"];
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, dep.Nombre));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, dep.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, Iss));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, aud));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(5).ToString()));

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = Iss,
                Audience = aud,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddDays(7),
                NotBefore = DateTime.Now,
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}
