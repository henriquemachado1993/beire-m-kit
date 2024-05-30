using BeireMKit.Domain.BaseModels;
using BeireMKit.Domain.Entity;

namespace BeireMKit.Data.Interfaces
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        ICollection<T> AddMultiple(ICollection<T> listEntity);
        Task<ICollection<T>> AddMultipleAsync(ICollection<T> listEntity);
        T Update(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(int id);
        Task DeleteAsync(int id);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        void DeleteMultiple(ICollection<T> listEntity);
        T GetById(int id, string navigation = "");
        Task<T?> GetByIdAsync(int id, string navigation = "");
        IQueryable<T> GetFiltered(QueryCriteria<T> query);
        IQueryable<T> GetPaged(QueryCriteria<T> query);
        int GetCount(QueryCriteria<T> query = null);
        Task<int> GetCountAsync(QueryCriteria<T> query = null);
    }
}
