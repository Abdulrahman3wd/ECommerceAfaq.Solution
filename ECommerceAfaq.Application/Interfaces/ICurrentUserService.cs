using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? FullName { get; }

        bool IsAuthenticated { get; }
    }
}
