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

            // If user isn't customer return to home
            if (Session["UserType"] != "customer")
            {
                return RedirectToAction("Index", "Home");
            }

            // Get user ID
            var userID = (int) Session["UserID"];
            // Get Shopping Cart
            var shoppingCart = db.ShoppingCart.Where(s => (s.customerID.Equals(userID))).Where(c => c.OrderStatus.status.Equals("none")).FirstOrDefault();
            if (shoppingCart == null)
                return View();
                IQueryable<ItemWrapper> itemWrapper = db.ItemWrapper.Where(s => (s.cartID.Equals(shoppingCart.cartID))).Include(p => p.Product);
                return View(itemWrapper.ToList());
        }

        // GET: ShoppingCarts/Checkout
        public ActionResult Checkout()
        {
            //ShoppingCart shoppingCart = db.ShoppingCart.Find(id);
            //if (shoppingCart == null)
            //{
            //    return HttpNotFound();
            //}
            //return View();

            // If user isn't customer return to home
            if (Session["UserType"] != "customer")
            {
                return RedirectToAction("Index", "Home");
            }

            // Get user ID
            var userID = (int)Session["UserID"];
            // Get Shopping Cart
            var shoppingCart = db.ShoppingCart.Where(s => (s.customerID.Equals(userID))).Where(c => c.OrderStatus.status.Equals("none")).FirstOrDefault();
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
            // Get user ID
            var userID = (int)Session["UserID"];
            // Get Shopping Cart
            var shoppingCart = db.ShoppingCart.Where(s => (s.customerID.Equals(userID))).Where(c => c.OrderStatus.status.Equals("none")).FirstOrDefault();
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
                // Send Admin Email
                return RedirectToAction("ViewCart", new { Message = "A Product Is Too Low On Stock" });
            }

            OrderStatus orderStatus = db.OrderStatus.Find(shoppingCart.cartID);
            orderStatus.status = "paid"; // Maybe "confirmed" ??
            db.SaveChanges();

            // Replace Shopping Cart
            var newShoppingCart = new ShoppingCart
            {
                customerID = userID
            };
            db.ShoppingCart.Add(newShoppingCart);
            db.SaveChanges();

            // Assign Order Status
            var newOrderStatus = new OrderStatus
            {
                cartID = (int)newShoppingCart.cartID,
                status = "none",
                statusDate = new DateTime()
            };
            db.OrderStatus.Add(newOrderStatus);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index");
        }

        // HANDLE CUSTOMER EDITING/DELETING

        // GET: ItemWrappers/Edit/5
        public ActionResult EditItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Get Cart ID
            var userID = (int)Session["userID"];
            var cartID = (int)db.ShoppingCart.Where(s => (s.customerID.Equals(userID))).Where(c => c.OrderStatus.status.Equals("none")).FirstOrDefault().cartID;

            // Find itemWrapper from IDs
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id, cartID);
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
                db.Entry(itemWrapper).State = EntityState.Modified;
                db.SaveChanges();
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

            if (id == null || (string)Session["UserType"] != "customer")
            {
                Debug.WriteLine("Did land in 404 tho");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get Cart ID
            var userID = (int) Session["userID"];
            var cartID = (int)db.ShoppingCart.Where(s => (s.customerID.Equals(userID))).Where(c => c.OrderStatus.status.Equals("none")).FirstOrDefault().cartID;

            // Find itemWrapper from IDs
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id, cartID);
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
            var userID = (int)Session["userID"];
            var cartID = db.ShoppingCart.Where(s => (s.customerID.Equals(userID))).Where(c => c.OrderStatus.status.Equals("none")).FirstOrDefault().cartID;

            // Find itemWrapper from IDs
            ItemWrapper itemWrapper = db.ItemWrapper.Find(id, cartID);

            db.ItemWrapper.Remove(itemWrapper);
            db.SaveChanges();
            return RedirectToAction("ViewCart");
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
