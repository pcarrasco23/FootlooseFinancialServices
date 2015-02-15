using FootlooseFS.Service;
using FootlooseFS.Web.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FootlooseFS.Web.Service.Controllers
{
    public class DashboardController : FootloseFSApiController
    {
        public DashboardController(IFootlooseFSService service) : base(service) { }

        // GET api/accounts
        public DashboardViewModel Get()
        {
            var dashboardViewModel = new DashboardViewModel();

            var person = service.GetPerson(authenticatedUser, new PersonIncludes { Accounts = false, AccountTransactions = false, Addressses = false, Login = false, Phones = false });

            dashboardViewModel.FirstName = person.FirstName;
            dashboardViewModel.LastName = person.LastName;

            return dashboardViewModel;
        }
    }
}
