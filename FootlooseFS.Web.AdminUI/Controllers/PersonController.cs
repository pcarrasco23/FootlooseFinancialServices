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

namespace FootlooseFS.Web.AdminUI.Controllers
{
    public class PersonController : Controller
    {
        private readonly IFootlooseFSService _footlooseFSService;

        [Inject]
        public PersonController(IFootlooseFSService footlooseFSService)
        {
            _footlooseFSService = footlooseFSService;
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
            // Serialize sort column to an enum of PersonSearchColumn
            PersonSearchColumn personSearchColumn = PersonSearchColumn.LastName;
            Enum.TryParse<PersonSearchColumn>(searchParameters.SortColumn, out personSearchColumn);

            // Serialize sort direction to an enum of SortDirection
            SortDirection sortDirection = SortDirection.Ascending;
            Enum.TryParse<SortDirection>(searchParameters.SortDirection, out sortDirection);

            // Serialize the search criteria array into a dictionary of person search column and value
            Dictionary<PersonSearchColumn, string> searchCriteria = new Dictionary<PersonSearchColumn, string>();

            if (searchParameters.SearchCriteria != null && searchParameters.SearchCriteria.Count() > 0)
            {
                foreach (var searchCriterion in searchParameters.SearchCriteria)
                {
                    if (!string.IsNullOrEmpty(searchCriterion.Value))
                    {
                        PersonSearchColumn column = PersonSearchColumn.None;
                        Enum.TryParse<PersonSearchColumn>(searchCriterion.Key, out column);

                        if (column != PersonSearchColumn.None)
                            searchCriteria.Add(column, searchCriterion.Value);
                    }
                }
            }

            var personsPage = _footlooseFSService.SearchPersonDocuments(searchParameters.PageNumber, personSearchColumn, sortDirection, searchParameters.NumberRecordsPerPage, searchCriteria);
            personsPage.SearchCriteria = searchParameters.SearchCriteria;

            return PartialView(personsPage);            
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

            var person = _footlooseFSService.GetPersonById(personID, personIncludes);

            // Add home phone if not in the person Object
            if (!person.Phones.Where(p => p.PhoneTypeID == 1).Any())
                person.Phones.Add(new Phone { PhoneTypeID = 1, Number = string.Empty });

            // Add work phone if not in the person Object
            if (!person.Phones.Where(p => p.PhoneTypeID == 2).Any())
                person.Phones.Add(new Phone { PhoneTypeID = 2, Number = string.Empty });

            // Add cell phone if not in the person Object
            if (!person.Phones.Where(p => p.PhoneTypeID == 3).Any())
                person.Phones.Add(new Phone { PhoneTypeID = 3, Number = string.Empty });

            var emptyAddress = new Address 
            { 
                StreetAddress = string.Empty,
                City = string.Empty,
                State = string.Empty,
                Zip = string.Empty
            };

            if (!person.Addresses.Where(a => a.AddressTypeID == 1).Any())
                person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 1, Address = emptyAddress });

            if (!person.Addresses.Where(a => a.AddressTypeID == 2).Any())
                person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 2, Address = emptyAddress });

            if (!person.Addresses.Where(a => a.AddressTypeID == 3).Any())
                person.Addresses.Add(new PersonAddressAssn { AddressTypeID = 3, Address = emptyAddress });

            if (person.Login == null)
                person.Login = new PersonLogin();

            ViewBag.States = stateSelectList();

            return PartialView(person);            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin,Demographics")]
        public JsonResult Save(FormCollection formCollection)
        {
            var person = new Person();

            int personID = 0;
            Int32.TryParse(formCollection["personID"], out personID);

            person.PersonID = personID;
            person.FirstName = formCollection["firstName"];
            person.LastName = formCollection["lastName"];
            person.EmailAddress = formCollection["emailAddress"];

            person.Phones = new List<Phone>();
            person.Phones.Add(new Phone { PersonID = personID, PhoneTypeID = (int)PhoneTypes.Home, Number = formCollection["homePhone"] });
            person.Phones.Add(new Phone { PersonID = personID, PhoneTypeID = (int)PhoneTypes.Work, Number = formCollection["workPhone"] });
            person.Phones.Add(new Phone { PersonID = personID, PhoneTypeID = (int)PhoneTypes.Cell, Number = formCollection["cellPhone"] });

            person.Addresses = new List<PersonAddressAssn>();

            if (!string.IsNullOrEmpty(formCollection["homeStreetAddress"]))
            {
                var address = new Address();

                int homeAddressID = 0;
                Int32.TryParse(formCollection["homeAddressID"], out homeAddressID);
                address.AddressID = homeAddressID;

                address.StreetAddress = formCollection["homeStreetAddress"];
                address.City = formCollection["homeCity"];
                address.State = formCollection["homeState"];
                address.Zip = formCollection["homeZip"];

                person.Addresses.Add(new PersonAddressAssn { PersonID = personID, AddressID = homeAddressID, Address = address, AddressTypeID = (int)AddressTypes.Home });
            }

            if (!string.IsNullOrEmpty(formCollection["workStreetAddress"]))
            {
                var address = new Address();

                int workAddressID = 0;
                Int32.TryParse(formCollection["workAddressID"], out workAddressID);
                address.AddressID = workAddressID;

                address.StreetAddress = formCollection["workStreetAddress"];
                address.City = formCollection["workCity"];
                address.State = formCollection["workState"];
                address.Zip = formCollection["workZip"];

                person.Addresses.Add(new PersonAddressAssn { PersonID = personID, AddressID = workAddressID, Address = address, AddressTypeID = (int)AddressTypes.Work });
            }

            if (!string.IsNullOrEmpty(formCollection["altStreetAddress"]))
            {
                var address = new Address();

                int altAddressID = 0;
                Int32.TryParse(formCollection["altAddressID"], out altAddressID);
                address.AddressID = altAddressID;

                address.StreetAddress = formCollection["altStreetAddress"];
                address.City = formCollection["altCity"];
                address.State = formCollection["altState"];
                address.Zip = formCollection["altZip"];

                person.Addresses.Add(new PersonAddressAssn { PersonID = personID, AddressID = altAddressID, Address = address, AddressTypeID = (int)AddressTypes.Alternate });
            }
            
            if (person.PersonID == 0)
            {
                var opStatus = _footlooseFSService.InsertPerson(person);

                var newPerson = (Person)opStatus.Data;

                var savePersonResult = new SavePersonResult
                    {
                        Message = "The person has been created in the system.",
                        Person = newPerson,
                        PersonID = newPerson.PersonID,
                        HomeAddressID = newPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Home).Any() ?
                                        newPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Home).First().AddressID : 0,
                        WorkAddressID = newPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Work).Any() ?
                                        newPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Work).First().AddressID : 0,
                        AlternateAddressID = newPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Alternate).Any() ?
                                        newPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Alternate).First().AddressID : 0,
                    };

                return Json(savePersonResult);
            }                
            else
            {
                var opStatus = _footlooseFSService.UpdatePerson(person);

                var updatedPerson = (Person)opStatus.Data;

                var savePersonResult = new SavePersonResult
                {
                    Message = "The person has been updated",
                    Person = updatedPerson,
                    PersonID = updatedPerson.PersonID,
                    HomeAddressID = updatedPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Home).Any() ?
                                    updatedPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Home).First().AddressID : 0,
                    WorkAddressID = updatedPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Work).Any() ?
                                    updatedPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Work).First().AddressID : 0,
                    AlternateAddressID = updatedPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Alternate).Any() ?
                                    updatedPerson.Addresses.Where(a => a.AddressTypeID == (int)AddressTypes.Alternate).First().AddressID : 0,
                };

                return Json(savePersonResult);                
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