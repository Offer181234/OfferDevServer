using Interface.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<Guid> CreateAsync(T entity);

        Task<T> FindByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<ClientDto> FindClientClaimsByIdAsync(Guid id);
    }
}