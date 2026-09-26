using ECommerceAfaq.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);

        Task ForgotPasswordAsync(string email); 
        Task ResetPasswordAsync(string email , string token  , string newPassword);

    }
}
