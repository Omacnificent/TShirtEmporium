using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TShirtEmpAdmin.CustomFilters;
using TShirtEmpAdmin.Models;

namespace TShirtEmpAdmin.Controllers
{    
    public class ShirtsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Shirts
        [AuthLog(Roles = "Registered Users, Administrators")]
        public async Task<ActionResult> Index()
        {
            ViewBag.Warning = TempData["warning"];
            //Define and convert querystring value to int and compare log/ordernum to 
            //num value as first step to check malicious url behaviour
            string newNum = Request.Params["unum"];
            int unum = 0;
            bool res = int.TryParse(newNum, out unum);
            var oResult = (from l in db.UserLogs where unum == l.Id select l);
            int oCount = oResult.Count();
            if (res == false)
            {
                //ASP.NET IDENTITY Logoff Script
                //var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                //AuthenticationManager.SignOut();
                return RedirectToAction("Index", "Error");
            }
            else
            {
                //get both current logged in user and see user whos log/ordernum matches what customer
                var customerId = User.Identity.GetUserId();
                var Customer = db.Users.Single(u => u.Id == customerId);
                // if log id exist...
                if (oCount < 1)
                {
                    return RedirectToAction("Index", "Error");
                }
                else
                {
                    var ocustomerId = User.Identity.GetUserId();
                    var oCustomer = db.Users.Single(u => u.Id == ocustomerId);
                    DateTime startDateTime = DateTime.Today; //Today at 00:00:00
                    DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                    var myNum = (from mn in db.UserLogs
                                 where oCustomer.Id == mn.Customer.Id
                                 && mn.LogDate >= startDateTime
                                 && mn.LogDate <= endDateTime
                                 select mn).ToList();
                    var todayUCount = myNum.Count();
                    var oNewNum = (from nn in myNum select nn.Id).Last();
                    if (todayUCount == 0)
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    //if the current customer matches the logid stamped with the user that logged in the database
                    var cLogNum = (from u in db.UserLogs where unum == u.Id select u.Id).Single();
                    var cUser = (from u in db.UserLogs where unum == u.Id select u.Customer).Single();
                    if ((cUser != Customer) || (cLogNum != unum) || (unum < oNewNum))
                    {
                        return RedirectToAction("Index", "Error");
                    }

                    //get all shirt count
                    var resultCount = (from rc in db.Shirts select rc).Count();
                    //get active shirt count
                    var activeCount = (from ac in db.Shirts where ac.SiteActive == false select ac).Count();

                    //return no shirts if there are no shirts
                    if (resultCount == activeCount)
                    {
                        return RedirectToAction("NoShirts", new { unum = unum });
                    }

                }
                var activeShirts = (from acs in db.Shirts where acs.SiteActive == true select acs);
                var shirts = activeShirts.Include(s => s.Files);
                ViewBag.LogNum = unum;
                ViewBag.TabTitle = "Available Shirts";
                return View(shirts.ToList());
            }
        }
        
        [AuthLog(Roles = "Administrators")]
        public async Task<ActionResult> ShirtList()
        {
            var Shirts = (from sh in db.Shirts select sh).OrderByDescending(x => x.Id);
            return View(await Shirts.ToListAsync());
        }

        // GET: Shirts/Details/5
        [AuthLog(Roles = "Administrators")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shirt shirt = db.Shirts.Include(s => s.Files).SingleOrDefault(s => s.Id == id);
            if (shirt == null)
            {
                return HttpNotFound();
            }
            return View(shirt);
        }

        // GET: Shirts/Create
        [AuthLog(Roles = "Administrators")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shirts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TabName,ShirtCause,Price,Caption,SiteActive,UpsizeCharge")] Shirt shirt, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var tshirt = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.TShirt,
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        tshirt.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    shirt.Files = new List<File> { tshirt };
                }
                db.Shirts.Add(shirt);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(shirt);
        }

        // GET: Shirts/Edit/5
        [AuthLog(Roles = "Administrators")]
        public async Task<ActionResult> Edit(int? id)
        {
            string newNum = Request.Params["unum"];
            int num = 0;
            bool res = int.TryParse(newNum, out num); 
            TempData["mynum"] = num;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shirt shirt = db.Shirts.Include(s => s.Files).SingleOrDefault(s => s.Id == id);
            if (shirt == null)
            {
                return HttpNotFound();
            }
            return View(shirt);
        }

        // POST: Shirts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TabName,ShirtCause,Price,Caption,SiteActive,UpsizeCharge")] Shirt shirt, HttpPostedFileBase upload)
        {
            //var shirtToUpdate = db.Shirts.Find(sid);
            if (ModelState.IsValid)
            {
                
                
                if (upload != null && upload.ContentLength > 0)
                {
                    db.Shirts.Attach(shirt);
                    db.Entry(shirt).Collection("Files").Load();
                    if (shirt.Files.Any(f => f.FileType == FileType.TShirt))
                    {
                        db.Files.Remove(shirt.Files.First(f => f.FileType == FileType.TShirt));
                    }
                    var tshirt = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.TShirt,
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        tshirt.Content = reader.ReadBytes(upload.ContentLength);
                    }                    
                    shirt.Files = new List<File> { tshirt };
                }
                //shirtToUpdate.Price = shirt.Price;
                 
                db.Entry(shirt).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("ShirtList", new { unum = TempData["mynum"] });
            }
            return View(shirt);
        }

        // GET: Shirts/Delete/5
        [AuthLog(Roles = "Administrators")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shirt shirt = await db.Shirts.FindAsync(id);
            if (shirt == null)
            {
                return HttpNotFound();
            }
            return View(shirt);
        }

        // POST: Shirts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Shirt shirt = await db.Shirts.FindAsync(id);
            db.Shirts.Remove(shirt);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AuthLog(Roles = "Administrators")]
        public ActionResult ToggleActive(int? id)
        {
            Shirt shirt = db.Shirts.Find(id);
            if (shirt.SiteActive == null)
            {
                shirt.SiteActive = true;
            }
            else if (shirt.SiteActive == true)
            {
                shirt.SiteActive = false;
            }
            else if (shirt.SiteActive == false)
            {
                shirt.SiteActive = true;
            }
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }
        
        [AuthLog(Roles = "Administrators")]
        public ActionResult ToggleUpCharge(int? id)
        {
            Shirt shirt = db.Shirts.Find(id);
            if (shirt.UpsizeCharge == null)
            {
                shirt.UpsizeCharge = true;
            }
            else if (shirt.UpsizeCharge == true)
            {
                shirt.UpsizeCharge = false;
            }
            else if (shirt.UpsizeCharge == false)
            {
                shirt.UpsizeCharge = true;
            }
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
