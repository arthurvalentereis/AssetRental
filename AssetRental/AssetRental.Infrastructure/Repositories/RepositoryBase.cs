using Microsoft.EntityFrameworkCore;
using AssetRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity, TId>
            where TEntity : EntityBase
            where TId : struct
    {
        private readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }
        public TEntity Add(TEntity entidade)
        {
            _context.Set<TEntity>().Add(entidade);
            return entidade;
        }
        public IEnumerable<TEntity> AddList(IEnumerable<TEntity> entidades)
        {
            _context.Set<TEntity>().AddRange(entidades);
            return entidades;
        }
        public void Delete(TEntity entidade)
        {
            _context.Set<TEntity>().Remove(entidade);
        }
        public TEntity Update(TEntity entidade)
        {
            _context.Entry(entidade).State = EntityState.Modified;
            return entidade;
        }
        public bool Exists(Func<TEntity, bool> where) => _context.Set<TEntity>().Any(where);
        public IQueryable<TEntity> ListWhere(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties) => List(includeProperties).Where(where);
        public IQueryable<TEntity> ListWhereAndSortedBy<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntity, object>>[] includeProperties) =>
         ascendente ? ListWhere(where, includeProperties).OrderBy(ordem) : ListWhere(where, includeProperties).OrderByDescending(ordem);
        public IQueryable<TEntity> ListSortedBy<TKey>(Expression<Func<TEntity, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntity, object>>[] includeProperties)
            => ascendente ? List(includeProperties).OrderBy(ordem) : List(includeProperties).OrderByDescending(ordem);
        public TEntity GetBy(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] includeProperties) => List(includeProperties).FirstOrDefault(where);
        public TEntity GetById(TId id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (includeProperties.Any()) return List(includeProperties).FirstOrDefault(x => x.Id.ToString() == id.ToString());
            return _context.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> List(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (includeProperties.Any()) return Include(_context.Set<TEntity>(), includeProperties).AsSplitQuery();
            return query;
        }
        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            foreach (var property in includeProperties) query = query.Include(property);
            return query;
        }


        //asynchronous
        public async Task<List<TEntity>> ListWhereAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties) =>
             await List(includeProperties).Where(where).ToListAsync();

        public async Task<List<TEntity>> ListWhereAndSortedByAsync<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return ascendente ? await ListWhere(where, includeProperties).OrderBy(ordem).ToListAsync()
                               : await ListWhere(where, includeProperties).OrderByDescending(ordem).ToListAsync();
        }
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where) => await _context.Set<TEntity>().AnyAsync(where);

        public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties) => await List(includeProperties).FirstOrDefaultAsync(where);

        public async Task<TEntity> GetByIdAsync(TId id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (includeProperties.Any()) return List(includeProperties).FirstOrDefault(x => x.Id.ToString() == id.ToString());
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> UpdateAsync(TEntity entidade)
        {
            _context.Entry(entidade).State = EntityState.Modified;
            return entidade;
        }
        public void Dispose() => _context.Dispose();


    }
}
