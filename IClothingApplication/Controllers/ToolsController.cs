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
    public class ToolsController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();

        // GET: Tools
        public ActionResult AdminBillingManager(int allItems = 0)
        {
            if (Session["UserType"] != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var shoppingCart = db.ShoppingCart.Include(s => s.Customer).Include(s => s.OrderStatus);
            if (allItems == 0)
            {
                shoppingCart = shoppingCart.Where(s => s.OrderStatus.currentStatus == "paid");
            }

            ViewBag.allItems = (allItems == 1 ? 1 : 0);

            return View(shoppingCart.ToList());
        }

        public ActionResult CustomerOldOrders()
        {
            if (Session["UserType"] != "customer")
            {
                return RedirectToAction("Index", "Home");
            }
            int userID = (int)Session["UserID"];
            var shoppingCart = db.ShoppingCart.Where(s => s.OrderStatus.currentStatus != "none").Where(s => s.Customer.customerID == userID).Include(s => s.Customer).Include(s => s.OrderStatus);
            return View(shoppingCart.ToList());
        }

        // GET: Tools
        public ActionResult Index()
        {
            var shoppingCart = db.ShoppingCart.Include(s => s.Customer).Include(s => s.OrderStatus);
            return View(shoppingCart.ToList());
        }

        // GET: Tools/BillingDetails/5
        public ActionResult BillingDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // GET: Tools/CartDetails/5
        public ActionResult CartDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // Confirm Billing
        public ActionResult Confirm(int? id)
        {
            if (Session["UserType"] != "admin")
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }

            // Update Order Status
            OrderStatus orderStatus = db.OrderStatus.Find(shoppingCart.cartID);
            orderStatus.currentStatus = "shipped";
            db.SaveChanges();

            // Attach ItemDelivery
            ItemDelivery itemDelivery = new ItemDelivery
            {
                stickerDate = DateTime.Now,
                cartID = shoppingCart.cartID
            };
            db.ItemDelivery.Add(itemDelivery);
            db.SaveChanges();

            // Attach ItemDelivery
            ItemDelivery itemDelivery = new ItemDelivery
            {
                stickerDate = DateTime.Now,
                cartID = shoppingCart.cartID
            };
            db.ItemDelivery.Add(itemDelivery);
            db.SaveChanges();

            String itemString = "";
            foreach (var item in shoppingCart.ItemWrapper)
            {
                itemString += item.productQty + "x " + item.Product.productName + "\n";
            }


            Email email = new Email();
            email.emailSubject = "Your Order Has Been Confirmed";
            email.emailBody = "Your order of the following is confirmed and will be shipped shortly.\n" + itemString;
            email.customerID = shoppingCart.Customer.customerID;
            email.adminID = (int)Session["UserID"];
            email.emailDate = DateTime.Now;
            db.Email.Add(email);
            db.SaveChanges();

            AdminEmail adminEmail = new AdminEmail();
            adminEmail.emailDate = DateTime.Now;
            adminEmail.emailSubject = "You Confirmed " + shoppingCart.Customer.customerName + "'s Order Was Confirmed";
            adminEmail.emailBody = "Their order was confirmed. It Included: \n" + itemString;
            adminEmail.adminID = (int)Session["UserID"];
            db.AdminEmail.Add(adminEmail);
            db.SaveChanges();

            return RedirectToAction("AdminBillingManager");
        }

        // Confirm Billing
        public ActionResult Deny(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }

            // Update Product Stock
            foreach (var itemW in shoppingCart.ItemWrapper)
            {
                itemW.Product.productQty += itemW.productQty;
                db.SaveChanges();
            }

            // Update Order Status
            OrderStatus orderStatus = db.OrderStatus.Find(shoppingCart.cartID);
            orderStatus.currentStatus = "denied";
            db.SaveChanges();

            return RedirectToAction("AdminBillingManager");
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
