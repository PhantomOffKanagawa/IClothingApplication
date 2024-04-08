using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IClothingApplication.Models;

namespace IClothingApplication.Controllers
{
    public class UserPasswordsController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();

        // GET: UserPasswords
        public ActionResult Index()
        {
            var userPassword = db.UserPassword.Include(u => u.Administrator).Include(u => u.Customer);
            return View(userPassword.ToList());
        }

        // GET: UserPasswords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPassword.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            return View(userPassword);
        }

        // GET: UserPasswords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserPasswords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,customerID,adminID,userAccountName,userEncryptedPassword,passwordExpiryTime,userAccountExpiryDate")] UserPassword userPassword)
        {
            if (ModelState.IsValid)
            {
                db.UserPassword.Add(userPassword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", userPassword.adminID);
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userPassword.customerID);
            return View(userPassword);
        }

        // GET: UserPasswords/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPassword.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", userPassword.adminID);
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userPassword.customerID);
            return View(userPassword);
        }

        // POST: UserPasswords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "customerID,adminID,userAccountName,userEncryptedPassword,passwordExpiryTime,userAccountExpiryDate")] UserPassword userPassword)
        {
            Debug.WriteLine("Got Edit Return");
            if (ModelState.IsValid)
            {
            Debug.WriteLine("Got Edit Return Was Valid");
                db.Entry(userPassword).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Debug.WriteLine("Got Edit Failed");
            ViewBag.adminID = new SelectList(db.Administrator, "adminID", "adminName", userPassword.adminID);
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userPassword.customerID);
            return View(userPassword);
        }

        // GET: UserPasswords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPassword userPassword = db.UserPassword.Find(id);
            if (userPassword == null)
            {
                return HttpNotFound();
            }
            return View(userPassword);
        }

        // POST: UserPasswords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserPassword userPassword = db.UserPassword.Find(id);
            db.UserPassword.Remove(userPassword);
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
