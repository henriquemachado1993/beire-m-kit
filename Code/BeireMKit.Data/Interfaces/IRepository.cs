using BeireMKit.Domain.BaseModels;
using BeireMKit.Domain.Entity;

namespace BeireMKit.Data.Interfaces
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        T Add(T entity);
        Task<T> AddAsync(T entity, CancellationToken token = default);
        ICollection<T> AddMultiple(ICollection<T> listEntity);
        Task<ICollection<T>> AddMultipleAsync(ICollection<T> listEntity, CancellationToken token = default);
        T Update(T entity);
        void Delete(int id);
        void Delete(T entity);
        void DeleteMultiple(ICollection<T> listEntity);
        T GetById(int id, string navigation = "");
        Task<T?> GetByIdAsync(int id, string navigation = "", CancellationToken token = default);
        IQueryable<T> GetFiltered(QueryCriteria<T> query);
        IQueryable<T> GetPaged(QueryCriteria<T> query);
        int GetCount(QueryCriteria<T> query = null);
        Task<int> GetCountAsync(QueryCriteria<T> query = null, CancellationToken token = default);
    }
}
