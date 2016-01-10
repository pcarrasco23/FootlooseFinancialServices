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
        PageOfList<T> Search(int pageNumber, int numRecsPerPage, T searchCriteria, string sort, SortDirection sortDirection);
        void Add(T entity);
        void AddBatch(IEnumerable<T> entities);
        T Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, int>> queryExpression, int id);
        void DeleteAll();
    }
}
