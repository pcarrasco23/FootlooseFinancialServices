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

        public PageOfList<PersonDocument> SearchPersonDocuments(int pageNumber, int numRecordsInPage, string sort, SortDirection sortDirection, PersonDocument searchCriteria)
        {
            // Note the Document Unit of Work will be disposed when out of scope (does not require using statement)
            var unitOfWork = new FootlooseFSDocUnitOfWork();

            // Search, sort, and page the results
            return unitOfWork.Persons.Search(pageNumber, numRecordsInPage, searchCriteria, sort, sortDirection);
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
            using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
            {
                var person = unitOfWork.Persons.Find(personID, personIncludes);

                if (person != null)
                { 
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

                return person;
            }           
        }
       
        public OperationStatus InsertPerson(Person person)
        {      
            try
            {
                using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
                {                           
                    unitOfWork.Persons.Add(person);
                    unitOfWork.Commit();
                }                    

                return new OperationStatus { Success = true, Data = person };                
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
                using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
                {                    
                    var person = unitOfWork.Persons.Update(updatedPerson);
                    unitOfWork.Commit();

                    var json = serializePersonToPersonDocumentJson(person);

                    notificationService.SendPersonUpdatedNotification(person.PersonID, json);

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
