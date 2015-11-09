using System;
using System.Collections.Generic;
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
            PersonServiceClient client = new PersonServiceClient();

            var searchCriteria = new Dictionary<PersonSearchColumn, string>();
            searchCriteria.Add(PersonSearchColumn.State, "NY");

            var pageOfListPersonDocuments = client.SearchPersons(1, PersonSearchColumn.LastName, SortDirection.Ascending, 10, searchCriteria);

            // Verify that only 10 records or less are returned
            Assert.IsTrue(pageOfListPersonDocuments.Data.Count <= 10);

            // Verify that only NY state record are returned
            foreach (PersonDocument personDoc in pageOfListPersonDocuments.Data)
                Assert.AreEqual(personDoc.State, "NY");

            client.Close();
        }

        [TestMethod]
        public void TestGetPersonById()
        {
            PersonServiceClient client = new PersonServiceClient();

            var person = client.GetPersonById(5, new PersonIncludes { Phones = true, Addressses = true, Accounts = true });

            client.Close();

        }
    }
}
