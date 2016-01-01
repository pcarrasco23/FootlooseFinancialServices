using FootlooseFS.Models;
using FootlooseFS.Service;
using FootlooseFS.Service.Tests;
using FootlooseFS.Web.AdminUI.Controllers;
using FootlooseFS.Web.AdminUI.FootlooseFSEnterpriseService;
using FootlooseFS.Web.AdminUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace FootlooseFS.Web.AdminUI.Tests
{
    [TestClass]
    public class PersonControllerTests
    {
        private Mock<IPersonService> mockPersonService;        
        private int pageIndex;
        private int pageSize;
        private int totalItemCount;

        [TestInitialize]
        public void SetupTests()
        {
            mockPersonService = new Mock<IPersonService>();

            pageIndex = 0;
            pageSize = 10;
            totalItemCount = 100;

            // Create PersonDocument test data
            PageOfList<PersonDocument> pageOfListPerson = createTestData();
            var pageOfPersonDocuments = new PageOfPersonDocuments();
            pageOfPersonDocuments.Data = pageOfListPerson.Data;
            pageOfPersonDocuments.PageIndex = pageOfListPerson.PageIndex;
            pageOfPersonDocuments.PageSize = pageOfListPerson.PageSize;

            // Mock SearchPersonDocument and UpdatePerson service methods
            mockPersonService.Setup(m => m.SearchPersons(It.IsAny<int>(), It.IsAny<PersonSearchColumn>(), It.IsAny<SortDirection>(), It.IsAny<int>(), It.IsAny<Dictionary<PersonSearchColumn, string>>())).Returns(pageOfPersonDocuments);
            mockPersonService.Setup(m => m.UpdatePerson(It.IsAny<Person>())).Returns((Person p) => { return SetupOperationStatus(p); });
        }

        private OperationStatus SetupOperationStatus(Person p)
        {
            var opStatus = new OperationStatus();
            opStatus.Data = p;

            return opStatus;
        }

        private PageOfList<PersonDocument> createTestData()
        {
            List<PersonDocument> personDocuments = new List<PersonDocument>();

            PersonDocument personDocument = new PersonDocument
            {
                PersonID = 1,
                FirstName = "Pam",
                LastName = "Scicchitano",
                EmailAddress = "pam@scicchitano.com",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 2,
                FirstName = "Dominique",
                LastName = "Marantz",
                EmailAddress = "dMarantz@Marantz.com",
                PhoneNumber = "336-418-5159",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 3,
                FirstName = "Denese",
                LastName = "Cullars",
                EmailAddress = "denese@cullars.com",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 4,
                FirstName = "Gaynelle",
                LastName = "Resetar",
                EmailAddress = "gaynelle@yahoo.com",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 5,
                FirstName = "Melynda",
                LastName = "Stockton",
                EmailAddress = "melynda_stockton@yahoo.com",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 6,
                FirstName = "Rubye",
                LastName = "Humphers",
                EmailAddress = "rubye@hotmail.com",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 7,
                FirstName = "Otto",
                LastName = "Uy",
                EmailAddress = "otto.uy@aol.com",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 8,
                FirstName = "Carita",
                LastName = "Campain",
                EmailAddress = "carita_campain@cox.net",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 9,
                FirstName = "Luvenia",
                LastName = "Safe",
                EmailAddress = "luvenia_safe@hotmail.com",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);

            personDocument = new PersonDocument
            {
                PersonID = 10,
                FirstName = "David",
                LastName = "ScicMilechitano",
                EmailAddress = "david@mile.org",
                PhoneNumber = "336-418-5159",
                StreetAddress = "38 S Dunworth St #4185",
                State = "NC",
                Zip = "27215"
            };
            personDocuments.Add(personDocument);
            
            PageOfList<PersonDocument> personDocumentPage = new PageOfList<PersonDocument>(personDocuments, pageIndex, pageSize, totalItemCount);

            return personDocumentPage;
        }

        [TestMethod]
        public void TestPersonSearch()
        {
            PersonController personController = new PersonController(mockPersonService.Object);

            SearchParameters searchParameters = new SearchParameters();
            searchParameters.NumberRecordsPerPage = pageSize;
            searchParameters.PageNumber = pageIndex;
            searchParameters.SortColumn = "PersonID";
            searchParameters.SortDirection = "ASC";           
            searchParameters.SearchCriteria = new Dictionary<string, string>();
            ActionResult result = personController.Search(searchParameters);

            // Verify that the result is of type PartialViewResult
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));

            var partialViewResult = result as PartialViewResult;          

            // Verify that the model of the result is PageOfList<PersonDocument>
            Assert.IsInstanceOfType(partialViewResult.Model, typeof(PageOfList<PersonDocument>));

            var personPage = partialViewResult.Model as PageOfList<PersonDocument>;

            // Verify that the same number of object provided by the service search method are returned by the controller
            Assert.AreEqual(personPage.Data.Count, 10);
        }

        [TestMethod]
        public void TestPersonSave()
        {
            PersonController personController = new PersonController(mockPersonService.Object);

            var person = new Person();
            person.PersonID = 1;            
            person.FirstName = "Pam";
            person.LastName = "Scicchitano";
            person.EmailAddress = "pam@scicchitano.com";

            ActionResult result = personController.Save(person);

            // Verify that the result is of type JsonResult
            Assert.IsInstanceOfType(result, typeof(JsonResult));

            var jsonResult = result as JsonResult;

            // Verify that the model of the result is SavePersonResult
            Assert.IsInstanceOfType(jsonResult.Data, typeof(SavePersonResult));

            var savePersonResult = jsonResult.Data as SavePersonResult;

            // Verify that Person object returned from the OperationStatus has the data provided from the form
            Assert.AreEqual(savePersonResult.Person.PersonID, 1);
            Assert.AreEqual(savePersonResult.Person.FirstName, "Pam");
            Assert.AreEqual(savePersonResult.Person.LastName, "Scicchitano");
            Assert.AreEqual(savePersonResult.Person.EmailAddress, "pam@scicchitano.com");
        }
    }
}
