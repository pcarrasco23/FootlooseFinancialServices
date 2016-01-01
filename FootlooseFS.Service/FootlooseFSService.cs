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
using Newtonsoft.Json;

namespace FootlooseFS.Service
{
    public class FootlooseFSService : IFootlooseFSService
    {
        private readonly IFootlooseFSUnitOfWorkFactory unitOfWorkFactory;
        private readonly IFootlooseFSNotificationService notificationService;

        public FootlooseFSService(IFootlooseFSUnitOfWorkFactory unitOfWorkFactory, IFootlooseFSNotificationService notificationService)
        {
            // The unit of work factory will determine which datastore will be used (SQL, MongoDB, Test)
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.notificationService = notificationService;
        }

        public PageOfList<PersonDocument> SearchPersonDocuments(int pageNumber, PersonSearchColumn personSearchColumn, SortDirection sortDirection, int numRecordsInPage, Dictionary<PersonSearchColumn, string> searchCriteria)
        {
            // Search for persons using the Document DB repository
            // Determine the starting row from the pageNumber and numRecordsInPage
            int startRow;
            int totalItemCount = 0;

            if (numRecordsInPage == -1)
                startRow = 0;
            else
                startRow = (pageNumber - 1) * numRecordsInPage;

            PageOfList<PersonDocument> searchResults = null;
         
            // Note the Document Unit of Work will be disposed when out of scope (does not require using statement)
            var unitOfWork = new FootlooseFSDocUnitOfWork();
            
            IQueryable<PersonDocument> personsQueryable = unitOfWork.Persons.GetQueryable();

            // For each search criteria given add an appropriate filter to the Person Queryable
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

        public Person GetPersonByUsername(string userName, PersonIncludes personIncludes)
        {
            using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
            {
                var personLoginQueryable = unitOfWork.PersonLogins.GetQueryable().Where(p => p.LoginID == userName);
                if (personLoginQueryable.Any())
                {
                    int personId = personLoginQueryable.First().PersonID;
                    
                    return GetPersonById(personId, personIncludes);
                }
                else
                {
                    throw new Exception("Person with given username not found");
                }
            }
        }

        public Person GetPersonById(int personID, PersonIncludes personIncludes)
        {
            // Get the person data from the SQL repository
            Person person = null;

            using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
            {
                var personQueryable = unitOfWork.Persons.GetQueryable().Where(p => p.PersonID == personID);

                if (personIncludes.Phones)
                {
                    personQueryable = personQueryable.Include("Phones");
                    personQueryable = personQueryable.Include("Phones.PhoneType");
                }

                if (personIncludes.Addressses)
                {
                    personQueryable = personQueryable.Include("Addresses.Address");
                    personQueryable = personQueryable.Include("Addresses.AddressType");
                }

                if (personIncludes.Accounts)
                {
                    personQueryable = personQueryable.Include("Accounts.Account")
                                                    .Include("Accounts.Account.AccountType");                    
                }   
                
                if (personIncludes.Login)                
                    personQueryable = personQueryable.Include("Login");                

                if (personIncludes.AccountTransactions)
                {
                    personQueryable = personQueryable.Include("Accounts.Account.Transactions");
                    personQueryable = personQueryable.Include("Accounts.Account.Transactions.TransactionType");
                }
                
                if (personQueryable.Any())
                {
                    person = personQueryable.First();

                    if (personIncludes.Phones)
                    {
                        // Add home phone if not in the person Object
                        if (!person.Phones.Where(p => p.PhoneTypeID == 1).Any())
                            person.Phones.Add(new Phone { PhoneTypeID = 1, Number = string.Empty, PhoneType = new PhoneType { Name = "Home" } });

                        // Add work phone if not in the person Object
                        if (!person.Phones.Where(p => p.PhoneTypeID == 2).Any())
                            person.Phones.Add(new Phone { PhoneTypeID = 2, Number = string.Empty, PhoneType = new PhoneType { Name = "Work" } });

                        // Add cell phone if not in the person Object
                        if (!person.Phones.Where(p => p.PhoneTypeID == 3).Any())
                            person.Phones.Add(new Phone { PhoneTypeID = 3, Number = string.Empty, PhoneType = new PhoneType { Name = "Cell" } });

                    }

                    if (personIncludes.Addressses)
                    {
                        var emptyAddress = new Address
                        {
                            StreetAddress = string.Empty,
                            City = string.Empty,
                            State = string.Empty,
                            Zip = string.Empty
                        };

                        if (!person.Addresses.Where(a => a.AddressTypeID == 1).Any())
                            person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 1, Address = emptyAddress, AddressType = new AddressType { Name = "Home" } });

                        if (!person.Addresses.Where(a => a.AddressTypeID == 2).Any())
                            person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 2, Address = emptyAddress, AddressType = new AddressType { Name = "Work" } });

                        if (!person.Addresses.Where(a => a.AddressTypeID == 3).Any())
                            person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 3, Address = emptyAddress, AddressType = new AddressType { Name = "Alternate" } });
                    }

                    if (personIncludes.Login && person.Login == null)
                        person.Login = new PersonLogin();
                }
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
                    using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
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

                    using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
                    {
                        person = unitOfWork.Persons.GetQueryable().Where(p => p.PersonID == updatedPerson.PersonID)  
                                            .Include("Addresses.Address")
                                            .Include("Phones")
                                            .First();

                        person.FirstName = updatedPerson.FirstName;
                        person.LastName = updatedPerson.LastName;
                        person.EmailAddress = updatedPerson.EmailAddress;

                        updatePhone(person, updatedPerson, (int)PhoneTypes.Home, unitOfWork);
                        updatePhone(person, updatedPerson, (int)PhoneTypes.Work, unitOfWork);
                        updatePhone(person, updatedPerson, (int)PhoneTypes.Cell, unitOfWork);

                        updateAddress(person, updatedPerson, (int)AddressTypes.Home, unitOfWork);
                        updateAddress(person, updatedPerson, (int)AddressTypes.Work, unitOfWork);
                        updateAddress(person, updatedPerson, (int)AddressTypes.Alternate, unitOfWork);

                        unitOfWork.Persons.Update(person);
                        unitOfWork.Commit();

                        var json = serializePersonToPersonDocumentJson(person);

                        notificationService.SendPersonUpdatedNotification(person.PersonID, json);
                    }                    

                    return new OperationStatus { Success = true, Data = person };
                } 
            }
            catch (Exception e)
            {
                return OperationStatus.CreateFromException("Error updating person.", e);
            }
        }

        public OperationStatus DeletePerson(Person person)
        {
            try
            {
                using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
                {
                    if (person.Phones != null)
                    {
                        foreach (Phone phone in person.Phones)
                            unitOfWork.Phones.Delete(phone);

                        person.Phones = null;
                    }

                    if (person.Addresses != null)
                    {
                        foreach (PersonAddressAssn addressAssn in person.Addresses)
                            unitOfWork.PersonAddressAssns.Delete(addressAssn);

                        person.Addresses = null;
                    }

                    unitOfWork.Persons.Delete(person);
                    unitOfWork.Commit();
                }

                return new OperationStatus { Success = true };
            }
            catch (Exception e)
            {
                return OperationStatus.CreateFromException("Error deleting person.", e);
            }
        }

        public int GetPersonID(string userName, string password)
        {
            using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
            {
                var personLoginQueryable = unitOfWork.PersonLogins.GetQueryable().Where(p => p.LoginID == userName && p.Password == password);
                if (personLoginQueryable.Any())
                {
                    return personLoginQueryable.First().PersonID;
                }
                else
                {
                    return -1;
                }
            }
        }

        public OperationStatus UpdatePassword(string user, string oldPassword, string newPassword)
        {
            try
            {
                using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
                {
                    var personLoginQueryable = unitOfWork.PersonLogins.GetQueryable().Where(p => p.LoginID == user && p.Password == oldPassword);
                    if (personLoginQueryable.Any())
                    {
                        var personLogin = personLoginQueryable.First();
                        personLogin.Password = newPassword;

                        unitOfWork.PersonLogins.Update(personLogin);
                        unitOfWork.Commit();

                        return new OperationStatus { Success = true };
                    }
                    else
                    {
                        return new OperationStatus { Success = false, Messages = new List<string> { "The old password provided is not correct" } };
                    }
                }
            }
            catch (Exception e)
            {
                return OperationStatus.CreateFromException("Error deleting person.", e);
            }            
        }

        private void updatePhone(Person person, Person updatedPerson, int phoneType, IFootlooseFSUnitOfWork unitOfWork)
        {
            var updatePhone = updatedPerson.Phones.Where(p => p.PhoneTypeID == phoneType).FirstOrDefault();
            if (updatePhone == null)
            {
                var phone = person.Phones.Where(p => p.PhoneTypeID == phoneType).FirstOrDefault();
                if (phone != null)                
                    unitOfWork.Phones.Delete(phone);                
            }
            else
            {
                var phone = person.Phones.Where(p => p.PhoneTypeID == phoneType).FirstOrDefault();
                if (phone == null && !string.IsNullOrEmpty(updatePhone.Number))
                {
                    phone = new Phone();
                    phone.PersonID = updatePhone.PersonID;
                    phone.PhoneTypeID = updatePhone.PhoneTypeID;                    
                    person.Phones.Add(phone);
                }
                else if (phone != null)
                {
                    phone.Number = updatePhone.Number;
                }                         
            }
        }

        private void updateAddress(Person person, Person updatedPerson, int addressType, IFootlooseFSUnitOfWork unitOfWork)
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
                if (addressAssn == null && !string.IsNullOrEmpty(updatedAddressAssn.Address.StreetAddress))
                {
                    addressAssn = new PersonAddressAssn();
                    addressAssn.PersonID = updatedAddressAssn.PersonID;
                    addressAssn.AddressTypeID = updatedAddressAssn.AddressTypeID;
                    addressAssn.Address = new Address();

                    person.Addresses.Add(addressAssn);

                }
                else if (addressAssn != null)
                {
                    addressAssn.Address.StreetAddress = updatedAddressAssn.Address.StreetAddress;
                    addressAssn.Address.City = updatedAddressAssn.Address.City;
                    addressAssn.Address.State = updatedAddressAssn.Address.State;
                    addressAssn.Address.Zip = updatedAddressAssn.Address.Zip;
                }                
            }
        }        

        private string serializePersonToPersonDocumentJson(Person person)
        {
            var personDocument = new PersonDocument();

            personDocument.PersonID = person.PersonID;
            personDocument.EmailAddress = person.EmailAddress;
            personDocument.FirstName = person.FirstName;
            personDocument.LastName = person.LastName;
            personDocument.PhoneNumber = person.Phones.Any(p => p.PhoneTypeID == 1) ? person.Phones.First(p => p.PhoneTypeID == 1).Number : string.Empty;

            var address = person.Addresses.Any(a => a.AddressTypeID == 1) ? person.Addresses.First(a => a.AddressTypeID == 1).Address : null;
            if (address != null)
            {
                personDocument.StreetAddress = address.StreetAddress;
                personDocument.City = address.City;
                personDocument.State = address.State;
                personDocument.Zip = address.Zip;
            }

            return JsonConvert.SerializeObject(personDocument);
        }
    }
}
