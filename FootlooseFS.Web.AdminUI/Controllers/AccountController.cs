using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using FootlooseFS.Web.AdminUI.Models;

namespace FootlooseFS.Web.AdminUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            // TODO logoff actions
            return RedirectToAction("Index", "Person");
        }
    }
}