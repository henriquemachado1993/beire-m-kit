using BeireMKit.Domain.BaseModels;
using System.Linq.Expressions;

namespace BeireMKit.Data.Interfaces.MongoDB
{
    public interface IMongoRepository<T>

    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> filterExpression);
        Task<PagingResult<List<T>>> FindPagedAsync(QueryCriteria<T> queryCriteria);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
