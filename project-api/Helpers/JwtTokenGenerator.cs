using Microsoft.IdentityModel.Tokens;
using project_api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace project_api.Helpers
{
    public class JwtTokenGenerator
    {
        public string GetToken(Departamentos dep)
        {
            List<Claim> claims = new List<Claim>();
            
            claims.Add(new Claim(ClaimTypes.Name, dep.Nombre));
            claims.Add(new Claim("Id", dep.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, "Saludos"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "prueba"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(5).ToString()));
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = "Saludos",
                Audience = "prueba",
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(5),
                NotBefore = DateTime.Now.AddMinutes(-1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8
            .GetBytes("ESTAESMILLAVEDECIFRADODELTOKEN2024")), SecurityAlgorithms.HmacSha256)
            };


            return handler.CreateEncodedJwt(token);
        }
    }
}
