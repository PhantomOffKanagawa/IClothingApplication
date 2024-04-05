﻿using System;
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
    public class ProductsController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();
        
        // GET: Products
        // Supports Sorting
        public ActionResult Index(string sortOrder, string filter, string searchString, bool? changeFilter)
        {
            Debug.WriteLine(searchString);
            var products = from p in db.Product
                           select p;

            // Handle Searching
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.productName.Contains(searchString)
                                       || s.productName.Contains(searchString));
            }
            ViewBag.searchString = searchString;

            // Handle Filtering
            // ! Get Working with Sorting
            if (!String.IsNullOrEmpty(filter))
            {
                products = products.Where(p => (p.Brand.brandName.Equals(filter)));
            }
            ViewBag.filter = filter;

            // Handle Sorting
            if (changeFilter != null && (bool) changeFilter)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
                ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
                ViewBag.BrandSortParm = sortOrder == "Brand" ? "brand_desc" : "Brand";
                ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            }
            ViewBag.sortOrder = sortOrder;

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.productName);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.productPrice);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.productPrice);
                    break;
                case "Quantity":
                    products = products.OrderBy(s => s.productQty);
                    break;
                case "quantity_desc":
                    products = products.OrderByDescending(s => s.productQty);
                    break;
                case "Brand":
                    products = products.OrderBy(s => s.Brand.brandName);
                    break;
                case "brand_desc":
                    products = products.OrderByDescending(s => s.Brand.brandName);
                    break;
                case "Category":
                    products = products.OrderBy(s => s.Category.categoryName);
                    break;
                case "category_desc":
                    products = products.OrderByDescending(s => s.Category.categoryName);
                    break;
                default:
                    products = products.OrderBy(s => s.productName);
                    break;
            }
            return View(products.ToList());
        }

        // GET: Products
        //public ActionResult Index()
        //{
        //    var brands = db.Brand.ToList();
        //    ViewBag.Brands = new SelectList(brands, "brandID", "brandName"); // Pass the brands to the view
        //    //return View();
        //    var product = db.Product.Include(p => p.Brand).Include(p => p.Category);
        //    return View(product.ToList());
        //}

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.brandID = new SelectList(db.Brand, "brandID", "brandName");
            ViewBag.categoryID = new SelectList(db.Category, "categoryID", "categoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productID,productName,productDescription,productPrice,productQty,categoryID,brandID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.brandID = new SelectList(db.Brand, "brandID", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.Category, "categoryID", "categoryName", product.categoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.brandID = new SelectList(db.Brand, "brandID", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.Category, "categoryID", "categoryName", product.categoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productID,productName,productDescription,productPrice,productQty,categoryID,brandID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brandID = new SelectList(db.Brand, "brandID", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.Category, "categoryID", "categoryName", product.categoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
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

        // GET: Products/Add/5
        // Working need to add error handling
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Product didn't have an issue
            // Write to DB
            if (ModelState.IsValid)
            {
                var wrapper = new ItemWrapper();
                wrapper.productID = (int) id;
                wrapper.productQty = 1; //Hard-Coded
                var userID = (int)Session["UserID"];
                var shoppingCart = db.ShoppingCart.Include(s => s.Customer).Where(s => s.customerID.Equals(userID)).First();
                wrapper.cartID = shoppingCart.cartID;
                db.ItemWrapper.Add(wrapper);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
