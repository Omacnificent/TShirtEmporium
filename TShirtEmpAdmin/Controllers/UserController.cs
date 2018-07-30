using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
using PagedList;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TShirtEmpAdmin.CustomFilters;
using TShirtEmpAdmin.Models;

namespace TShirtEmpAdmin.Controllers
{
    [AuthLog(Roles = "Administrators")]
    public class UserController : Controller
    {
        ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        DpapiDataProtectionProvider provider = new DpapiDataProtectionProvider("TShirtEmpAdmin");
        EmailService EmailService = new EmailService();



        public UserController(UserManager<ApplicationUser> userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        private UserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public UserController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            _context = new ApplicationDbContext();
        }

        [AuthLog(Roles = "Administrators")]
        // GET: User
        public ActionResult Index(string message, string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var MYU = from m in _context.Users
                      group m by new { m.EmployeeId } into grp
                      where grp.Count() > 1
                      select grp.Key;

            var Users = from i in _context.Users select i;
            var UserCount = Users.Count();

            var duplicates = Users.GroupBy(i => new { i.EmployeeId })
                            .Where(g => g.Count() > 1)
                            .Select(g => g.FirstOrDefault()).ToList();

            var dupCount = duplicates.Count();
            ViewBag.Count = UserCount - dupCount;

            switch (sortOrder)
            {
                case "EmployeeId_desc":
                    Users = Users.OrderByDescending(e => e.EmployeeId);
                    break;
                default:
                    Users = Users.OrderBy(e => e.EmployeeId);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(y => y.EmployeeId.Equals(searchString)
                    || y.EmployeeId.Contains(searchString)
                    || y.FirstName.Equals(searchString)
                    || y.FirstName.Contains(searchString)
                    || y.LastName.Equals(searchString)
                    || y.LastName.Contains(searchString));
            }

            ViewBag.Message = TempData["message"];

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(Users.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Duplicates()
        {
            var users = from us in _context.Users select us;
            var duplicates = users.GroupBy(i => new { i.EmployeeId })
                            .Where(g => g.Count() > 1)
                            .Select(g => g.FirstOrDefault()).ToList();

            var dupCount = duplicates.Count();
            ViewBag.Count = dupCount;
            
            return View(duplicates.ToList());
        }

        public ActionResult Details(string id, ApplicationUser Users)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users = _context.Users.Find(id);
            if (Users == null)
            {
                return HttpNotFound();
            }
            return View(Users);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id, ApplicationUser Users)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users = _context.Users.Find(id);
            if (Users == null)
            {
                return HttpNotFound();
            }
            return View(Users);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser user = UserManager.FindByName(model.UserName);
                user.EmployeeId = model.EmployeeId;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Department = model.Department;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.EmailConfirmed = model.EmailConfirmed;
                var updateResult = UserManager.Update(user);
                if (updateResult.Succeeded)
                {
                    TempData["message"] = "The record has been updated";
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    TempData["message"] = "Something went wrong";
                    return RedirectToAction("Index", "User"); 
                }
            }
            return View(model);
        }

        public ActionResult Delete(string UserName)
        {
            var user = UserManager.FindByName(UserName);
            //***************************************************************************
            //***************************************************************************
            //used the following on Up to remove the Orders and UserLog key constraint. 
            //Doing this prevents the need to clear the log and order table.
            //DropForeignKey("dbo.UserLogs", "Customer_Id", "dbo.AspNetUsers");
            //DropForeignKey("dbo.Orders", "Customer_Id", "dbo.AspNetUsers");
            //DropForeignKey("dbo.Orders", "EmpId_Id", "dbo.AspNetUsers");
            //_context.UserLogs
            //   .Where(p => p.Customer.Id == user.Id)
            //   .ToList()
            //   .ForEach(p => _context.UserLogs.Remove(p));

            //_context.Orders
            //   .Where(p => p.Customer.Id == user.Id)
            //   .ToList()
            //   .ForEach(p => _context.Orders.Remove(p));
            //***************************************************************************
            //***************************************************************************
            _context.SaveChanges();
            UserManager.Delete(user);
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

    }
}