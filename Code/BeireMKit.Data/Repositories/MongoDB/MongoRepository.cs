﻿using BeireMKit.Data.Interfaces.Entity;
using BeireMKit.Data.Interfaces.MongoDB;
using BeireMKit.Domain.BaseModels;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace BeireMKit.Data.Repositories.MongoDB
{
    public class MongoRepository<T> : IMongoRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IBaseMongoDbContext context)
        {
            _collection = context.Database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _collection.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            Expression<Func<T, bool>> filterExpression = entity => ((IBaseEntity)entity).Id == id;
            var result = await _collection.Find(filterExpression).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filterExpression)
        {
            var result = await _collection.Find(filterExpression).ToListAsync();
            return result;
        }

        public async Task<PagingResult<List<T>>> FindPagedAsync(QueryCriteria<T> queryCriteria)
        {
            var totalCount = await _collection.CountDocumentsAsync(queryCriteria.Expression);
            int skip = (queryCriteria.Offset - 1) * queryCriteria.Limit;
            var result = await _collection.Find(queryCriteria.Expression)
                .Skip(skip)
                .Limit(queryCriteria.Limit)
                .ToListAsync();

            return PagingResult<List<T>>.CreateValidResultPaging(result, new PageResult()
            {
                Limit = queryCriteria.Limit,
                Offset = queryCriteria.Offset,
                TotalCount = (int)totalCount
            });
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            // Assuming that the entity has a field called “Id”
            var id = (Guid?)entity?.GetType()?.GetProperty("Id")?.GetValue(entity, null);
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
        }
    }
}
