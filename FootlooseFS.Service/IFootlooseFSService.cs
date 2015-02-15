using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FootlooseFS.DataPersistence;
using FootlooseFS.Models;

namespace FootlooseFS.Service
{
    public interface IFootlooseFSService
    {
        // Persons (Account Holders)
        PageOfList<Person> SearchPersons(int pageNumber, PersonSearchColumn personSearchColumn, SortDirection sortDirection, int numRecordsInPage, Dictionary<PersonSearchColumn, string> criteria);
        PageOfList<PersonDocument> SearchPersonDocuments(int pageNumber, PersonSearchColumn personSearchColumn, SortDirection sortDirection, int numRecordsInPage, Dictionary<PersonSearchColumn, string> criteria);
        Person GetPerson(int personID, PersonIncludes personIncludes);
        Person GetPerson(string userName, PersonIncludes personIncludes); 
        OperationStatus InsertPerson(Person person);
        OperationStatus UpdatePerson(Person person);
        OperationStatus DeletePerson(int personID);
        int GetPersonID(string userName, string password);
        OperationStatus UpdatePassword(string user, string oldPassword, string newPassword);
    }
}
