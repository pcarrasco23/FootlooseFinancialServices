using FootlooseFS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

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
                    _persons = new DocRepository<PersonDocument>(_database, "persons");

                return _persons;
            }
        }        

        public static void Init()
        {
            BsonClassMap.RegisterClassMap<PersonDocument>(cm =>
            {
                cm.MapIdProperty(p => p.PersonID);
                cm.MapProperty(p => p.FirstName);
                cm.MapProperty(p => p.LastName);
                cm.MapProperty(p => p.EmailAddress);
                cm.MapProperty(p => p.PhoneNumber);
                cm.MapProperty(p => p.StreetAddress);
                cm.MapProperty(p => p.City);
                cm.MapProperty(p => p.County);
                cm.MapProperty(p => p.State);
                cm.MapProperty(p => p.Zip);
            });
        }
    }
}
