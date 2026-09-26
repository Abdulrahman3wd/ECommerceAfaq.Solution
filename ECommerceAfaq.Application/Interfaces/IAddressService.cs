using ECommerceAfaq.Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IAddressService
    {
        Task<List<AddressResponseDto>> GetUserAddressesAsync(string userId);
        Task<AddressResponseDto> AddAsync(string userId , AddressCreateDto dto);
        Task<bool> UpdateAsync(string userId, int addressId, AddressCreateDto dto);
        Task<bool> DeleteAsync(string userId, int addressId);
        Task<bool> SetDefaultAsync(string userId, int addressId);



    }
}
