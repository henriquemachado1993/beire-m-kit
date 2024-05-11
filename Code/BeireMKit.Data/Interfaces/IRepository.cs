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
        void Delete(int id);
        void Delete(T entity);
        void DeleteMultiple(ICollection<T> listEntity);
        T Update(T entity);
        T GetById(int id, string navigation = "");
        IQueryable<T> GetFiltered(QueryCriteria<T> query);
        IQueryable<T> GetPaged(QueryCriteria<T> query);
        int GetCount(QueryCriteria<T> query = null);
    }
}
