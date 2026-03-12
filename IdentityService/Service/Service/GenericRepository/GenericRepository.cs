//using Interface.Interface;
//using Microsoft.EntityFrameworkCore;
//using Service.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Threading.Tasks;

//namespace Service.GenericRepository
//{
//    public class GenericRepository<T> : IGenericRepository<T>, IDisposable
//        where T : class, IEntity, new()
//    {
//        private readonly IdentityDbContext _context;
//        private readonly DbSet<T> _dbSet;
//        private bool _disposed;

//        public GenericRepository(IdentityDbContext context)
//        {
//            _context = context;
//            _dbSet = _context.Set<T>();
//        }

//        public async Task<Guid> CreateAsync(T entity)
//        {
//            if (entity == null)
//                throw new ArgumentNullException(nameof(entity));

//            if (entity.Id == Guid.Empty)
//                entity.Id = Guid.NewGuid();

//            await _dbSet.AddAsync(entity);
//            await SaveChangesAsync();

//            return entity.Id;
//        }

//        public async Task<Guid> DeleteAsync(T entity)
//        {
//            if (entity == null)
//                throw new ArgumentNullException(nameof(entity));

//            _dbSet.Remove(entity);
//            await SaveChangesAsync();

//            return entity.Id;
//        }

//        public async Task<IList<T>> FindAllAsync(
//            Expression<Func<T, bool>>? filter = null,
//            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
//            string includeProperties = "")
//        {
//            IQueryable<T> query = _dbSet;

//            if (filter != null)
//                query = query.Where(filter);

//            foreach (var includeProperty in includeProperties
//                     .Split(',', StringSplitOptions.RemoveEmptyEntries))
//            {
//                query = query.Include(includeProperty);
//            }

//            if (orderBy != null)
//                query = orderBy(query);

//            return await query.AsNoTracking().ToListAsync();
//        }

//        public async Task<T?> FindByIdAsync(object id)
//        {
//            return await _dbSet.FindAsync(id);
//        }

//        public async Task<T?> FindOneAsync(
//            Expression<Func<T, bool>>? filter = null,
//            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
//            string includeProperties = "")
//        {
//            IQueryable<T> query = _dbSet;

//            if (filter != null)
//                query = query.Where(filter);

//            foreach (var includeProperty in includeProperties
//                     .Split(',', StringSplitOptions.RemoveEmptyEntries))
//            {
//                query = query.Include(includeProperty);
//            }

//            if (orderBy != null)
//                query = orderBy(query);

//            return await query.AsNoTracking().FirstOrDefaultAsync();
//        }

//        public async Task<Guid> UpdateAsync(T entity)
//        {
//            if (entity == null)
//                throw new ArgumentNullException(nameof(entity));

//            _dbSet.Update(entity);
//            await SaveChangesAsync();

//            return entity.Id;
//        }

//        private async Task SaveChangesAsync()
//        {
//            await _context.SaveChangesAsync();
//        }

//        public void Dispose()
//        {
//            if (!_disposed)
//            {
//                _context.Dispose();
//                _disposed = true;
//            }

//            GC.SuppressFinalize(this);
//        }


//        public async Task<Guid> GetClaimIdsByClaimNameAsync(Guid claimName)
//        {
//            var claim = await _context.Claims
//                .FirstOrDefaultAsync(c => c.Id == claimName);

//            return claim?.Id ?? Guid.Empty;
//        }
//    }
//}