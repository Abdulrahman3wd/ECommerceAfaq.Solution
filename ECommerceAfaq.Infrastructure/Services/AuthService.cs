using ECommerceAfaq.Application.DTOs.Auth;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(ILogger<AuthService> logger,
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _tokenService = tokenService;
            _userManager = userManager;
        }
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return; 

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            _logger.LogInformation($"Password rese token  for Email: Token", email, resetToken);

        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email); // email 

            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new ArgumentException("Invalid email or password");

            var roles =  await _userManager.GetRolesAsync(user);

            var (token, expiresAt) = _tokenService.GenerateToken(user, roles);


            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                Token = token,
                ExpireAt = expiresAt
            };

        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
                throw new ArgumentException("Email is already registerd");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName

            };

            var result = await _userManager.CreateAsync(user , dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description)); 
                throw new ArgumentException(errors);
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            var roles = await _userManager.GetRolesAsync(user);

            var (token, expiresAt) = _tokenService.GenerateToken(user, roles);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Token = token,
                ExpireAt = expiresAt
            };
        }

        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new ArgumentException("Invalid Request");

            var result = await _userManager.ResetPasswordAsync(user , token, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ArgumentException(errors);
            }


        }
    }
}
