using FootlooseFS.Models;
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

            var person = service.GetPersonByUsername(authenticatedUser, new PersonIncludes());

            dashboardViewModel.FirstName = person.FirstName;
            dashboardViewModel.LastName = person.LastName;

            return dashboardViewModel;
        }
    }
}
