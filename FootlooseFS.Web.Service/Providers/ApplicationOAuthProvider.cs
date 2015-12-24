using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using FootlooseFS.Web.Service.Models;
using FootlooseFS.Service;
using FootlooseFS.DataPersistence;
using FootlooseFS.Web.Service.Controllers;
using FootlooseFS.QueueService;

namespace FootlooseFS.Web.Service.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var service = new FootlooseFSService(new FootlooseFSSqlUnitOfWorkFactory(), new FootlooseFSNotificationService());

            var personId = service.GetPersonID(context.UserName, context.Password);
            if (personId <= 0)
            {
                context.SetError("invalid_grant", "The username or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));

            context.Validated(identity);
        }     
    }
}