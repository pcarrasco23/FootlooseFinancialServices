using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FootlooseFS.EnterpriseService.IntegrationTests.FootlooseFSEnterpriseService;

namespace FootlooseFS.EnterpriseService.IntegrationTests
{
    [TestClass]
    public class FootloosFSEnterpriseServiceIntegrationTests
    {
        [TestMethod]
        public void TestPersonDocumentSearch()
        {
            var client = new PersonServiceClient();

            var searchCriteria = new PersonDocument();
            searchCriteria.State = "NY";

            var pageOfListPersonDocuments = client.SearchPersons(1, 10, "lastname", SortDirection.Ascending, searchCriteria);
            client.Close();

            // Verify that only 10 records or less are returned
            Assert.IsTrue(pageOfListPersonDocuments.Data.Count <= 10);

            // Verify that only NY state record are returned
            foreach (PersonDocument personDoc in pageOfListPersonDocuments.Data)
                Assert.AreEqual(personDoc.State, "NY");            
        }

        [TestMethod]
        public void TestGetPersonById()
        {
            var client = new PersonServiceClient();

            var person = client.GetPersonById(5, new PersonIncludes { Phones = true, Addressses = true, Accounts = true });
            client.Close();

            Assert.IsNotNull(person);
        }
    }
}
