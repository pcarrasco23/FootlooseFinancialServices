using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using FootlooseFS.Models;

namespace FootlooseFS.DataPersistence
{
    public class SqlRepository<T> : BaseRepository<T>, IRepository<T> where T : class
    {
        protected DbContext _dbContext;

        public SqlRepository(DbContext dbContext)
        {
            // Disable proxy creation
            dbContext.Configuration.ProxyCreationEnabled = false;

            _dbContext = dbContext;
        }

        #region IRepository<T> Members

        public override void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);           
        }

        public override void AddBatch(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        public override void Delete(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
        }

        public override void Delete(Expression<Func<T, int>> queryExpression, int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public override T Update(T entity)
        {
            _dbContext.Entry<T>(entity).State = EntityState.Modified;

            return entity;
        }

        public override IQueryable<T> GetQueryable()
        {
            return _dbContext.Set<T>();
        }

        #endregion
    }
}
