using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TShirtEmpAdmin.Models;
using TShirtEmpAdmin.ViewModels;
using System;
using System.Collections.Generic;
using TShirtEmpAdmin.CustomFilters;

namespace TShirtEmpAdmin.Controllers
{
    [AuthLog(Roles = "Administrators")]
    public class ShirtToSizesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        readonly IList<ShirtSize> ShirtSizes;

        public ShirtToSizesController()
        {
            var sizes = (from ss in db.ShirtSizes select ss).ToList();
            ShirtSizes = sizes;
        }

        // GET: ShirtToSizes
        public ActionResult Index(int? shirtId)
        {
            if (shirtId == null)
            {
                return RedirectToAction("ShirtList", "Shirts");
            }
            
            var result = (from res in db.Shirts where res.Id == shirtId select res).ToList();
            var shirtName = (from sn in result select sn.ShirtCause).Single();
            var fileName = (from fn in db.Files where fn.ShirtId == shirtId select fn.FileId).Single();

            ViewBag.FileName = fileName;
            ViewBag.ShirtName = shirtName;

            var viewModel = new ViewModels.ShirtsToSizesViewModel();
            viewModel.Files = db.Files.ToList();
            viewModel.ShirtSizes = ShirtSizes;
            viewModel.Shirts = db.Shirts.ToList();

            var selectCount = (from sc in db.ShirtToSizes select sc).Count();
            var SelectedSizes = (from sn in db.ShirtToSizes where sn.ShirtId == shirtId select sn).ToList();
            if (selectCount >= 1)
            {
                foreach (var item in SelectedSizes)
                {
                    var size = ShirtSizes.First(p => p.Id == item.ShirtSizeId);
                    size.IsSelected = true;
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ShirtsToSizesViewModel viewModel)
        {
            var selectedSizes = viewModel
                .ShirtSizes
                .Where(p => p.IsSelected);

            db.ShirtToSizes
                .Where(p => p.ShirtId == viewModel.ShirtId)
                .ToList()
                .ForEach(p=>db.ShirtToSizes.Remove(p));
            db.SaveChanges();
            
            var ShirtsToSizes = selectedSizes.Select(p => new ShirtToSize { 
                ShirtId = viewModel.ShirtId,
                ShirtSizeId = p.Id
            });
            db.ShirtToSizes.AddRange(ShirtsToSizes);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        

        // POST: ShirtToSizes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id")] ShirtToSize shirtToSize)
        {
            if (ModelState.IsValid)
            {
                db.ShirtToSizes.Add(shirtToSize);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shirtToSize);
        }

        // GET: ShirtToSizes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShirtToSize shirtToSize = db.ShirtToSizes.Find(id);
            if (shirtToSize == null)
            {
                return HttpNotFound();
            }
            return View(shirtToSize);
        }

        // POST: ShirtToSizes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] ShirtToSize shirtToSize)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shirtToSize).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shirtToSize);
        }

        // GET: ShirtToSizes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShirtToSize shirtToSize = db.ShirtToSizes.Find(id);
            if (shirtToSize == null)
            {
                return HttpNotFound();
            }
            return View(shirtToSize);
        }

        // POST: ShirtToSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShirtToSize shirtToSize = db.ShirtToSizes.Find(id);
            db.ShirtToSizes.Remove(shirtToSize);
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
