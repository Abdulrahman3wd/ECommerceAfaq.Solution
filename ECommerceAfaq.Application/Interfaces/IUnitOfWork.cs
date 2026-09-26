using ECommerceAfaq.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRpository<Category> Categories  { get; }
        IProductRepository Products { get; }
        ICartRepository Carts { get; }


        IGenericRpository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();


    }
}
