using Microsoft.EntityFrameworkCore;
using BeireMKit.Data.Interfaces;
using BeireMKit.Domain.BaseModels;
using BeireMKit.Domain.Entity;

namespace BeireMKit.Data.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        protected readonly IDbContext _context;

        public Repository(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public T Add(T entity) => _context.Set<T>().Add(entity ?? throw new ArgumentNullException(nameof(entity))).Entity;

        public async Task<T> AddAsync(T entity) => (await _context.Set<T>().AddAsync(entity ?? throw new ArgumentNullException(nameof(entity))).ConfigureAwait(false)).Entity;

        public ICollection<T> AddMultiple(ICollection<T> listEntity)
        {
            if (listEntity == null || !listEntity.Any())
                throw new ArgumentNullException(nameof(listEntity));

            _context.Set<T>().AddRange(listEntity);

            return listEntity;
        }

        public async Task<ICollection<T>> AddMultipleAsync(ICollection<T> listEntity)
        {
            if (listEntity == null || !listEntity.Any())
                throw new ArgumentNullException(nameof(listEntity));

            await _context.Set<T>().AddRangeAsync(listEntity).ConfigureAwait(false);
            return listEntity;
        }

        public T Update(T entity)
        {
            if (entity != null)
            {
                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            return entity;
        }

        public void Delete(int id)
        {
            var _entity = GetById(id);
            if (_entity != null)
                _context.Set<T>().Remove(_entity);
        }

        public void Delete(T entity)
        {
            if (entity != null)
                _context.Set<T>().Remove(entity);
        }

        public void DeleteMultiple(ICollection<T> listEntity)
        {
            if (listEntity != null && listEntity.Any())
                _context.Set<T>().RemoveRange(listEntity);
        }

        public T GetById(int id, string navigation = "")
        {
            IQueryable<T> result = _context.Set<T>().Where(x => x.Id == id).AsNoTracking();

            if (result == null || !result.Any())
                return null;

            if (!string.IsNullOrEmpty(navigation))
                result = navigation.Split(',').Aggregate(result, (current, nav) => current.Include(nav));

            return result.FirstOrDefault();
        }

        public int GetCount(QueryCriteria<T> query = null)
        {
            query ??= new QueryCriteria<T>();
            query.Expression ??= x => x.Id != 0;
            return _context.Set<T>().Count(query.Expression);
        }

        public IQueryable<T> GetFiltered(QueryCriteria<T> query)
        {
            if (query == null)
                return Enumerable.Empty<T>().AsQueryable();

            query.Expression ??= x => x.Id != 0;
            query.OrderBy ??= x => x.Id;

            var filteredQuery = _context.Set<T>()
                .Where(query.Expression)
                .AsNoTracking();

            if (query.Ascending)
                filteredQuery = filteredQuery.OrderBy(query.OrderBy);
            else
                filteredQuery = filteredQuery.OrderByDescending(query.OrderBy);

            if (query.Navigation != null && query.Navigation.Any())
                foreach (var nav in query.Navigation)
                    filteredQuery = filteredQuery.Include(nav);

            return filteredQuery;
        }

        public IQueryable<T> GetPaged(QueryCriteria<T> query)
        {
            if (query == null)
                return Enumerable.Empty<T>().AsQueryable();

            query.Expression ??= x => x.Id != 0;
            query.OrderBy ??= x => x.Id;

            int skip = (query.Offset - 1) * query.Limit;

            IQueryable<T> filteredQuery = _context.Set<T>()
                .Where(query.Expression)
                .AsNoTracking();

            if (query.Ascending)
                filteredQuery = filteredQuery.OrderBy(query.OrderBy);
            else
                filteredQuery = filteredQuery.OrderByDescending(query.OrderBy);

            if (query.Navigation != null && query.Navigation.Any())
            {
                foreach (var nav in query.Navigation)
                {
                    filteredQuery = filteredQuery.Include(nav);
                }
            }

            return filteredQuery.Skip(skip).Take(query.Limit);
        }
    }
}
