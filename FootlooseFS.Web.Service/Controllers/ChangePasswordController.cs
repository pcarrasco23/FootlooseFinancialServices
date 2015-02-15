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
    public class ChangePasswordController : FootloseFSApiController
    {
        public ChangePasswordController(IFootlooseFSService service) : base(service) { }

        // PUT: api/changepassword
        public void Put([FromBody] ChangePasswordViewModel changePasswordViewModel)
        {
            var oppStatus = service.UpdatePassword(authenticatedUser, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);

            // Return success or error state
            if (!oppStatus.Success)
            {
                throw new Exception(oppStatus.Messages[0]);
            }
        }
    }
}
