using ECommerceAfaq.Application.Interfaces;
using ECommerceAfaq.Infrastructure.Presistenece;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ECommerceAfaq.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRpository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);

        }

        public void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistesAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return  await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return   await _dbSet.FindAsync(id);
        }

        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
