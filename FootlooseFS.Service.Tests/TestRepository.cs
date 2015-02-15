using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Service.Tests
{
    public class TestRepository<T> : IRepository<T> where T : class
    {
        private List<T> repository;

        public TestRepository()
        {
            repository = new List<T>();
        }

        #region IRepository<T> Members

        public virtual void Add(T entity)
        {
            repository.Add(entity);
        }

        public virtual void AddBatch(IEnumerable<T> entities)
        {
            repository.AddRange(entities);
        }

        public virtual void Delete(T entity)
        {
            repository.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, int>> queryExpression, int id)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteAll()
        {
            repository.Clear();
        }

        public virtual void Update(T entity)
        {
            repository.Remove(entity);
            repository.Add(entity);
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return repository.AsQueryable<T>();
        }

        #endregion
    }
}
