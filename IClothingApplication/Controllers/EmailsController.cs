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
    public class EmailsController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();

        // GET: Emails
        public ActionResult Index()
        {
            var email = db.Email.Include(e => e.Administrator).Include(e => e.Customer);
            return View(email.ToList());
        }

        // GET: Emails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Email.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            return View(email);
        }

        // GET: Emails/Create
        public ActionResult Create()
        {
            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName");
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName");
            return View();
        }

        // POST: Emails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "emailNo,emailDate,emailSubject,emailBody,customerID,adminID")] Email email)
        {
            if (ModelState.IsValid)
            {
                db.Email.Add(email);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", email.adminID);
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", email.customerID);
            return View(email);
        }

        // GET: Emails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Email.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", email.adminID);
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", email.customerID);
            return View(email);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emailNo,emailDate,emailSubject,emailBody,customerID,adminID")] Email email)
        {
            if (ModelState.IsValid)
            {
                db.Entry(email).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", email.adminID);
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", email.customerID);
            return View(email);
        }

        // GET: Emails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Email.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            return View(email);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Email email = db.Email.Find(id);
            db.Email.Remove(email);
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
