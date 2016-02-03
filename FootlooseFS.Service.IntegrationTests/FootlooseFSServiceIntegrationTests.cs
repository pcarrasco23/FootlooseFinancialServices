using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FootlooseFS.DataPersistence;
using FootlooseFS.Models;
using NUnit.Framework;

namespace FootlooseFS.Service.IntegrationTests
{
    [TestFixture]
    public class FootlooseFSServiceIntegrationTests
    {
        [Test]
        public void TestPersonDocumentSearch()
        {
            var uowFactory = new FootlooseFSSqlUnitOfWorkFactory();
            var notificationService = new FootlooseFSNotificationService();
            var service = new FootlooseFSService(uowFactory, notificationService);

            var searchCriteria = new PersonDocument();
            searchCriteria.State = "NY";

            var pageOfListPersonDocuments = service.SearchPersonDocuments(1, 10, "LastName", SortDirection.Ascending, searchCriteria);

            // Verify that only 10 records or less are returned
            Assert.IsTrue(pageOfListPersonDocuments.Data.Count <= 10);

            // Verify that only NY state record are returned
            Assert.IsFalse(pageOfListPersonDocuments.Data.Any(p => p.State != "NY"));
        }

        [Test]
        public void TestUpdatePerson()
        {
            var uowFactory = new FootlooseFSSqlUnitOfWorkFactory();
            var notificationService = new FootlooseFSNotificationService();
            var service = new FootlooseFSService(uowFactory, notificationService);

            // for this test lets get all of the linked tables and verify that we can properly query for all of the this information
            var person = service.GetPersonById(5, new PersonIncludes { Phones = true, Accounts = true, Addressses = true, AccountTransactions = true });

            // Update the email address
            person.EmailAddress = "pam@gmail.com";

            // Upate the home phone
            var homePhone = person.Phones.Where(p => p.PhoneTypeID == 1).First();
            var oldhomePhone = homePhone.Number;
            homePhone.Number = "336-418-5555";

            // Update the home address
            var homeAddress = person.Addresses.Where(pa => pa.AddressTypeID == 1).First().Address;
            var oldAddress = homeAddress.StreetAddress;
            var oldCity = homeAddress.City;
            var oldZip = homeAddress.Zip;
            var oldState = homeAddress.State;

            homeAddress.StreetAddress = "631 Glebe Road";
            homeAddress.City = "Arlington";
            homeAddress.Zip = "20178";
            homeAddress.State = "VA";

            // Now perform the update in the SQL datastore
            var opStatus = service.UpdatePerson(person);

            var updatedPerson = (Person)opStatus.Data;

            // Verify that the UpdatePerson method returns data of type Person
            Assert.IsInstanceOf<Person>(updatedPerson);

            var updatedPersonFromUoW = service.GetPersonById(updatedPerson.PersonID, new PersonIncludes { Phones = true, Addressses = true });

            // Verify that email address was updated
            Assert.AreEqual(updatedPersonFromUoW.EmailAddress, "pam@gmail.com");

            // Verify that the home phone number was updated
            homePhone = updatedPersonFromUoW.Phones.Where(p => p.PhoneTypeID == 1).FirstOrDefault();
            Assert.AreEqual(homePhone.Number, "336-418-5555");

            // Verify that the address was updated
            var address = updatedPersonFromUoW.Addresses.Where(a => a.AddressTypeID == 1).FirstOrDefault().Address;

            Assert.AreEqual(address.StreetAddress, "631 Glebe Road");
            Assert.AreEqual(address.City, "Arlington");
            Assert.AreEqual(address.Zip, "20178");
            Assert.AreEqual(address.State, "VA");

            // Now put the original data back
            homePhone = person.Phones.Where(p => p.PhoneTypeID == 1).First();
            homePhone.Number = oldhomePhone;

            // Update the home address
            homeAddress = person.Addresses.Where(pa => pa.AddressTypeID == 1).First().Address;
            homeAddress.StreetAddress = oldAddress;
            homeAddress.City = oldCity;
            homeAddress.Zip = oldZip;
            homeAddress.State = oldState;

            // Now perform the update in the SQL datastore
            opStatus = service.UpdatePerson(person);

            Assert.IsTrue(opStatus.Success);            
        }

        [Test]
        public void TestInsertAndDeletePerson()
        {
            var uowFactory = new FootlooseFSSqlUnitOfWorkFactory();
            var notificationService = new FootlooseFSNotificationService();
            var service = new FootlooseFSService(uowFactory, notificationService);

            var person = new Person
            {
                FirstName = "John",
                LastName = "Dorman",
                EmailAddress = "john@dorman.com", // Updated email address
                Phones = new List<Phone>
                {
                    new Phone
                    {
                        Number = "813-657-2222",    // Update home phone
                        PhoneTypeID = 1
                    }
                },
                Addresses = new List<PersonAddressAssn>
                {
                    new PersonAddressAssn
                    {
                        Address = new Address
                        {
                            StreetAddress = "823 Newton Drive",   // Updated address
                            State = "FL",
                            Zip = "33782",
                            AddressID = 5,
                            City = "Pinellas Park"
                        },
                        AddressTypeID = 1,
                    }
                },
            };

            var opStatus = service.InsertPerson(person);

            var insertedPerson = (Person)opStatus.Data;

            // Verify that the InsertPerson method returns data of type Person
            Assert.IsInstanceOf<Person>(insertedPerson);

            var insertedPersonFromUoW = service.GetPersonById(insertedPerson.PersonID, new PersonIncludes { Phones = true, Addressses = true });

            // Verify that email address was applied
            Assert.AreEqual(insertedPersonFromUoW.EmailAddress, "john@dorman.com");

            // Verify that the home phone number was updated
            var homePhone = insertedPersonFromUoW.Phones.Where(p => p.PhoneTypeID == 1).FirstOrDefault();
            Assert.AreEqual(homePhone.Number, "813-657-2222");

            // Verify that there is no work number
            var workPhone = insertedPersonFromUoW.Phones.Where(p => p.PhoneTypeID == 2 && ! string.IsNullOrEmpty(p.Number));
            Assert.AreEqual(workPhone.Count(), 0);

            // Verify that there is no cell number
            var cellPhone = insertedPersonFromUoW.Phones.Where(p => p.PhoneTypeID == 3 && !string.IsNullOrEmpty(p.Number));
            Assert.AreEqual(cellPhone.Count(), 0);

            // Verify that the address was updated
            var address = insertedPersonFromUoW.Addresses.Where(a => a.AddressTypeID == 1).FirstOrDefault().Address;

            Assert.AreEqual(address.StreetAddress, "823 Newton Drive");
            Assert.AreEqual(address.City, "Pinellas Park");
            Assert.AreEqual(address.Zip, "33782");
            Assert.AreEqual(address.State, "FL");

            // Now delete the person
            service.DeletePerson(insertedPersonFromUoW);

            // Make sure the person is deleted
            var deletedPerson = service.GetPersonById(insertedPersonFromUoW.PersonID, new PersonIncludes());

            Assert.IsNull(deletedPerson);
        }
    }
}
