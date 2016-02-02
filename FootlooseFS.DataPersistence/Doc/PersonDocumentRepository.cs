using FootlooseFS.Models;
using MongoDB.Driver;
using System.Linq;

namespace FootlooseFS.DataPersistence.Doc
{
    public class PersonDocumentRepository : DocRepository<PersonDocument>
    {
        public PersonDocumentRepository(MongoDatabase database, string tableName) : base(database, tableName) { }

        protected override IOrderedQueryable<PersonDocument> ApplyFilter(PersonDocument searchCriteria, string sort, SortDirection sortDirection)
        {
            var queryable = GetQueryable();

            if (searchCriteria.PersonID > 0)
                queryable = queryable.Where(p => p.PersonID == searchCriteria.PersonID);

            if (!string.IsNullOrEmpty(searchCriteria.FirstName))
                queryable = queryable.Where(p => p.FirstName.ToLower().StartsWith(searchCriteria.FirstName));

            if (!string.IsNullOrEmpty(searchCriteria.LastName))
                queryable = queryable.Where(p => p.LastName.ToLower().StartsWith(searchCriteria.LastName));

            if (!string.IsNullOrEmpty(searchCriteria.EmailAddress))
                queryable = queryable.Where(p => p.EmailAddress.ToLower().StartsWith(searchCriteria.EmailAddress));

            if (!string.IsNullOrEmpty(searchCriteria.StreetAddress))
                queryable = queryable.Where(p => p.StreetAddress.ToLower().StartsWith(searchCriteria.StreetAddress));

            if (!string.IsNullOrEmpty(searchCriteria.City))
                queryable = queryable.Where(p => p.City.ToLower().StartsWith(searchCriteria.City));

            if (!string.IsNullOrEmpty(searchCriteria.Zip))
                queryable = queryable.Where(p => p.Zip.ToLower().StartsWith(searchCriteria.Zip));

            if (!string.IsNullOrEmpty(searchCriteria.State))
                queryable = queryable.Where(p => p.State.ToLower().StartsWith(searchCriteria.State));

            if (!string.IsNullOrEmpty(searchCriteria.PhoneNumber))
                queryable = queryable.Where(p => p.PhoneNumber.ToLower().StartsWith(searchCriteria.PhoneNumber));

            sort = sort.ToLower();

            IOrderedQueryable<PersonDocument> orderedQueryable;

            if (sortDirection == SortDirection.Ascending)
            {
                if (sort == "firstname")
                    orderedQueryable = queryable.OrderBy(p => p.FirstName);
                else if (sort == "lastname")
                    orderedQueryable = queryable.OrderBy(p => p.LastName);
                else if (sort == "emailaddress")
                    orderedQueryable = queryable.OrderBy(p => p.EmailAddress);
                else if (sort == "streetaddress")
                    orderedQueryable = queryable.OrderBy(p => p.StreetAddress);
                else if (sort == "city")
                    orderedQueryable = queryable.OrderBy(p => p.City);
                else if (sort == "zip")
                    orderedQueryable = queryable.OrderBy(p => p.Zip);
                else if (sort == "state")
                    orderedQueryable = queryable.OrderBy(p => p.State);
                else if (sort == "phonenumber")
                    orderedQueryable = queryable.OrderBy(p => p.PhoneNumber);
                else
                    orderedQueryable = queryable.OrderBy(p => p.PersonID);
            }                
            else
            {
                if (sort == "firstname")
                    orderedQueryable = queryable.OrderByDescending(p => p.FirstName);
                else if (sort == "lastname")
                    orderedQueryable = queryable.OrderByDescending(p => p.LastName);
                else if (sort == "emailaddress")
                    orderedQueryable = queryable.OrderByDescending(p => p.EmailAddress);
                else if (sort == "streetaddress")
                    orderedQueryable = queryable.OrderByDescending(p => p.StreetAddress);
                else if (sort == "city")
                    orderedQueryable = queryable.OrderByDescending(p => p.City);
                else if (sort == "zip")
                    orderedQueryable = queryable.OrderByDescending(p => p.Zip);
                else if (sort == "state")
                    orderedQueryable = queryable.OrderByDescending(p => p.State);
                else if (sort == "phonenumber")
                    orderedQueryable = queryable.OrderByDescending(p => p.PhoneNumber);
                else
                    orderedQueryable = queryable.OrderBy(p => p.PersonID);
            }

            return orderedQueryable;            
        }
    }
}
