using FootlooseFS.Models;
using System.Configuration;
using MongoDB.Driver;
using FootlooseFS.DataPersistence.Doc;

namespace FootlooseFS.DataPersistence
{
    public class FootlooseFSDocUnitOfWork
    {
        private MongoDatabase _database;

        protected DocRepository<PersonDocument> _persons;

        public FootlooseFSDocUnitOfWork()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBConectionString"];
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var databaseName = ConfigurationManager.AppSettings["MongoDBDatabaseName"];
            _database = server.GetDatabase(databaseName);            
        }

        public IRepository<PersonDocument> Persons
        {
            get
            {
                if (_persons == null)
                    _persons = new PersonDocumentRepository(_database, "persons");

                return _persons;
            }
        }        
    }
}
