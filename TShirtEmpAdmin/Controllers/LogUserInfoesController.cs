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
    public class LogUserInfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LogUserInfoes
        public ActionResult Index()
        {
            return View(db.LogUserInfos.ToList());
        }

        // GET: LogUserInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogUserInfo logUserInfo = db.LogUserInfos.Find(id);
            if (logUserInfo == null)
            {
                return HttpNotFound();
            }
            return View(logUserInfo);
        }

        // GET: LogUserInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LogUserInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmpID,LastName,FirstName,PrefName,Position,Company,CostCenter,DeptName,Email,Manager,VP")] LogUserInfo logUserInfo)
        {
            if (ModelState.IsValid)
            {
                db.LogUserInfos.Add(logUserInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(logUserInfo);
        }

        // GET: LogUserInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogUserInfo logUserInfo = db.LogUserInfos.Find(id);
            if (logUserInfo == null)
            {
                return HttpNotFound();
            }
            return View(logUserInfo);
        }

        // POST: LogUserInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpID,LastName,FirstName,PrefName,Position,Company,CostCenter,DeptName,Email,Manager,VP")] LogUserInfo logUserInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(logUserInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(logUserInfo);
        }

        // GET: LogUserInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogUserInfo logUserInfo = db.LogUserInfos.Find(id);
            if (logUserInfo == null)
            {
                return HttpNotFound();
            }
            return View(logUserInfo);
        }

        // POST: LogUserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LogUserInfo logUserInfo = db.LogUserInfos.Find(id);
            db.LogUserInfos.Remove(logUserInfo);
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
