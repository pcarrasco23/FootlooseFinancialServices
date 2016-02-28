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
        private const int PASSWORD_SALT_SIZE = 32;

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

                    var json = serializePersonToPersonDocumentJson(person);
                    notificationService.SendPersonUpdatedNotification(person.PersonID, json);

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

        public OperationStatus Login(string userName, string password)
        {
            try
            {
                using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
                {
                    var personLoginQueryable = unitOfWork.PersonLogins.GetQueryable().Where(p => p.LoginID == userName);
                    if (personLoginQueryable.Any())
                    {
                        var personLogin = personLoginQueryable.First();

                        var generatedHashedPassword = PasswordUtils.GenerateHashedPassword(password, personLogin.Salt);

                        if (generatedHashedPassword == personLogin.HashedPassword)
                            return new OperationStatus { Success = true, Data = personLoginQueryable.First().PersonID };
                        else
                            return new OperationStatus { Success = false, Messages = new List<string> { "Invalid username/password" } };
                    }
                    else
                    {
                        return new OperationStatus { Success = false, Messages = new List<string> { "Invalid username/password" } };
                    }
                }            
            }
            catch(Exception ex)
            {
                return OperationStatus.CreateFromException("Error with login.", ex);
            }
        }

        public OperationStatus UpdatePassword(string user, string oldPassword, string newPassword)
        {
            try
            {
                using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
                {
                    var personLoginQueryable = unitOfWork.PersonLogins.GetQueryable().Where(p => p.LoginID == user);
                    if (personLoginQueryable.Any())
                    {
                        // Validate the old password
                        var personLogin = personLoginQueryable.First();

                        var generatedHashedPassword = PasswordUtils.GenerateHashedPassword(oldPassword, personLogin.Salt);

                        if (generatedHashedPassword == personLogin.HashedPassword)
                        {
                            // Now verify that the new password meet the criteria for valid passwords
                            var passwordValidationStatus = PasswordUtils.ValidatePassword(newPassword);

                            if (passwordValidationStatus.Success)
                            {
                                // Now generate a salt and hash for the new password
                                var salt = PasswordUtils.CreateSalt(PASSWORD_SALT_SIZE);
                                personLogin.Salt = salt;
                                personLogin.HashedPassword = PasswordUtils.GenerateHashedPassword(newPassword, salt);

                                unitOfWork.PersonLogins.Update(personLogin);
                                unitOfWork.Commit();

                                return new OperationStatus { Success = true };
                            }
                            else
                            {
                                return passwordValidationStatus;
                            }
                        }    
                        else
                        {
                            return new OperationStatus { Success = false, Messages = new List<string> { "The old password provided is not correct" } };
                        }                    
                    }
                    else
                    {
                        return new OperationStatus { Success = false, Messages = new List<string> { "The username provided does not match a user in the system" } };
                    }
                }
            }
            catch (Exception e)
            {
                return OperationStatus.CreateFromException("Error deleting person.", e);
            }            
        }

        public OperationStatus Enroll(EnrollmentRequest enrollmentRequest)
        {
            try
            {
                using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
                {
                    // Verify that the provided enrollment data matches a person in the system
                    var personQueryable = unitOfWork.Persons.GetQueryable()
                        .Where(p => 
                            p.LastName == enrollmentRequest.LastName && 
                            p.Accounts.Any(a => a.Account.AccountNumber == enrollmentRequest.AccountNumber));

                    if (personQueryable.Any())
                    {
                        var person = personQueryable.First();

                        // Verify that the person does not already have an account
                        var personLoginQueryable = unitOfWork.PersonLogins.GetQueryable()
                            .Where(p => p.PersonID == person.PersonID);

                        if (personLoginQueryable.Any())                        
                        {
                            return new OperationStatus { Success = false, Messages = new List<string> { "The holder of this account is already registered in the system." } };
                        }
                        else
                        {
                            // Verify that the username is not already used
                            personLoginQueryable = unitOfWork.PersonLogins.GetQueryable()
                                .Where(p => p.LoginID.ToLower() == enrollmentRequest.Username.ToLower());

                            if (personLoginQueryable.Any())
                            {
                                return new OperationStatus { Success = false, Messages = new List<string> { "The username is already in use." } };
                            }
                            else
                            {
                                var passwordValidationStatus = PasswordUtils.ValidatePassword(enrollmentRequest.Password);

                                if (passwordValidationStatus.Success)
                                {
                                    var personLogin = new PersonLogin();

                                    personLogin.PersonID = person.PersonID;
                                    personLogin.LoginID = enrollmentRequest.Username;

                                    // The stored password will be a hash based on a salt and the password provided
                                    var salt = PasswordUtils.CreateSalt(PASSWORD_SALT_SIZE);
                                    personLogin.Salt = salt;
                                    personLogin.HashedPassword = PasswordUtils.GenerateHashedPassword(enrollmentRequest.Password, salt);

                                    unitOfWork.PersonLogins.Add(personLogin);
                                    unitOfWork.Commit();

                                    return new OperationStatus { Success = true };
                                }
                                else
                                {
                                    return passwordValidationStatus;
                                }
                            }
                        }                                        
                    }
                    else
                    {
                        return new OperationStatus { Success = false, Messages = new List<string> { "There is no one in the system that matches the information provided" } };
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
