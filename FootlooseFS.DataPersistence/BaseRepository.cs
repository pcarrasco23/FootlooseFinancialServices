using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootlooseFS.DataPersistence
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        public virtual void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void AddBatch(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(System.Linq.Expressions.Expression<Func<T, int>> queryExpression, int id)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            throw new NotImplementedException();
        }        

        public virtual PageOfList<T> Search(int pageNumber, int numRecordsInPage, T searchCriteria, string sort, SortDirection sortDirection)
        {
            // Determine the starting row from the pageNumber and numRecordsInPage
            int startRow;

            if (numRecordsInPage == -1)
                startRow = 0;
            else
                startRow = (pageNumber - 1) * numRecordsInPage;

            // Apply the filters and sort
            var queryable = ApplyFilter(searchCriteria, sort, sortDirection);

            // Get the number of records
            int recordCount = queryable.Count();

            // Apply the paging
            List<T> entities;
            if (numRecordsInPage != -1)
                entities = queryable.Skip(startRow)
                                                .Take(numRecordsInPage)
                                                .ToList();
            else
                entities = queryable.ToList();

            return new PageOfList<T>(entities, pageNumber, numRecordsInPage, recordCount);
        }

        public virtual T Update(T entity)
        {
            throw new NotImplementedException();
        }

        protected virtual IOrderedQueryable<T> ApplyFilter(T searchCriteria, string sort, SortDirection sortdirection)
        {
            throw new NotImplementedException();
        }
    }
}
