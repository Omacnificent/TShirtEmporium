using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TShirtEmpAdmin.CustomFilters;
using TShirtEmpAdmin.Models;

namespace TShirtEmpAdmin.Controllers
{
    public class ManageOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AuthLog(Roles = "Administrators")]
        // GET: ManageOrders
        public ActionResult Index(string sortOrder, string shirtName, string Name, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (shirtName != null)
            {
                page = 1;
            }
            else
            {
                Name = shirtName;
            }

            ViewBag.CurrentFilter = searchString;

            var shirtLst = new List<string>();
            var shirtQry = from sq in db.Shirts
                           where sq.SiteActive == true
                           orderby sq.ShirtCause
                           select sq.ShirtCause;
            shirtLst.AddRange(shirtQry.Distinct());
            ViewBag.shirtName = new SelectList(shirtLst);

            var result = db.Orders.GroupBy(x => x.OrderNum).Select(y => y.FirstOrDefault());

            //get all shirt count
            var resultCount = (from rc in db.Shirts select rc).Count();
            //get non-active shirt count
            var activeCount = (from ac in db.Shirts where ac.SiteActive == false select ac).Count();

            var test = from t in result select t;
            var myOrders = from o in db.Orders select o;
                    
            //return no shirts if there are no shirts
            if (resultCount != activeCount)
            {
                //var acs = (from ac in db.Shirts where ac.SiteActive == true select ac.ShirtCause).First();
                var acs = (from ac in db.Shirts where ac.SiteActive == true select ac.ShirtCause).ToList();
                test = (from t in result where acs.Contains(t.ShirtCause) select t);

                myOrders = from o in db.Orders where acs.Contains(o.ShirtCause) select o;
            }

            var OrderTotal = (from ot in myOrders where ot.OrderCompleted == true select ot.Total).DefaultIfEmpty().Sum();
            var OrderAbandoned = (from oa in myOrders where oa.OrderCompleted == false select oa.Total).DefaultIfEmpty().Sum();

            ViewBag.CompletedTotal = OrderTotal;
            ViewBag.OrderAbandoned = OrderAbandoned;
            
            switch (sortOrder)
            {
                case "OderNum_desc":
                    test = test.OrderByDescending(d => d.OrderNum);
                    break;
                default:
                    test = test.OrderBy(d => d.OrderNum);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                test = test.Where(y => y.EmployeeID.Equals(searchString)
                    || y.EmployeeID.Contains(searchString)
                    || y.FirstName.Equals(searchString)
                    || y.FirstName.Contains(searchString)
                    || y.LastName.Equals(searchString)
                    || y.LastName.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(shirtName))
            {
                test = test.Where(x => x.ShirtCause == shirtName);
            }


            var listTotal = (from lt in result select lt.Total).Sum();

            ViewBag.Total = listTotal;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(test.ToPagedList(pageNumber, pageSize));
        }
        
        public ActionResult Find(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Order order = db.Orders.Find(id);
            var result = (from r in db.Orders where id == r.OrderNum select r).ToList();
            var reCount = result.Count();
            ViewBag.Count = reCount;
            if (reCount < 1)
            {
                ViewBag.Message = "No Orders to Show";
            }
            else
            {
                var EmpId = (from ei in result select ei.EmployeeID).First();
                var FName = (from fn in result select fn.FirstName).First();
                var LName = (from ln in result select ln.LastName).First();
                var odnum = (from odn in result select odn.OrderNum).First();
                var Total = (from tot in result select tot.Total).Sum();

                ViewBag.EmpID = EmpId;
                ViewBag.Name = FName + " " + LName;
                ViewBag.Total = Total;
                ViewBag.Order = odnum;

            }
            
            return View(result);
        }


        // GET: ManageOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: ManageOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: ManageOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShirtCause,CreateDate,OShirtSize,OShirtQuantity,ODeliveryOption,Total,ShirtYear,OrderNum,OrderCompleted,RecievedShirt,EmployeeID,FirstName,LastName,Department,PhoneNumber,Email")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: ManageOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public ActionResult ToggleCompleted(int? id)
        {
            Order order = db.Orders.Find(id);
            if (order.OrderCompleted == null)
            {
                order.OrderCompleted = true;
            }
            else if (order.OrderCompleted == true)
            {
                order.OrderCompleted = false;
            }
            else if (order.OrderCompleted == false)
            {
                order.OrderCompleted = true;
            }
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public ActionResult ToggleRecieved(int? id)
        {
            Order order = db.Orders.Find(id);
            if (order.RecievedShirt == null)
            {
                order.RecievedShirt = true;
            }
            else if (order.RecievedShirt == true)
            {
                order.RecievedShirt = false;
            }
            else if (order.RecievedShirt == false)
            {
                order.RecievedShirt = true;
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
