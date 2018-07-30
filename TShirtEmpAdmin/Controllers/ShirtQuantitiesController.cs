using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TShirtEmpAdmin.CustomFilters;
using TShirtEmpAdmin.Models;

namespace TShirtEmpAdmin.Controllers
{
    [AuthLog(Roles = "Administrators")]
    public class ShirtQuantitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShirtQuantities
        public ActionResult Index()
        {
            return View(db.ShirtQuantities.ToList());
        }

        // GET: ShirtQuantities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShirtQuantity shirtQuantity = db.ShirtQuantities.Find(id);
            if (shirtQuantity == null)
            {
                return HttpNotFound();
            }
            return View(shirtQuantity);
        }

        // GET: ShirtQuantities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShirtQuantities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] ShirtQuantity shirtQuantity)
        {
            if (ModelState.IsValid)
            {
                db.ShirtQuantities.Add(shirtQuantity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shirtQuantity);
        }

        // GET: ShirtQuantities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShirtQuantity shirtQuantity = db.ShirtQuantities.Find(id);
            if (shirtQuantity == null)
            {
                return HttpNotFound();
            }
            return View(shirtQuantity);
        }

        // POST: ShirtQuantities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] ShirtQuantity shirtQuantity)
        {
            string newNum = Request.Params["unum"];
            int num = 0;
            bool res = int.TryParse(newNum, out num);
            if (ModelState.IsValid)
            {
                db.Entry(shirtQuantity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { unum = num });
            }
            return View(shirtQuantity);
        }

        // GET: ShirtQuantities/ToggleActive/5
        public ActionResult ToggleActive(int? id)
        {
            ShirtQuantity shirtQuantity = db.ShirtQuantities.Find(id);
            if (shirtQuantity.Active == null)
            {
                shirtQuantity.Active = true;
                TempData["click"] = "Hide";
            }
            else if (shirtQuantity.Active == true)
            {
                shirtQuantity.Active = false;
                TempData["click"] = "Show";
            }
            else if (shirtQuantity.Active == false)
            {
                shirtQuantity.Active = true;
                TempData["click"] = "Hide";
            }
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        // POST: ShirtQuantities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShirtQuantity shirtQuantity = db.ShirtQuantities.Find(id);
            db.ShirtQuantities.Remove(shirtQuantity);
            db.SaveChanges();
            return RedirectToAction("Index");
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
