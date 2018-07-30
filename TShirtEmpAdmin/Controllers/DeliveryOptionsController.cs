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
    public class DeliveryOptionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DeliveryOptions
        public ActionResult Index()
        {
            return View(db.DeliveryOptions.ToList());
        }

        // GET: DeliveryOptions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryOption deliveryOption = db.DeliveryOptions.Find(id);
            if (deliveryOption == null)
            {
                return HttpNotFound();
            }
            return View(deliveryOption);
        }

        // GET: DeliveryOptions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeliveryOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Active")] DeliveryOption deliveryOption)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryOptions.Add(deliveryOption);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryOption);
        }

        // GET: DeliveryOptions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryOption deliveryOption = db.DeliveryOptions.Find(id);
            if (deliveryOption == null)
            {
                return HttpNotFound();
            }
            return View(deliveryOption);
        }

        // POST: DeliveryOptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Active")] DeliveryOption deliveryOption)
        {
            string newNum = Request.Params["unum"];
            int num = 0;
            bool res = int.TryParse(newNum, out num);
            if (ModelState.IsValid)
            {
                db.Entry(deliveryOption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { unum = num });
            }
            return View(deliveryOption);
        }

        // GET: ShirtQuantities/ToggleActive/5
        public ActionResult ToggleActive(int? id)
        {
            DeliveryOption deliveryOption = db.DeliveryOptions.Find(id);
            if (deliveryOption.Active == null)
            {
                deliveryOption.Active = true;
                TempData["click"] = "Hide";
            }
            else if (deliveryOption.Active == true)
            {
                deliveryOption.Active = false;
                TempData["click"] = "Show";
            }
            else if (deliveryOption.Active == false)
            {
                deliveryOption.Active = true;
                TempData["click"] = "Hide";
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
