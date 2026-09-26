using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ECommerceAfaq.Application.Interfaces
{
    public interface IGenericRpository<T>  where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities); 
        void DeleteAsync(T entity);
        void UpdateAsync(T entity);
        Task<bool> ExistesAsync(Expression<Func<T, bool>> predicate);


    }
}
