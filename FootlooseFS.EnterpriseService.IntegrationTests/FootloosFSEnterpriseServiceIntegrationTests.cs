using FootlooseFS.EnterpriseService.IntegrationTests.FootlooseFSEnterpriseService;
using NUnit.Framework;

namespace FootlooseFS.EnterpriseService.IntegrationTests
{
    [TestFixture]
    public class FootloosFSEnterpriseServiceIntegrationTests
    {
        [Test]
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

        [Test]
        public void TestGetPersonById()
        {
            var client = new PersonServiceClient();

            var person = client.GetPersonById(5, new PersonIncludes { Phones = true, Addressses = true, Accounts = true });
            client.Close();

            Assert.IsNotNull(person);
        }
    }
}
