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
    public class UserBillingsController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();

        // GET: UserBillings
        public ActionResult Index()
        {
            var userBilling = db.UserBilling.Include(u => u.Customer).Include(u => u.ShoppingCart);
            return View(userBilling.ToList());
        }

        // GET: UserBillings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBilling userBilling = db.UserBilling.Find(id);
            if (userBilling == null)
            {
                return HttpNotFound();
            }
            return View(userBilling);
        }

        // GET: UserBillings/Create
        public ActionResult Create()
        {
            if (Session["UserType"] == "customer")
            {
                ViewBag.customerIDVal = (int)Session["UserID"];
            }
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName");
            ViewBag.cartID = new SelectList(db.ShoppingCart, "cartID", "cartID");
            return View();
        }

        // POST: UserBillings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "billingID,customerID,cartID,cardNumber,expirationDate,cvv,billingDate")] UserBilling userBilling)
        {
            try {
                if (ModelState.IsValid)
                {
                    db.UserBilling.Add(userBilling);
                    db.SaveChanges();
                    if (Session["UserType"] == "customer")
                        return RedirectToAction("ViewAll", "Customers");
                    return RedirectToAction("Index");
                }

                if (Session["UserType"] == "customer")
                {
                    ViewBag.customerIDVal = (int)Session["UserID"];
                }

                ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userBilling.customerID);
                ViewBag.cartID = new SelectList(db.ShoppingCart, "cartID", "cartID", userBilling.cartID);
                return View(userBilling);
            }
            catch
            {
                ViewBag.Message = "UserBilling can only have one parent (CustomerID or CartID)";
                if (Session["UserType"] == "customer")
                {
                    ViewBag.customerIDVal = (int)Session["UserID"];
                }

                ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userBilling.customerID);
                ViewBag.cartID = new SelectList(db.ShoppingCart, "cartID", "cartID", userBilling.cartID);
                return View(userBilling);
            }

        }

        // GET: UserBillings/Edit/5
        public ActionResult Edit(int? id)
        {
            UserBilling userBilling = null;
            if (id != null)
            {
                userBilling = db.UserBilling.Find(id);
            }
            
            if (userBilling == null)
            {
                int userID = (int)Session["UserID"];
                if (Session["UserType"] == "customer")
                {
                    ViewBag.customerIDVal = (int)Session["UserID"];
                    return RedirectToAction("Create");
                }
                else
                {
                    return HttpNotFound();
                }
            }

            if (Session["UserType"] == "customer")
            {
                ViewBag.customerIDVal = (int)Session["UserID"];
            }

            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userBilling.customerID);
            ViewBag.cartID = new SelectList(db.ShoppingCart, "cartID", "cartID", userBilling.cartID);
            return View(userBilling);
        }

        // POST: UserBillings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "billingID,customerID,cartID,cardNumber,expirationDate,cvv,billingDate")] UserBilling userBilling)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userBilling).State = EntityState.Modified;
                db.SaveChanges();
                if (TempData.ContainsKey("ActionName"))
                {
                    return RedirectToAction((string) TempData["ActionName"], (string) TempData["ControllerName"]);
                }
                return RedirectToAction("Index");
            }

            if (Session["UserType"] == "customer")
            {
                ViewBag.customerIDVal = (int)Session["UserID"];
            }

            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", userBilling.customerID);
            ViewBag.cartID = new SelectList(db.ShoppingCart, "cartID", "cartID", userBilling.cartID);
            return View(userBilling);
        }

        // GET: UserBillings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBilling userBilling = db.UserBilling.Find(id);
            if (userBilling == null)
            {
                return HttpNotFound();
            }
            return View(userBilling);
        }

        // POST: UserBillings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserBilling userBilling = db.UserBilling.Find(id);
            db.UserBilling.Remove(userBilling);
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
