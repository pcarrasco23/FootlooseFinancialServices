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
    public class RegisterController : FootloseFSApiController
    {
        public RegisterController(IFootlooseFSService service) : base(service) { }

        // PUT: api/register
        [AllowAnonymous]
        public void Put([FromBody] RegisterViewModel registerViewModel)
        {
            var enrollmentRequest = new EnrollmentRequest
            {
                LastName = registerViewModel.LastName,
                AccountNumber = registerViewModel.AccountNumber,
                Username = registerViewModel.Username,
                Password = registerViewModel.Password                
            };

            var oppStatus = service.Enroll(enrollmentRequest);

            // Return success or error state
            if (!oppStatus.Success)
            {
                throw new Exception(oppStatus.Messages[0]);
            }
        }
    }
}
