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
    public class AdminEmailsController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();

        // GET: AdminEmails
        public ActionResult Index()
        {
            var adminEmail = db.AdminEmail.Include(a => a.Administrator);
            return View(adminEmail.ToList());
        }

        // GET: AdminEmails
        public ActionResult PersonalIndex()
        {
            int adminID = (int)Session["UserID"];
            if (Session["UserType"] != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var adminEmail = db.AdminEmail.Include(a => a.Administrator).Where(ae => ae.adminID == adminID);
            return View(adminEmail.ToList());
        }

        // GET: AdminEmails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminEmail adminEmail = db.AdminEmail.Find(id);
            if (adminEmail == null)
            {
                return HttpNotFound();
            }
            return View(adminEmail);
        }

        // GET: AdminEmails/Create
        public ActionResult Create()
        {
            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName");
            return View();
        }

        // POST: AdminEmails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "emailNo,emailDate,emailSubject,emailBody,adminID")] AdminEmail adminEmail)
        {
            if (ModelState.IsValid)
            {
                db.AdminEmail.Add(adminEmail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", adminEmail.adminID);
            return View(adminEmail);
        }

        // GET: AdminEmails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminEmail adminEmail = db.AdminEmail.Find(id);
            if (adminEmail == null)
            {
                return HttpNotFound();
            }
            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", adminEmail.adminID);
            return View(adminEmail);
        }

        // POST: AdminEmails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emailNo,emailDate,emailSubject,emailBody,adminID")] AdminEmail adminEmail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminEmail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", adminEmail.adminID);
            return View(adminEmail);
        }

        // GET: AdminEmails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminEmail adminEmail = db.AdminEmail.Find(id);
            if (adminEmail == null)
            {
                return HttpNotFound();
            }
            return View(adminEmail);
        }

        // POST: AdminEmails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminEmail adminEmail = db.AdminEmail.Find(id);
            db.AdminEmail.Remove(adminEmail);
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
