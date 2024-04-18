using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AssetRental.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity, in TId> : IDisposable where TEntity : class where TId : struct
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        bool Exists(Func<TEntity, bool> where);
        IEnumerable<TEntity> AddList(IEnumerable<TEntity> entity);
        IQueryable<TEntity> List(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> ListWhere(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> ListWhereAndSortedBy<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> ListSortedBy<TKey>(Expression<Func<TEntity, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetBy(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(TId id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> ListWhereAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<List<TEntity>> ListWhereAndSortedByAsync<TKey>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TKey>> ordem, bool ascendente = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where);
        Task<TEntity> UpdateAsync(TEntity entidade);
        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(TId id, params Expression<Func<TEntity, object>>[] includeProperties);

    }
}
