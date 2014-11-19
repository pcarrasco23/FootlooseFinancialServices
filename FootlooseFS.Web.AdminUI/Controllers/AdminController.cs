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
    public class AdminController : Controller
    {
        public AdminController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Admin/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<User> users = new List<User>();

            using (var dbContext = new ApplicationDbContext())
            {
                // Get a list of application users in the system
                users = (from u in dbContext.Users
                         select new User
                         {
                             UserId = u.Id,
                             UserName = u.UserName
                         }).ToList();

                // For each user get the list of roles in which they are assigned
                foreach(var user in users)
                {
                    var roles = UserManager.GetRoles<ApplicationUser>(user.UserId);
                    user.Roles = String.Join(",", roles.ToArray());
                }
            }

            return View(users);
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {                    
                    return RedirectToAction("Index", "Person");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
	}
}