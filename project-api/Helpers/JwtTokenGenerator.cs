using Microsoft.IdentityModel.Tokens;
using project_api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace project_api.Helpers
{
    public class JwtTokenGenerator
    {
        
        private readonly IConfiguration configuration;
        public JwtTokenGenerator(IConfiguration conf)
        {
   
           
            configuration = conf;
        }
        public string GetToken(Departamentos dep)
        {
            var Iss = configuration["JwtSettings:Issuer"];
            var aud = configuration["JwtSettings:Audience"];
            var key = configuration["JwtSettings:SecretKey"];
            List<Claim> claims = new List<Claim>();
            
            claims.Add(new Claim(ClaimTypes.Name, dep.Nombre));
            claims.Add(new Claim("Id", dep.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, Iss));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, aud));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(5).ToString()));
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = Iss,
                Audience = aud,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(5),
                NotBefore = DateTime.Now.AddMinutes(-1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes(key)), SecurityAlgorithms.HmacSha256)
            };


            return handler.CreateEncodedJwt(token);
        }
    }
}
