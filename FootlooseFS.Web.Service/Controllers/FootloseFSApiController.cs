using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using FootlooseFS.Service;

namespace FootlooseFS.Web.Service.Controllers
{
    public class FootloseFSApiController : ApiController
    {
        protected string authenticatedUser;
        protected readonly IFootlooseFSService service;

        public FootloseFSApiController(IFootlooseFSService service)        
        {
            this.service = service;

            // If the current user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                var userNameClaim = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name);
                if (userNameClaim != null)
                {
                    authenticatedUser = userNameClaim.Value;
                }
            }            
        }
    }
}