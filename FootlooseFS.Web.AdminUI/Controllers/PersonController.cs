using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

using FootlooseFS.Models;
using FootlooseFS.Service;
using Ninject;
using FootlooseFS.Web.AdminUI.Models;
using FootlooseFS.Web.AdminUI.FootlooseFSEnterpriseService;

namespace FootlooseFS.Web.AdminUI.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService personService;

        [Inject]
        public PersonController(IPersonService personService)
        {
            this.personService = personService;
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Person/Search
        [HttpPost]
        public ActionResult Search(SearchParameters searchParameters)
        {
            // Serialize sort direction to an enum of SortDirection
            SortDirection sortDirection = SortDirection.Ascending;
            Enum.TryParse<SortDirection>(searchParameters.SortDirection, out sortDirection);

            var searchCriteria = new PersonDocument();

            if (searchParameters.SearchCriteria != null && searchParameters.SearchCriteria.Count() > 0)
            {
                foreach (var searchCriterion in searchParameters.SearchCriteria)
                {
                    if (!string.IsNullOrEmpty(searchCriterion.Value))
                    {
                        if (searchCriterion.Key == "PersonID")
                            searchCriteria.PersonID = Int32.Parse(searchCriterion.Value);
                        else if (searchCriterion.Key == "FirstName")
                            searchCriteria.FirstName = searchCriterion.Value;
                        else if (searchCriterion.Key == "LastName")
                            searchCriteria.LastName = searchCriterion.Value;
                        else if (searchCriterion.Key == "PhoneNumber")
                            searchCriteria.PhoneNumber = searchCriterion.Value;
                        else if (searchCriterion.Key == "StreetAddress")
                            searchCriteria.StreetAddress = searchCriterion.Value;
                        else if (searchCriterion.Key == "City")
                            searchCriteria.City = searchCriterion.Value;
                        else if (searchCriterion.Key == "Zip")
                            searchCriteria.Zip = searchCriterion.Value;
                        else if (searchCriterion.Key == "State")
                            searchCriteria.State = searchCriterion.Value;
                        else if (searchCriterion.Key == "EmailAddress")
                            searchCriteria.EmailAddress = searchCriterion.Value;
                    }
                }
            }

            var personsPage = personService.SearchPersons(searchParameters.PageNumber, searchParameters.NumberRecordsPerPage, searchParameters.SortColumn, sortDirection, searchCriteria);

            var pageOfListPersonDocuments = new PageOfList<PersonDocument>(personsPage.Data, searchParameters.PageNumber, searchParameters.NumberRecordsPerPage, personsPage.TotalItemCount);
            pageOfListPersonDocuments.SearchCriteria = searchParameters.SearchCriteria;

            return PartialView(pageOfListPersonDocuments);            
        }

        //
        // GET: /Person/New
        [Authorize(Roles = "Admin,Demographics")]
        public ActionResult New()
        {           
            var person = new Person();

            person.PersonID = 0;
            person.FirstName = string.Empty;
            person.LastName = string.Empty;
            person.EmailAddress = string.Empty;

            person.Phones = new List<Phone>();
            person.Phones.Add(new Phone { PhoneTypeID = 1, Number = string.Empty });
            person.Phones.Add(new Phone { PhoneTypeID = 2, Number = string.Empty });
            person.Phones.Add(new Phone { PhoneTypeID = 3, Number = string.Empty });

            var emptyAddress = new Address
            {
                StreetAddress = string.Empty,
                City = string.Empty,
                State = string.Empty,
                Zip = string.Empty
            };

            person.Addresses = new List<PersonAddressAssn>();
            person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 1, Address = emptyAddress });
            person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 2, Address = emptyAddress });
            person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 3, Address = emptyAddress });

            ViewBag.States = stateSelectList();

            return PartialView("Edit", person);
        }

        //
        // GET: /Person/Edit
        [Authorize(Roles = "Admin,Demographics,Financial,OnlineAccess")]
        public ActionResult Edit(int personID)
        {
            // When editing persons from here only include the phones and addresses
            var personIncludes = new PersonIncludes();
            personIncludes.Phones = true;
            personIncludes.Addressses = true;
            personIncludes.Accounts = true;
            personIncludes.Login = true;

            var person = personService.GetPersonById(personID, personIncludes);

            ViewBag.States = stateSelectList();

            return PartialView(person);            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Demographics")]
        [HttpPost]
        public ContentResult Save(Person person)
        {            
            if (person.PersonID == 0)
            {
                var opStatus = personService.InsertPerson(person);

                return Content("The person has been created in the system.");
            }                
            else
            {
                var opStatus = personService.UpdatePerson(person);

                return Content("The person has been updated");                
            }
        }

        private List<SelectListItem> stateSelectList()            
        {
            return new List<SelectListItem>()
            {
                new SelectListItem() {Text="Alabama", Value="AL"},
                new SelectListItem() { Text="Alaska", Value="AK"},
                new SelectListItem() { Text="Arizona", Value="AZ"},
                new SelectListItem() { Text="Arkansas", Value="AR"},
                new SelectListItem() { Text="California", Value="CA"},
                new SelectListItem() { Text="Colorado", Value="CO"},
                new SelectListItem() { Text="Connecticut", Value="CT"},
                new SelectListItem() { Text="District of Columbia", Value="DC"},
                new SelectListItem() { Text="Delaware", Value="DE"},
                new SelectListItem() { Text="Florida", Value="FL"},
                new SelectListItem() { Text="Georgia", Value="GA"},
                new SelectListItem() { Text="Hawaii", Value="HI"},
                new SelectListItem() { Text="Idaho", Value="ID"},
                new SelectListItem() { Text="Illinois", Value="IL"},
                new SelectListItem() { Text="Indiana", Value="IN"},
                new SelectListItem() { Text="Iowa", Value="IA"},
                new SelectListItem() { Text="Kansas", Value="KS"},
                new SelectListItem() { Text="Kentucky", Value="KY"},
                new SelectListItem() { Text="Louisiana", Value="LA"},
                new SelectListItem() { Text="Maine", Value="ME"},
                new SelectListItem() { Text="Maryland", Value="MD"},
                new SelectListItem() { Text="Massachusetts", Value="MA"},
                new SelectListItem() { Text="Michigan", Value="MI"},
                new SelectListItem() { Text="Minnesota", Value="MN"},
                new SelectListItem() { Text="Mississippi", Value="MS"},
                new SelectListItem() { Text="Missouri", Value="MO"},
                new SelectListItem() { Text="Montana", Value="MT"},
                new SelectListItem() { Text="Nebraska", Value="NE"},
                new SelectListItem() { Text="Nevada", Value="NV"},
                new SelectListItem() { Text="New Hampshire", Value="NH"},
                new SelectListItem() { Text="New Jersey", Value="NJ"},
                new SelectListItem() { Text="New Mexico", Value="NM"},
                new SelectListItem() { Text="New York", Value="NY"},
                new SelectListItem() { Text="North Carolina", Value="NC"},
                new SelectListItem() { Text="North Dakota", Value="ND"},
                new SelectListItem() { Text="Ohio", Value="OH"},
                new SelectListItem() { Text="Oklahoma", Value="OK"},
                new SelectListItem() { Text="Oregon", Value="OR"},
                new SelectListItem() { Text="Pennsylvania", Value="PA"},
                new SelectListItem() { Text="Rhode Island", Value="RI"},
                new SelectListItem() { Text="South Carolina", Value="SC"},
                new SelectListItem() { Text="South Dakota", Value="SD"},
                new SelectListItem() { Text="Tennessee", Value="TN"},
                new SelectListItem() { Text="Texas", Value="TX"},
                new SelectListItem() { Text="Utah", Value="UT"},
                new SelectListItem() { Text="Vermont", Value="VT"},
                new SelectListItem() { Text="Virginia", Value="VA"},
                new SelectListItem() { Text="Washington", Value="WA"},
                new SelectListItem() { Text="West Virginia", Value="WV"},
                new SelectListItem() { Text="Wisconsin", Value="WI"},
                new SelectListItem() { Text="Wyoming", Value="WY"}
            };          
        }
	}
}