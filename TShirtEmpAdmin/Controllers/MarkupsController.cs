using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TShirtEmpAdmin.Models;

namespace TShirtEmpAdmin.Controllers
{
    public class MarkupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Markups
        public ActionResult Index()
        {
            return View(db.Markups.ToList());
        }

        // GET: Markups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Markup markup = db.Markups.Find(id);
            if (markup == null)
            {
                return HttpNotFound();
            }
            return View(markup);
        }

        // GET: Markups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Markups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Amount")] Markup markup)
        {
            if (ModelState.IsValid)
            {
                db.Markups.Add(markup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(markup);
        }

        // GET: Markups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Markup markup = db.Markups.Find(id);
            if (markup == null)
            {
                return HttpNotFound();
            }
            return View(markup);
        }

        // POST: Markups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Amount")] Markup markup)
        {
            string newNum = Request.Params["unum"];
            int num = 0;
            bool res = int.TryParse(newNum, out num);
            if (ModelState.IsValid)
            {
                db.Entry(markup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { unum = num });
            }
            return View(markup);
        }

        // GET: Markups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Markup markup = db.Markups.Find(id);
            if (markup == null)
            {
                return HttpNotFound();
            }
            return View(markup);
        }

        // POST: Markups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Markup markup = db.Markups.Find(id);
            db.Markups.Remove(markup);
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
