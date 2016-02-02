using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using FootlooseFS.Models;

namespace FootlooseFS.DataPersistence
{
    public abstract class DocRepository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        private MongoDatabase _database;
        private string _tableName;
        private MongoCollection<T> _collection;

        public DocRepository(MongoDatabase database, string tableName)
        {
            _database = database;
            _tableName = tableName;

            _collection = _database.GetCollection<T>(tableName);
        }

        #region IRepository<T> Members

        public override void Add(T entity)
        {
            _collection.Insert(entity);
        }

        public override void AddBatch(IEnumerable<T> entities)
        {
            _collection.InsertBatch(entities);
        }

        public override void Delete(Expression<Func<T,int>> queryExpression, int id)
        {
            var query = Query<T>.EQ(queryExpression, id);
            _collection.Remove(query);          
        }

        public override void DeleteAll()
        {
            _collection.RemoveAll();
        }

        public override T Update(T entity)
        {
            _collection.Save(entity);

            return entity;
        }

        public override IQueryable<T> GetQueryable()
        {
            return _collection.AsQueryable<T>();
        }              

        #endregion
    }
}
