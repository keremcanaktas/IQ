using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleWebApplication.Repositories
{
    public abstract class EfCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>
    {
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        protected EfCoreRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public virtual IQueryable<TEntity> Query(bool trackChanges = false)
        {
            var query = DbSet.AsQueryable();
            if (!trackChanges)
                query = query.AsNoTracking();
            return query;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        protected virtual async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(e => EF.Property<TKey>(e, "Id").Equals(id), cancellationToken);
        }
    }
} 