using FootlooseFS.DataPersistence;
using FootlooseFS.Models;
using FootlooseFS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FootlooseFS.EnterpriseService
{
    public class PersonService : IPersonService
    {
        private readonly IFootlooseFSService footlooseService;

        public PersonService()
        {
            footlooseService = new FootlooseFSService(new FootlooseFSSqlUnitOfWorkFactory(), new FootlooseFSNotificationService());
        }

        public PageOfPersonDocuments SearchPersons(int pageNumber, int numRecordsInPage, string sort, SortDirection sortDirection, PersonDocument searchCriteria)
        {
            var pageOfListPerson = footlooseService.SearchPersonDocuments(pageNumber, numRecordsInPage, sort, sortDirection, searchCriteria);

            var pageOfPersons = new PageOfPersonDocuments();
            pageOfPersons.Data = pageOfListPerson.Data;
            pageOfPersons.PageIndex = pageOfListPerson.PageIndex;
            pageOfPersons.PageSize = pageOfListPerson.PageSize;
            pageOfPersons.TotalItemCount = pageOfListPerson.TotalItemCount;

            return pageOfPersons;
        }

        public Person GetPersonById(int personID, PersonIncludes personIncludes)
        {
            return footlooseService.GetPersonById(personID, personIncludes);
        }

        public OperationStatus InsertPerson(Person person)
        {
            return footlooseService.InsertPerson(person);
        }
       
        public OperationStatus UpdatePerson(Person person)
        {
            return footlooseService.UpdatePerson(person);
        }
        
        public OperationStatus DeletePerson(Person person)
        {
            return footlooseService.DeletePerson(person);
        }
    }
}
