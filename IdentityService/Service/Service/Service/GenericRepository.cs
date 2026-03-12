using Interface.DTOs;
using Interface.Interface;
using Microsoft.EntityFrameworkCore;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IdentityDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(IdentityDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<Guid> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            var property = entity.GetType().GetProperty("Id");

            if (property != null)
                return (Guid)property.GetValue(entity);

            return Guid.Empty;
        }

        public async Task<T> FindByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task<ClientDto> FindClientClaimsByIdAsync(Guid id)
        {
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && c.Status == "Active");

            if (client == null)
                return null;
            var test = await _context.ClientClaims.ToListAsync();
            client.clientClaimDtos = await _context.ClientClaims
                .Where(cc => cc.ClientId == id)
                .ToListAsync();

            return client;
        }
    }
}
