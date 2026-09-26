using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string UserId  { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName {  get; set; } = string.Empty;
        public string Token {  get; set; } = string.Empty;
        public DateTime ExpireAt {  get; set; }


    }
}
