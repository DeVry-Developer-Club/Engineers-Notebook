using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using EngineerNotebook.Core.Interfaces;
using EngineerNotebook.Shared;
using EngineerNotebook.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EngineerNotebook.Infrastructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        protected readonly EngineerDbContext DbContext;

        public EfRepository(EngineerDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var keyValues = new object[] { id };
            return await DbContext.Set<T>().FindAsync(keyValues, cancellationToken);
        }

        public virtual async Task<T> GetByIdAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.CountAsync(cancellationToken);
        }

        public async Task<T> FirstAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.FirstAsync(cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(DbContext.Set<T>().AsQueryable(), spec);
        }
    }
}