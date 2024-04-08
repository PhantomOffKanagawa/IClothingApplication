using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using IClothingApplication.Models;

namespace IClothingApplication.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();

        // GET: ShoppingCarts
        public ActionResult Index()
        {
            var shoppingCart = db.ShoppingCart.Include(s => s.Customer);
            return View(shoppingCart.ToList());
        }

        // GET: View
        public ActionResult ViewCart(string Message)
        {
            ViewBag.Message = Message;

            // If user isn an admin return to home
            if (Session["UserType"] == "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            // Get Cart
            ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session);

            if (shoppingCart == null)
                return View();
            IQueryable<ItemWrapper> itemWrapper = db.ItemWrapper.Where(s => (s.cartID.Equals(shoppingCart.cartID))).Include(p => p.Product);
            return View(itemWrapper.ToList());
        }

        // GET: ShoppingCarts/Details/5
        public ActionResult Details(int? id)
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

        // GET: ShoppingCarts/Create
        public ActionResult Create()
        {
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cartID,customerID")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.ShoppingCart.Add(shoppingCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", shoppingCart.customerID);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", shoppingCart.customerID);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cartID,customerID")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerID = new SelectList(db.Customer, "customerID", "customerName", shoppingCart.customerID);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            db.ShoppingCart.Remove(shoppingCart);
            db.SaveChanges();
            ViewBag.Message("Item deleted from cart.");
            return View("Index");
        }

        // HANDLE CUSTOMER EDITING/DELETING

        // GET: ItemWrappers/Edit/5
        public ActionResult EditItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get Shopping Cart
            ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session);

            // Find itemWrapper from IDs
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id, shoppingCart.cartID);
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
        public ActionResult EditItem([Bind(Include = "productID,cartID,productQty")] ItemWrapper itemWrapper)
        {
            if (ModelState.IsValid)
            {
                // If Quantity now 0 remove
                if (itemWrapper.productQty <= 0)
                {
                    var entry = db.Entry(itemWrapper);
                    if (entry.State == EntityState.Detached)
                        db.ItemWrapper.Attach(itemWrapper);
                    db.ItemWrapper.Remove(itemWrapper);
                    db.SaveChanges();

                }
                else
                {
                    // Else update
                    db.Entry(itemWrapper).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("ViewCart");
            }
            return View(itemWrapper);
        }

        // Handle Deleting From ViewCart

        // GET: ShoppingCarts/DeleteItem/5
        public ActionResult DeleteItem(int? id)
        {
            Debug.WriteLine("ID is " + id);
            Debug.WriteLine("UserType is " + (string)Session["UserType"]);

            if (id == null || (string)Session["UserType"] == "admin")
            {
                Debug.WriteLine("Did land in 404 tho");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get Cart
            ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session);

            // Find itemWrapper from IDs
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id, shoppingCart.cartID);
            if (itemWrapper == null)
            {
                return HttpNotFound();
            }
            return View(itemWrapper);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("DeleteItem")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItemConfirmed(int id)
        {
            // Get Cart ID
            ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session);

            // Find itemWrapper from IDs
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id, shoppingCart.cartID);

            db.ItemWrapper.Remove(itemWrapper);
            db.SaveChanges();
            return RedirectToAction("ViewCart");
        }

        // GET: ShoppingCarts/Checkout
        public ActionResult Checkout()
        {
            ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session);

            // If user isn't customer return to home
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Register", "Home", new { Message = "You need to register to check out" });
            }

            // Handle Shopping Cart
            if (shoppingCart == null)
                return View();
            IQueryable<ItemWrapper> itemWrapper = db.ItemWrapper.Where(s => (s.cartID.Equals(shoppingCart.cartID))).Include(p => p.Product);
            return View(itemWrapper.ToList());
        }

        // POST: ShoppingCarts/Checkout
        [HttpPost, ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckoutConfirmed()
        {
            return RedirectToAction("Billing");
        }

        // GET: ShoppingCarts/Billing
        public ActionResult Billing()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Home");
            int userID = (int)Session["UserID"];
            UserBilling userBilling = db.UserBilling.Where(u => u.customerID == userID).FirstOrDefault();
            return View(userBilling);
        }

        // POST: ShoppingCarts/Billing
        [HttpPost, ActionName("Billing")]
        [ValidateAntiForgeryToken]
        public ActionResult BillingConfirmed()
        {
            // Get Shopping Cart
            ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session);
            if (shoppingCart == null)
                return View();

            // Handle 0 Items
            IQueryable<ItemWrapper> itemWrapper = db.ItemWrapper.Where(s => (s.cartID.Equals(shoppingCart.cartID)));
            if (itemWrapper.Count() == 0)
            {
                return RedirectToAction("ViewCart", new { Message = "You Had Nothing In Your Cart!" });
            }

            // Handle Low Stock
            if (itemWrapper.Any(i => i.Product.productQty < i.productQty))
            {
                return RedirectToAction("ViewCart", new { Message = "A Product Is Too Low On Stock" });
            }

            // Update Product Stock
            foreach (var itemW in itemWrapper)
            {
                itemW.Product.productQty -= itemW.productQty;
                if (itemW.Product.productQty <= 0)
                {
                    foreach (var admin in db.Administrator)
                    {
                        AdminEmail adminEmail = new AdminEmail
                        {
                            emailDate = DateTime.Now,
                            emailSubject = $"Item Out Of Stock Alert ({itemW.Product.productName})",
                            emailBody = $"The {itemW.Product.productName} is out of stock.\nBrand: {itemW.Product.Brand.brandName}\nCategory: {itemW.Product.Category.categoryName}",
                            adminID = admin.adminID
                        };
                        db.AdminEmail.Add(adminEmail);
                    }
                }
            }
            db.SaveChanges();

            // Update Order Status
            OrderStatus orderStatus = db.OrderStatus.Find(shoppingCart.cartID);
            orderStatus.currentStatus = "paid"; // Maybe "confirmed" ??
            db.SaveChanges();

            // Attach Billing Details
            var userBilling = new UserBilling
            {
                customerID = null,
                cartID = (int)shoppingCart.cartID,
                cardNumber = shoppingCart.Customer.UserBilling.FirstOrDefault().cardNumber,
                cvv = shoppingCart.Customer.UserBilling.FirstOrDefault().cvv,
                expirationDate = shoppingCart.Customer.UserBilling.FirstOrDefault().expirationDate,
                billingDate = DateTime.Now
            };
            db.UserBilling.Add(userBilling);
            db.SaveChanges();

            // Replace Shopping Cart
            var newShoppingCart = new ShoppingCart();
            if (Session["UserID"] != null)
                newShoppingCart.customerID = (int)Session["UserID"];

            db.ShoppingCart.Add(newShoppingCart);
            db.SaveChanges();

            // Assign Order Status
            var newOrderStatus = new OrderStatus
            {
                cartID = (int)newShoppingCart.cartID,
                currentStatus = "none",
                statusDate = DateTime.Now
            };
            db.OrderStatus.Add(newOrderStatus);
            db.SaveChanges();

            return RedirectToAction("Index", "Home", new { Message = "You have successfully placed you order!" });
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
