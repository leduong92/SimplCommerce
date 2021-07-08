using System.Collections.Generic;
using System.Threading.Tasks;
using SimplCommerce.Infrastructure.Models;

namespace SimplCommerce.Infrastructure.Data
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}