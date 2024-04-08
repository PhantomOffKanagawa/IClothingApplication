using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using IClothingApplication.Models;
using Newtonsoft.Json.Linq;

namespace IClothingApplication.Controllers
{
    public class ProductsController : Controller
    {
        private ICLOTHINGEntities db = new ICLOTHINGEntities();
        private bool IsChildOfDepartment(Category category, int departmentID)
        {
            if (category.parentDepartmentID == departmentID)
            {
                Debug.WriteLine(category.categoryName + " was under department");
                return true;
            }
            else if (category.parentCategoryID != null)
            {
                var parentCategory = db.Category.FirstOrDefault(c => c.categoryID == category.parentCategoryID);
                if (parentCategory != null)
                {
                    return IsChildOfDepartment(parentCategory, departmentID);
                }
            }

            Debug.WriteLine(category.categoryName + " was not under department");
            return false;
        }

        // GET: Products
        // Supports Sorting
        public ActionResult Index(string sortOrder, string filter, string filterType, string searchString, bool? changeSort, string Message)
        {
            // Pass on Message if exists
            if (Message != null)
            {
                ViewBag.Message = Message;
            }

            // Pass DB items for drop-downs
            var brands = db.Brand.ToList().OrderBy(s => s.brandName);
            ViewBag.Collections_Brands = new SelectList(brands, "brandName", "brandName");
            var departments = db.Department.ToList().OrderBy(s => s.departmentName);
            ViewBag.Collections_Departments = new SelectList(departments, "departmentName", "departmentName");
            var categories = db.Category.ToList().OrderBy(s => s.categoryName);
            ViewBag.Collections_Categories = new SelectList(categories, "categoryName", "categoryName");


            // Get products
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
                switch(filterType)
                {
                    case "Department":
                        // If department get all category ID's under said department recursively
                        Debug.WriteLine(filter + " current looking for");
                        var department = db.Department.FirstOrDefault(d => d.departmentName.Equals(filter));

                        if (department != null)
                        {
                            var childCategories = db.Category.ToList().Where(c => IsChildOfDepartment(c, department.departmentID)).ToList();
                            var categoryIds = childCategories.Select(c => c.categoryID).ToList();
                            products = products.Where(p => categoryIds.Any(c => p.categoryID.Equals(c)));
                        }
                        ViewBag.description = department.departmentDescription;
                        break;
                    case "Category":
                        products = products.Where(p => (p.Category.categoryName.Equals(filter)));
                        ViewBag.description = db.Category.Where(b => b.categoryName == filter).FirstOrDefault().categoryDescription;
                        break;
                    case "Brand":
                        products = products.Where(p => (p.Brand.brandName.Equals(filter)));
                        ViewBag.description = db.Brand.Where(b => b.brandName == filter).FirstOrDefault().brandDescription;
                        break;
                }
            }
            ViewBag.filter = filter;
            ViewBag.filterType = filterType;

            // Handle Sorting
            if (changeSort == null || (bool) changeSort)
            {
                // Toggle sort order as held in memory
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
                ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "quantity_desc" : "Quantity";
                ViewBag.BrandSortParm = sortOrder == "Brand" ? "brand_desc" : "Brand";
                ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            }
            ViewBag.sortOrder = sortOrder;

            // Switch to decide what sort to do
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

        // New method to handle Ajax request
        [HttpPost]
        public JsonResult DetailsAjax(int? id)
        {
            Debug.WriteLine("Ajax Inside with " + id);
            Product product = db.Product.Find(id);
            Debug.WriteLine("Ajax Inside with " + product.productName);
            string imageSrc = System.IO.File.Exists(Server.MapPath("~/Images/Products/" + product.productName + ".jpg")) ? "Products/" + product.productName + ".jpg" : "placeholder.jpg";

            return Json(new { imageSrc = imageSrc, id = product.productID, name = product.productName, description = product.productDescription, price = product.productPrice, qty = product.productQty, brand = product.Brand.brandName, category = product.Category.categoryName });
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            Debug.WriteLine("Hit Details");
            if (Request.IsAjaxRequest())
            {
                Debug.WriteLine("Ajax");
                // Call the new method if this is an Ajax request
                return DetailsAjax(id);
            }
            else
            {
                Debug.WriteLine("Normal");
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

        // New method to handle Ajax request
        [HttpPost]
        public JsonResult AddToCartAjax(int? id)
        {
            Product product = db.Product.Find(id);

            // Product didn't have an issue
            // Write to DB
            if (ModelState.IsValid)
            {
                // Try for already in cart error
                try
                {
                    var wrapper = new ItemWrapper();
                    wrapper.productID = (int)id;
                    wrapper.productQty = 1; //Hard-Coded

                    // Get cart for id
                    ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session);
                    wrapper.cartID = shoppingCart.cartID;

                    // Add Wrapper
                    db.ItemWrapper.Add(wrapper);
                    db.SaveChanges();

                    Debug.WriteLine(wrapper.Product.productName + " Added To Cart");
                    //ViewBag.Message = wrapper.Product.productName + " Added To Cart";
                    return Json(new { Message = wrapper.Product.productName + " Added To Cart" });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + "\n" + ex.ToString());
                    Debug.WriteLine("Item in cart");
                    //ViewBag.Message = "Item Already In Cart";
                    return Json(new { Message = "Item Already In Cart" });
                }
            }
            return Json(new { Message = "Error" });
        }


        // GET: Products/Add/5
        // Working need to add error handling
        public ActionResult Add(int? id, string sortOrder, string filter, string filterType, string searchString, bool? changeSort)
        {
            if (Request.IsAjaxRequest())
            {
                // Call the new method if this is an Ajax request
                return AddToCartAjax(id);
            }
            else
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
                    // Try for already in cart error
                    try
                    {
                        var wrapper = new ItemWrapper();
                        wrapper.productID = (int)id;
                        wrapper.productQty = 1; //Hard-Coded

                        // Get cart for id
                        ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session);
                        wrapper.cartID = shoppingCart.cartID;

                        // Add Wrapper
                        db.ItemWrapper.Add(wrapper);
                        db.SaveChanges();

                        Debug.WriteLine(wrapper.Product.productName + " Added To Cart");
                        //ViewBag.Message = wrapper.Product.productName + " Added To Cart";
                        return RedirectToAction("Index", "Products", new { Message = wrapper.Product.productName + " Added To Cart", sortOrder = sortOrder, filter = filter, filterType = filterType, searchString = searchString, changeSort = changeSort });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + "\n" + ex.ToString());
                        Debug.WriteLine("Item in cart");
                        //ViewBag.Message = "Item Already In Cart";
                        return RedirectToAction("Index", "Products", new { Message = "Item Already In Cart", sortOrder = sortOrder, filter = filter, filterType = filterType, searchString = searchString, changeSort = changeSort });
                    }
                }
                return View(product);
            }
        }
    }
}
