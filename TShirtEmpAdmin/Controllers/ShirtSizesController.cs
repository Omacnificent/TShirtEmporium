using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TShirtEmpAdmin.Models;
using TShirtEmpAdmin.CustomFilters;


namespace TShirtEmpAdmin.Controllers
{
    [AuthLog(Roles = "Administrators")]
    public class ShirtSizesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShirtSizes
        public async Task<ActionResult> Index(int? id)
        {
            return View(await db.ShirtSizes.ToListAsync());
        }

        // GET: ShirtSizes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShirtSize shirtSize = await db.ShirtSizes.FindAsync(id);
            if (shirtSize == null)
            {
                return HttpNotFound();
            }
            return View(shirtSize);
        }

        // GET: ShirtSizes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShirtSizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] ShirtSize shirtSize)
        {
            if (ModelState.IsValid)
            {
                db.ShirtSizes.Add(shirtSize);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(shirtSize);
        }

        // GET: ShirtSizes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShirtSize shirtSize = await db.ShirtSizes.FindAsync(id);
            if (shirtSize == null)
            {
                return HttpNotFound();
            }
            return View(shirtSize);
        }

        // POST: ShirtSizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] ShirtSize shirtSize)
        {
            string newNum = Request.Params["unum"];
            int num = 0;
            bool res = int.TryParse(newNum, out num);
            if (ModelState.IsValid)
            {
                db.Entry(shirtSize).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { unum = num });
            }
            return View(shirtSize);
        }

        // GET: ShirtSizes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShirtSize shirtSize = await db.ShirtSizes.FindAsync(id);
            if (shirtSize == null)
            {
                return HttpNotFound();
            }
            return View(shirtSize);
        }

        // POST: ShirtSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ShirtSize shirtSize = await db.ShirtSizes.FindAsync(id);
            db.ShirtSizes.Remove(shirtSize);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: ShirtSizes/ToggleActive/5
        public async Task<ActionResult> ToggleActive(int? id)
        {            
            ShirtSize shirtSize = await db.ShirtSizes.FindAsync(id);
            if (shirtSize.Active == null)
            {
                shirtSize.Active = true;
                TempData["click"] = "Hide";
            }
            else if (shirtSize.Active == true)
            {
                shirtSize.Active = false;
                TempData["click"] = "Show";
            }
            else if (shirtSize.Active == false)
            {
                shirtSize.Active = true;
                TempData["click"] = "Hide";
            }
            await db.SaveChangesAsync();
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
