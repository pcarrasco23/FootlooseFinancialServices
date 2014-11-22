using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.Models
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetQueryable();
        void Add(T entity);
        void AddBatch(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, int>> queryExpression, int id);
        void DeleteAll();
    }
}
