using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public (string Token, DateTime ExpiresAt) GenerateToken(ApplicationUser user, IList<string> roles)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"]!;
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiryMinutes = int.Parse( _configuration["JwtSettings:ExpiryTimeInMinutes"]!);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub , user.Id),
                new(JwtRegisteredClaimNames.Email , user.Email!), 
                new(ClaimTypes.NameIdentifier , user.Id), 
                new("fullName" , user.FullName),
                new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())

            };


            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var token = new JwtSecurityToken
                (
                issuer : issuer,
                audience : audience,
                claims : claims, 
                expires : expiresAt ,
                signingCredentials : credentials                            
                );


            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expiresAt);
        }
    }
}
