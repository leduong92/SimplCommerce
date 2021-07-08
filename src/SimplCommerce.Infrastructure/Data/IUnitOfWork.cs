using System;
using System.Threading.Tasks;
using SimplCommerce.Infrastructure.Models;

namespace SimplCommerce.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : EntityBase;
        Task<int> Complete();
    }
}