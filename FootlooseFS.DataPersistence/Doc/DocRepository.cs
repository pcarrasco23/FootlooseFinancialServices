using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using FootlooseFS.Models;

namespace FootlooseFS.DataPersistence
{
    public class DocRepository<T> : IRepository<T> where T : class
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

        public virtual void Add(T entity)
        {
            _collection.Insert(entity);
        }

        public virtual void AddBatch(IEnumerable<T> entities)
        {
            _collection.InsertBatch(entities);
        }

        public virtual void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(Expression<Func<T,int>> queryExpression, int id)
        {
            var query = Query<T>.EQ(queryExpression, id);
            _collection.Remove(query);
        }

        public virtual void Update(T entity)
        {
            _collection.Save(entity);
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _collection.AsQueryable<T>();
        }

        #endregion
    }
}
