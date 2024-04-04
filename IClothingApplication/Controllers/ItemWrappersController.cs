using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IClothingApplication.Models;

namespace IClothingApplication.Controllers
{
    public class ItemWrappersController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();

        // GET: ItemWrappers
        public ActionResult Index()
        {
            return View(db.ItemWrapper.ToList());
        }

        // GET: ItemWrappers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id);
            if (itemWrapper == null)
            {
                return HttpNotFound();
            }
            return View(itemWrapper);
        }

        // GET: ItemWrappers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemWrappers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productID,cartID,productQty")] ItemWrapper itemWrapper)
        {
            if (ModelState.IsValid)
            {
                db.ItemWrapper.Add(itemWrapper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(itemWrapper);
        }

        // GET: ItemWrappers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id);
            if (itemWrapper == null)
            {
                return HttpNotFound();
            }
            return View(itemWrapper);
        }

        // POST: ItemWrappers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productID,cartID,productQty")] ItemWrapper itemWrapper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemWrapper).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemWrapper);
        }

        // GET: ItemWrappers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id);
            if (itemWrapper == null)
            {
                return HttpNotFound();
            }
            return View(itemWrapper);
        }

        // POST: ItemWrappers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id);
            db.ItemWrapper.Remove(itemWrapper);
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
