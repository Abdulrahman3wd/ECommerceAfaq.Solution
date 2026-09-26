using ECommerceAfaq.Application.DTOs.Address;
using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AddressResponseDto>> GetUserAddressesAsync(string userId)
        {
            var addresses = await _unitOfWork.Repository<Address>().FindAsync(a => a.UserId == userId);
            return addresses.Select(MapToResponseDto).ToList();

        }
        public async Task<AddressResponseDto> AddAsync(string userId, AddressCreateDto dto)
        {
            var address = new Address
            {
                UserId = userId,
                Title = dto.Title,
                FullName =dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                City = dto.City,
                Area = dto.Area,
                Apartment = dto.Apartment,
                Building = dto.Building,
                IsDefault = dto.IsDefault, // true
                Street = dto.Street
            };

            if(dto.IsDefault) // true
            {
                await ClearExistingDefaultAsync(userId);
            }

            else
            {
                var hasAnyAddress = await _unitOfWork.Repository<Address>().ExistesAsync(a => a.UserId == userId);

                if (!hasAnyAddress)
                    address.IsDefault = true;
            }




            await _unitOfWork.Repository<Address>().AddAsync(address);
            await _unitOfWork.SaveChangesAsync(); 

            return MapToResponseDto(address);



        }

        public async Task<bool> DeleteAsync(string userId, int addressId)
        {
            var address = await GetOwendAddressAsync(userId, addressId);
            if (address is null)
                return false; 

            _unitOfWork.Repository<Address>().DeleteAsync(address);

            await _unitOfWork.SaveChangesAsync();
            return true;

 

        }



        public async Task<bool> SetDefaultAsync(string userId, int addressId)
        {
            var address = await GetOwendAddressAsync(userId, addressId);
            if (address is null)
                return false;

            await ClearExistingDefaultAsync(userId); 

            address.IsDefault = true;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(string userId, int addressId, AddressCreateDto dto)
        {
            var adderss = await GetOwendAddressAsync(userId, addressId);

            if (adderss is null)
                return false; 


            adderss.Title = dto.Title;
            adderss.FullName = dto.FullName;
            adderss.PhoneNumber = dto.PhoneNumber;
            adderss.City = dto.City;
            adderss.Area = dto.Area;
            adderss.Street = dto.Street;
            adderss.Building = dto.Building;
            adderss.Apartment = dto.Apartment;


            if (dto.IsDefault && !adderss.IsDefault)
            {
                adderss.IsDefault = true;            
            }

            await _unitOfWork.SaveChangesAsync();
            return true;

        }


        private async Task<Address?> GetOwendAddressAsync(string userId , int addressId)
        {
            var address = await _unitOfWork.Repository<Address>().GetByIdAsync(addressId);

            return address  is null || address.UserId != userId ? null : address;
              
        }

        private async Task ClearExistingDefaultAsync(string userId)
        {
            var addresses = await _unitOfWork.Repository<Address>().FindAsync(a => a.UserId == userId);
           foreach(var address in addresses)
            {
                address.IsDefault = false;
                _unitOfWork.Repository<Address>().UpdateAsync(address);
            }
        }
        private static AddressResponseDto MapToResponseDto(Address address)
        {
            return new AddressResponseDto
            {
                Id = address.Id,
                Title = address.Title,
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                City = address.City,
                Area = address.Area,
                Street = address.Street,
                Building = address.Building,
                Apartment = address.Apartment,
                IsDefault = address.IsDefault
            };
        }
    }
}
