
using System.Security.Claims;
using System.Text;
using System;
using System.Reflection.Emit;
using BlogApp.Core.Domain.Interfaces;
using BlogApp.Core.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BlogApp.Application.Services.TokenService
{
    public class TokenGenerator
    {
        private readonly IConfiguration _config;

        public TokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string ReturnToken(string secretKey, string issuer, string audience, double expirationMinutes, List<Claim> claims = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            DateTime dateTimeLocal = DateTime.UtcNow; // Preferible usar UTC
            DateTime dateExp = dateTimeLocal.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(issuer, audience, claims, dateTimeLocal, dateExp, credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
            }

            string secretKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("La clave secreta no está configurada.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>()
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
            // Puedes agregar más claims si es necesario
        };

            return ReturnToken(secretKey, _config["Jwt:Issuer"], _config["Jwt:Audience"], Convert.ToDouble(_config["Jwt:AccessTokenExpirationMinutes"]), claims);
        }
    }

}
