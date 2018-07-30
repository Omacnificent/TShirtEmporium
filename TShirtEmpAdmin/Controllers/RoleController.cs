using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Mvc;
using TShirtEmpAdmin.Models;
using TShirtEmpAdmin.CustomFilters;

namespace TShirtEmpAdmin.Controllers
{
    [AuthLog(Roles = "Administrators")]
    public class RoleController : Controller
    {
        ApplicationDbContext _context;

        public RoleController()
        {
            _context = new ApplicationDbContext();
        }

        //get all roles
        public ActionResult Index()
        {
            var Roles = _context.Roles.ToList();
            ViewBag.Warning = TempData["warning"];
            ViewBag.Success = TempData["success"];
            return View(Roles);
        }

        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!roleManager.RoleExists(Role.Name))
            {
                _context.Roles.Add(Role);
                _context.SaveChanges();
                TempData["success"] = "Your new role has been created!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["warning"] = "This role already exists. Please check your roles and try again";
                return RedirectToAction("Index");
            }

        }


        public ActionResult ManageUserRoles()
        {
            var list =
                _context.Roles.OrderBy(r => r.Name)
                    .ToList()
                    .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name })
                    .ToList();
            ViewBag.Roles = list;
            return View();
        }

        //
        // GET: /Roles/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole =
                _context.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                var roleStore = new RoleStore<IdentityRole>(_context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                if (!roleManager.RoleExists(role.Name))
                {
                    _context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    TempData["success"] = "Your new role has been updated!";
                    return RedirectToAction("Index");
                }
                else
                {

                    TempData["warning"] = "This role already exists. Please check your roles and try again";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Roles/Delete/5
        public ActionResult Delete(string RoleName)
        {
            var thisRole =
            _context.Roles.FirstOrDefault(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase));
            _context.Roles.Remove(thisRole);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user =
                _context.Users.FirstOrDefault(
                    u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
            var account = new AccountController();
            account.UserManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = RoleName + " Role added to " + UserName + " successfully!";

            // prepopulat roles for the view dropdown
            var list =
                _context.Roles.OrderBy(r => r.Name)
                    .ToList()
                    .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name })
                    .ToList();
            ViewBag.Roles = list;

            return View("ManageUserRoles");
        }

        public ActionResult GetRoles()
        {
            var list =
                _context.Roles.OrderBy(r => r.Name)
                    .ToList()
                    .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name })
                    .ToList();
            ViewBag.Roles = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var account = new AccountController();
                var list =
                    _context.Roles.OrderBy(r => r.Name)
                        .ToList()
                        .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name })
                        .ToList();
                ApplicationUser user =
                    _context.Users.FirstOrDefault(
                        u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
                if (_context.Users.Any(x => x.UserName == UserName))
                {
                    ViewBag.RolesForThisUser = account.UserManager.GetRoles(user.Id);
                    ViewBag.Roles = list;
                }
                else
                {

                    ViewBag.Message = "User does not exist";
                }
            }

            return View("GetRoles");
        }

        [HttpGet]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var account = new AccountController();
            ApplicationUser user =
                _context.Users.FirstOrDefault(
                    u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));

            if (account.UserManager.IsInRole(user.Id, RoleName))
            {
                account.UserManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.Message = "Role removed from this user successfully!";
                return View("GetRoles");
            }
            var list =
                _context.Roles.OrderBy(r => r.Name)
                    .ToList()
                    .Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name })
                    .ToList();
            ViewBag.Roles = list;

            return View("GetRoles");
        }
    }
}