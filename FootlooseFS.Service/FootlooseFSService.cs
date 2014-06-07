using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootlooseFS.DataPersistence;
using FootlooseFS.Models;
using System.ComponentModel.DataAnnotations;

namespace FootlooseFS.Service
{
    public class FootlooseFSService : IFootlooseFSService
    {
        public PageOfList<Person> SearchPersons(int pageNumber, PersonSearchColumn personSearchColumn, SortDirection sortDirection, int numRecordsInPage, Dictionary<PersonSearchColumn, string> searchCriteria)
        {
            // Search for persons using the SQL repository
            // Determine the starting row
            int startRow;
            int totalItemCount = 0;

            if (numRecordsInPage == -1)
                startRow = 0;
            else
                startRow = (pageNumber - 1) * numRecordsInPage;

            PageOfList<Person> searchResults = null;

            using (var unitOfWork = new FootlooseFSSqlUnitOfWork())
            {
                IQueryable<Person> personsQueryable = unitOfWork.Persons.GetQueryable();

                foreach (KeyValuePair<PersonSearchColumn, string> entry in searchCriteria)
                {
                    if (entry.Key == PersonSearchColumn.PersonID)
                    {
                        var personID = Int32.Parse(entry.Value);
                        personsQueryable = personsQueryable.Where(p => p.PersonID == personID);
                    }

                    if (entry.Key == PersonSearchColumn.FirstName)
                        personsQueryable = personsQueryable.Where(p => p.FirstName.StartsWith(entry.Value));

                    if (entry.Key == PersonSearchColumn.LastName)
                        personsQueryable = personsQueryable.Where(p => p.LastName.StartsWith(entry.Value));

                    if (entry.Key == PersonSearchColumn.EmailAddress)
                        personsQueryable = personsQueryable.Where(p => p.EmailAddress.StartsWith(entry.Value));

                    if (entry.Key == PersonSearchColumn.Phone)
                        personsQueryable = personsQueryable.Where(p => p.Phones.Any(h => h.PhoneTypeID == 1 && h.Number.StartsWith(entry.Value)));

                    if (entry.Key == PersonSearchColumn.City)
                        personsQueryable = personsQueryable.Where(p => p.Addresses.Any(pa => pa.AddressTypeID == 1 && pa.Address.City.StartsWith(entry.Value)));

                    if (entry.Key == PersonSearchColumn.County)
                        personsQueryable = personsQueryable.Where(p => p.Addresses.Any(pa => pa.AddressTypeID == 1 && pa.Address.County.StartsWith(entry.Value)));

                    if (entry.Key == PersonSearchColumn.State)
                        personsQueryable = personsQueryable.Where(p => p.Addresses.Any(pa => pa.AddressTypeID == 1 && pa.Address.State.StartsWith(entry.Value)));

                    if (entry.Key == PersonSearchColumn.StreetAddress)
                        personsQueryable = personsQueryable.Where(p => p.Addresses.Any(pa => pa.AddressTypeID == 1 && pa.Address.StreetAddress.StartsWith(entry.Value)));

                    if (entry.Key == PersonSearchColumn.Zip)
                        personsQueryable = personsQueryable.Where(p => p.Addresses.Any(pa => pa.AddressTypeID == 1 && pa.Address.Zip.StartsWith(entry.Value)));
                }

                // Include the related phones and addresses in the search query
                personsQueryable = personsQueryable.Include("Addresses.Address").Include("Phones");

                IOrderedQueryable<Person> personOrderedQueryable = null;

                // Apply the sorting using the requested sort column and direction
                if (sortDirection == SortDirection.Ascending)
                {
                    if (personSearchColumn == PersonSearchColumn.PersonID)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.PersonID);
                    else if (personSearchColumn == PersonSearchColumn.FirstName)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.FirstName);
                    else if (personSearchColumn == PersonSearchColumn.LastName)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.LastName);
                    else if (personSearchColumn == PersonSearchColumn.EmailAddress)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.EmailAddress);
                    else if (personSearchColumn == PersonSearchColumn.Phone)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.Phones.FirstOrDefault().Number);
                    else if (personSearchColumn == PersonSearchColumn.City)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.Addresses.FirstOrDefault().Address.City);
                    else if (personSearchColumn == PersonSearchColumn.County)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.Addresses.FirstOrDefault().Address.County);
                    else if (personSearchColumn == PersonSearchColumn.State)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.Addresses.FirstOrDefault().Address.State);
                    else if (personSearchColumn == PersonSearchColumn.StreetAddress)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.Addresses.FirstOrDefault().Address.StreetAddress);
                    else if (personSearchColumn == PersonSearchColumn.Zip)
                        personOrderedQueryable = personsQueryable.OrderBy(p => p.Addresses.FirstOrDefault().Address.Zip);
                }
                else
                {
                    if (personSearchColumn == PersonSearchColumn.PersonID)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.PersonID);
                    else if (personSearchColumn == PersonSearchColumn.FirstName)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.FirstName);
                    else if (personSearchColumn == PersonSearchColumn.LastName)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.LastName);
                    else if (personSearchColumn == PersonSearchColumn.EmailAddress)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.EmailAddress);
                    else if (personSearchColumn == PersonSearchColumn.Phone)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.Phones.FirstOrDefault().Number);
                    else if (personSearchColumn == PersonSearchColumn.City)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.Addresses.FirstOrDefault().Address.City);
                    else if (personSearchColumn == PersonSearchColumn.County)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.Addresses.FirstOrDefault().Address.County);
                    else if (personSearchColumn == PersonSearchColumn.State)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.Addresses.FirstOrDefault().Address.State);
                    else if (personSearchColumn == PersonSearchColumn.StreetAddress)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.Addresses.FirstOrDefault().Address.StreetAddress);
                    else if (personSearchColumn == PersonSearchColumn.Zip)
                        personOrderedQueryable = personsQueryable.OrderByDescending(p => p.Addresses.FirstOrDefault().Address.Zip);
                }

                // Get the number of records
                int recordCount = personOrderedQueryable.Count();

                // Apply the paging and make the SQL call
                List<Person> persons;
                if (numRecordsInPage != -1)
                    persons = personOrderedQueryable.Skip(startRow)
                                                    .Take(numRecordsInPage)
                                                    .ToList();
                else
                    persons = personOrderedQueryable.ToList();

                searchResults = new PageOfList<Person>(persons, pageNumber, numRecordsInPage, recordCount);
            }

            return searchResults;
        }

        public PageOfList<PersonDocument> SearchPersonDocuments(int pageNumber, PersonSearchColumn personSearchColumn, SortDirection sortDirection, int numRecordsInPage, Dictionary<PersonSearchColumn, string> searchCriteria)
        {
            // Search for persons using the Document DB repository
            // Determine the starting row
            int startRow;
            int totalItemCount = 0;

            if (numRecordsInPage == -1)
                startRow = 0;
            else
                startRow = (pageNumber - 1) * numRecordsInPage;

            PageOfList<PersonDocument> searchResults = null;

            var unitOfWork = new FootlooseFSDocUnitOfWork();
            
            IQueryable<PersonDocument> personsQueryable = unitOfWork.Persons.GetQueryable();

            foreach (KeyValuePair<PersonSearchColumn, string> entry in searchCriteria)
            {
                if (entry.Key == PersonSearchColumn.PersonID)
                {
                    var personID = Int32.Parse(entry.Value);
                    personsQueryable = personsQueryable.Where(p => p.PersonID == personID);
                }

                var searchValue = entry.Value.ToLower();

                if (entry.Key == PersonSearchColumn.FirstName)
                    personsQueryable = personsQueryable.Where(p => p.FirstName.ToLower().StartsWith(searchValue));

                if (entry.Key == PersonSearchColumn.LastName)
                    personsQueryable = personsQueryable.Where(p => p.LastName.ToLower().StartsWith(searchValue));

                if (entry.Key == PersonSearchColumn.EmailAddress)
                    personsQueryable = personsQueryable.Where(p => p.EmailAddress.ToLower().StartsWith(searchValue));

                if (entry.Key == PersonSearchColumn.Phone)
                    personsQueryable = personsQueryable.Where(p => p.PhoneNumber.ToLower().StartsWith(searchValue));

                if (entry.Key == PersonSearchColumn.City)
                    personsQueryable = personsQueryable.Where(p => p.City.ToLower().StartsWith(searchValue));

                if (entry.Key == PersonSearchColumn.County)
                    personsQueryable = personsQueryable.Where(p => p.County.ToLower().StartsWith(searchValue));

                if (entry.Key == PersonSearchColumn.State)
                    personsQueryable = personsQueryable.Where(p => p.State.ToLower().StartsWith(searchValue));

                if (entry.Key == PersonSearchColumn.StreetAddress)
                    personsQueryable = personsQueryable.Where(p => p.StreetAddress.ToLower().StartsWith(searchValue));

                if (entry.Key == PersonSearchColumn.Zip)
                    personsQueryable = personsQueryable.Where(p => p.Zip.ToLower().StartsWith(searchValue));
            }

            IOrderedQueryable<PersonDocument> personOrderedQueryable = null;

            // Apply the sorting using the requested sort column and direction
            if (sortDirection == SortDirection.Ascending)
            {
                if (personSearchColumn == PersonSearchColumn.PersonID)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.PersonID);
                else if (personSearchColumn == PersonSearchColumn.FirstName)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.FirstName);
                else if (personSearchColumn == PersonSearchColumn.LastName)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.LastName);
                else if (personSearchColumn == PersonSearchColumn.EmailAddress)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.EmailAddress);
                else if (personSearchColumn == PersonSearchColumn.Phone)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.PhoneNumber);
                else if (personSearchColumn == PersonSearchColumn.City)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.City);
                else if (personSearchColumn == PersonSearchColumn.County)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.County);
                else if (personSearchColumn == PersonSearchColumn.State)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.State);
                else if (personSearchColumn == PersonSearchColumn.StreetAddress)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.StreetAddress);
                else if (personSearchColumn == PersonSearchColumn.Zip)
                    personOrderedQueryable = personsQueryable.OrderBy(p => p.Zip);
            }
            else
            {
                if (personSearchColumn == PersonSearchColumn.PersonID)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.PersonID);
                else if (personSearchColumn == PersonSearchColumn.FirstName)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.FirstName);
                else if (personSearchColumn == PersonSearchColumn.LastName)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.LastName);
                else if (personSearchColumn == PersonSearchColumn.EmailAddress)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.EmailAddress);
                else if (personSearchColumn == PersonSearchColumn.Phone)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.PhoneNumber);
                else if (personSearchColumn == PersonSearchColumn.City)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.City);
                else if (personSearchColumn == PersonSearchColumn.County)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.County);
                else if (personSearchColumn == PersonSearchColumn.State)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.State);
                else if (personSearchColumn == PersonSearchColumn.StreetAddress)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.StreetAddress);
                else if (personSearchColumn == PersonSearchColumn.Zip)
                    personOrderedQueryable = personsQueryable.OrderByDescending(p => p.Zip);
            }

            // Get the number of records
            int recordCount = personOrderedQueryable.Count();

            // Apply the paging
            List<PersonDocument> persons;
            if (numRecordsInPage != -1)
                persons = personOrderedQueryable.Skip(startRow)
                                                .Take(numRecordsInPage)
                                                .ToList();
            else
                persons = personOrderedQueryable.ToList();

            return new PageOfList<PersonDocument>(persons, pageNumber, numRecordsInPage, recordCount);
        }

        public Person GetPerson(int personID, PersonIncludes personIncludes)
        {
            // Get the person data from the SQL repository
            Person person = null;

            using (var unitOfWork = new FootlooseFSSqlUnitOfWork())
            {
                var personQueryable = unitOfWork.Persons.GetQueryable().Where(p => p.PersonID == personID);

                if (personIncludes.Phones)
                    personQueryable = personQueryable.Include("Phones");

                if (personIncludes.Addressses)
                    personQueryable = personQueryable.Include("Addresses.Address");

                if (personIncludes.Accounts)
                {
                    personQueryable = personQueryable.Include("Accounts.Account")
                                                    .Include("Accounts.Account.AccountType");                    
                }   
                
                if (personIncludes.Login)                
                    personQueryable = personQueryable.Include("Login");                

                if (personIncludes.AccountTransactions)
                    personQueryable = personQueryable.Include("Accounts.Account.Transactions");

                person = personQueryable.First();
            }

            return person;
        }
       
        public OperationStatus InsertPerson(Person person)
        {      
            try
            {
                // Use the ValidationContext to validate the Product model against the product data annotations
                // before saving it to the database
                var validationContext = new ValidationContext(person, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(person, validationContext, validationResults, true);

                // If there any exception return them in the return result
                if (!isValid)
                {
                    OperationStatus opStatus = new OperationStatus();
                    opStatus.Success = false;

                    foreach (ValidationResult message in validationResults)
                    {
                        opStatus.Messages.Add(message.ErrorMessage);
                    }

                    return opStatus;
                }
                else
                {
                    // Otherwise connect to the data source using the db context and save the 
                    // person to the database
                    using (var unitOfWork = new FootlooseFSSqlUnitOfWork())
                    {                           
                        unitOfWork.Persons.Add(person);
                        unitOfWork.Commit();
                    }                    

                    return new OperationStatus { Success = true, Data = person };
                } 
            }
            catch (Exception e)
            {
                return OperationStatus.CreateFromException("Error inserting person.", e);
            }
        }

        public OperationStatus UpdatePerson(Person updatedPerson)
        {
            try
            {           
                // Use the ValidationContext to validate the Product model against the product data annotations
                // before saving it to the database
                var validationContext = new ValidationContext(updatedPerson, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(updatedPerson, validationContext, validationResults, true);

                // If there any exception return them in the return result
                if (!isValid)
                {
                    OperationStatus opStatus = new OperationStatus();
                    opStatus.Success = false;

                    foreach (ValidationResult message in validationResults)
                    {
                        opStatus.Messages.Add(message.ErrorMessage);
                    }

                    return opStatus;
                }
                else
                {
                    // Otherwise connect to the data source using the db context and save the 
                    // person to the database
                    Person person;

                    using (var unitOfWork = new FootlooseFSSqlUnitOfWork())
                    {
                        person = unitOfWork.Persons.GetQueryable().Where(p => p.PersonID == updatedPerson.PersonID)  
                                            .Include("Addresses.Address")
                                            .Include("Phones")
                                            .First();

                        person.FirstName = updatedPerson.FirstName;
                        person.LastName = updatedPerson.LastName;
                        person.EmailAddress = updatedPerson.EmailAddress;

                        updatePhone(person, updatedPerson, (int)PhoneTypes.Home);
                        updatePhone(person, updatedPerson, (int)PhoneTypes.Work);
                        updatePhone(person, updatedPerson, (int)PhoneTypes.Cell);

                        updateAddress(person, updatedPerson, (int)AddressTypes.Home, unitOfWork);
                        updateAddress(person, updatedPerson, (int)AddressTypes.Work, unitOfWork);
                        updateAddress(person, updatedPerson, (int)AddressTypes.Alternate, unitOfWork);

                        unitOfWork.Persons.Update(person);
                        unitOfWork.Commit();
                    }                    

                    return new OperationStatus { Success = true, Data = person };
                } 
            }
            catch (Exception e)
            {
                return OperationStatus.CreateFromException("Error updating person.", e);
            }
        }

        public OperationStatus DeletePerson(int personID)
        {
            try
            {
                using (var unitOfWork = new FootlooseFSSqlUnitOfWork())
                {
                    unitOfWork.Persons.Delete((p => p.PersonID), personID);
                    unitOfWork.Commit();
                }

                return new OperationStatus { Success = true };
            }
            catch (Exception e)
            {
                return OperationStatus.CreateFromException("Error deleting person.", e);
            }
        }

        private void updatePhone(Person person, Person updatedPerson, int phoneType)
        {
            var updatePhone = updatedPerson.Phones.Where(p => p.PhoneTypeID == phoneType).FirstOrDefault();
            if (updatePhone != null)
            {
                var phone = person.Phones.Where(p => p.PhoneTypeID == phoneType).FirstOrDefault();
                if (phone == null)
                {
                    phone = new Phone();
                    phone.PersonID = updatePhone.PersonID;
                    phone.PhoneTypeID = updatePhone.PhoneTypeID;
                    person.Phones.Add(phone);
                }

                phone.Number = updatePhone.Number;
            }
        }

        private void updateAddress(Person person, Person updatedPerson, int addressType, FootlooseFSSqlUnitOfWork unitOfWork)
        {
            var updatedAddressAssn = updatedPerson.Addresses.Where(a => a.AddressTypeID == addressType).FirstOrDefault();
            if (updatedAddressAssn == null)
            {
                var addressAssn = person.Addresses.Where(a => a.AddressTypeID == addressType).FirstOrDefault();
                if (addressAssn != null)
                {
                    unitOfWork.Addresses.Delete(addressAssn.Address);
                    unitOfWork.PersonAddressAssns.Delete(addressAssn);
                }
            }
            else
            {
                var addressAssn = person.Addresses.Where(a => a.AddressTypeID == addressType).FirstOrDefault();
                if (addressAssn == null)
                {
                    addressAssn = new PersonAddressAssn();
                    addressAssn.PersonID = updatedAddressAssn.PersonID;
                    addressAssn.AddressTypeID = updatedAddressAssn.AddressTypeID;
                    addressAssn.Address = new Address();

                    person.Addresses.Add(addressAssn);

                }

                addressAssn.Address.StreetAddress = updatedAddressAssn.Address.StreetAddress;
                addressAssn.Address.City = updatedAddressAssn.Address.City;
                addressAssn.Address.County = updatedAddressAssn.Address.County;
                addressAssn.Address.State = updatedAddressAssn.Address.State;
                addressAssn.Address.Zip = updatedAddressAssn.Address.Zip;
            }
        }        
    }
}
