using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Domain.Entities;
using ECommerceAfaq.Infrastructure.Repositories;
using ECommerceAfaq.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Presistenece
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        private IDbContextTransaction? _currentTransaction;
        private IGenericRpository<Category>? _categories;

        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        private IProductRepository? _products;

        private ICartRepository? _carts;
        public UnitOfWork(AppDbContext dbContext)
        {
           _dbContext = dbContext;
        }

        public IGenericRpository<Category> Categories =>
            _categories ??= new GenericRepository<Category>(_dbContext);

        public IProductRepository Products =>
             _products ??= new ProductRepository(_dbContext);

        public ICartRepository Carts =>
            _carts ??= new CartRepository(_dbContext);

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IGenericRpository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (_repositories.TryGetValue(type, out var esistingRepo))
                return (IGenericRpository<T>) esistingRepo;

            var repo = new GenericRepository<T>(_dbContext);

            _repositories[type] = repo; 
             return repo;



        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }




        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _currentTransaction!.CommitAsync();
            }
            finally
            {
                await _currentTransaction!.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _currentTransaction!.RollbackAsync();
            }
            finally
            {
                await _currentTransaction!.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}

