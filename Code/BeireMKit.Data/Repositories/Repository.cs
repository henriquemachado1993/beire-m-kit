using BeireMKit.Data.Interfaces;
using BeireMKit.Domain.BaseModels;
using BeireMKit.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace BeireMKit.Data.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        protected readonly IBaseDbContext _context;

        public Repository(IBaseDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public T Add(T entity) => _context.Set<T>().Add(entity ?? throw new ArgumentNullException(nameof(entity))).Entity;

        public async Task<T> AddAsync(T entity, CancellationToken token = default) => (await _context.Set<T>().AddAsync(entity ?? throw new ArgumentNullException(nameof(entity)), token).ConfigureAwait(false)).Entity;

        public ICollection<T> AddMultiple(ICollection<T> listEntity)
        {
            if (listEntity == null || !listEntity.Any())
                throw new ArgumentNullException(nameof(listEntity));

            _context.Set<T>().AddRange(listEntity);

            return listEntity;
        }

        public async Task<ICollection<T>> AddMultipleAsync(ICollection<T> listEntity, CancellationToken token = default)
        {
            if (listEntity == null || !listEntity.Any())
                throw new ArgumentNullException(nameof(listEntity));

            await _context.Set<T>().AddRangeAsync(listEntity, token).ConfigureAwait(false);
            return listEntity;
        }

        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

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

        /// <summary>
        /// navigation: "navigation1,navigation2"
        /// </summary>
        public T GetById(int id, string navigation = "")
        {
            IQueryable<T> filteredQuery = _context.Set<T>().Where(x => x.Id == id).AsNoTracking();

            if (filteredQuery == null || !filteredQuery.Any())
                return null;

            if (!string.IsNullOrEmpty(navigation))
            {
                var navigations = navigation.Split(',')?.ToList();
                IncludeNavigation(navigations, ref filteredQuery);
            }
            
            return filteredQuery.FirstOrDefault();
        }

        public async Task<T?> GetByIdAsync(int id, string navigation = "", CancellationToken token = default)
        {
            IQueryable<T> filteredQuery = _context.Set<T>().Where(x => x.Id == id).AsNoTracking();

            if (filteredQuery == null || !filteredQuery.Any())
                return null;

            if (!string.IsNullOrEmpty(navigation))
            {
                var navigations = navigation.Split(',')?.ToList();
                IncludeNavigation(navigations, ref filteredQuery);
            }

            return await filteredQuery.FirstOrDefaultAsync(token);
        }

        public int GetCount(QueryCriteria<T> query = null)
        {
            query ??= new QueryCriteria<T>();
            query.Expression ??= x => true;
            return _context.Set<T>().Count(query.Expression);
        }

        public async Task<int> GetCountAsync(QueryCriteria<T> query = null, CancellationToken token = default)
        {
            query ??= new QueryCriteria<T>();
            query.Expression ??= x => true;
            return await _context.Set<T>().CountAsync(query.Expression, token);
        }

        public IQueryable<T> GetFiltered(QueryCriteria<T> query)
        {
            if (query == null)
                return Enumerable.Empty<T>().AsQueryable();

            query.Expression ??= x => true;
            query.OrderBy ??= x => x.Id;

            var filteredQuery = _context.Set<T>()
                .Where(query.Expression)
                .AsNoTracking();

            if (query.Ascending)
                filteredQuery = filteredQuery.OrderBy(query.OrderBy);
            else
                filteredQuery = filteredQuery.OrderByDescending(query.OrderBy);

            IncludeNavigation(query.Navigation, ref filteredQuery);

            return filteredQuery;
        }

        public IQueryable<T> GetPaged(QueryCriteria<T> query)
        {
            if (query == null)
                return Enumerable.Empty<T>().AsQueryable();

            query.Expression ??= x => true;
            query.OrderBy ??= x => x.Id;

            int skip = (query.Offset - 1) * query.Limit;

            IQueryable<T> filteredQuery = _context.Set<T>()
                .Where(query.Expression)
                .AsNoTracking();

            if (query.Ascending)
                filteredQuery = filteredQuery.OrderBy(query.OrderBy);
            else
                filteredQuery = filteredQuery.OrderByDescending(query.OrderBy);

            IncludeNavigation(query.Navigation, ref filteredQuery);
            
            return filteredQuery.Skip(skip).Take(query.Limit);
        }

        private void IncludeNavigation(List<string>? navigations, ref IQueryable<T> query)
        {
            if (navigations == null || !navigations.Any())
                return;

            foreach (var navigation in navigations)
            {
                var properties = navigation.Split('.');
                IQueryable<T> includeQuery = query.Include(properties[0]);

                for (int i = 1; i < properties.Length; i++)
                {
                    includeQuery = ApplyThenInclude(includeQuery, properties[i]);
                }

                query = includeQuery;
            }
        }

        private IQueryable<T> ApplyThenInclude(IQueryable<T> query, string navigationProperty)
        {
            var method = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(m => m.Name == "ThenInclude" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), typeof(object));

            return (IQueryable<T>)method.Invoke(null, new object[] { query, navigationProperty });
        }
    }
}
