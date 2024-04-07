using IClothingApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.Entity;
using System.Diagnostics;

namespace IClothingApplication.Controllers
{
    namespace IClothingApplication.Models
    {
        public class HomeController : Controller
        {
            private ICLOTHINGEntities db = new ICLOTHINGEntities();

            public ActionResult Index()
            {
                var aboutUs = db.AboutUs.Include(a => a.Administrator);
                ViewData["address"] = aboutUs.ToList().ElementAt(0).companyAddress;
                var product = db.Product.Include(p => p.Brand).Include(p => p.Category);

                // Pass Departments to Temp Data for Nav Bar
                Session["departments"] = db.Department.ToList();

                return View(product.ToList());
            }

            public ActionResult Search(string searchString)
            {
                // Query brand based on the search string
                var product = from p in db.Product
                             select p;

                if (!string.IsNullOrEmpty(searchString))
                {
                    product = product.Where(s => s.productName.Contains(searchString));
                }

                return View(product.ToList());
            }

            // GET: AboutUs
            public ActionResult About()
            {
                var aboutUs = db.AboutUs.Include(a => a.Administrator);
                return View(aboutUs.ToList());
            }

            // GET: Dashboard
            public ActionResult Dashboard()
            {
                if (Session["UserType"] == "customer")
                {
                    return RedirectToAction("CustomerDashboard");
                }
                else if (Session["UserType"] == "admin")
                {
                    return RedirectToAction("AdminDashboard");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            [AllowAnonymous]
            public ActionResult Register(string Message)
            {
                if (Message != null)
                {
                    ViewBag.Message = Message;
                }

                return View();
            }

            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public ActionResult Register(Customer model)
            {
                if (ModelState.IsValid)
                {
                    Customer customer = db.Customer.Add(model);
                    db.SaveChanges();
                    TempData["userType"] = "customer";
                    TempData["userID"] = customer.customerID;
                    return View("RegisterPassword");
                }

                ViewBag.Message = "Error";
                return View("Register");

            }

            [AllowAnonymous]
            public ActionResult RegisterPassword()
            {
                return RedirectToAction("Register");
            }

            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public ActionResult RegisterPassword(UserPassword model)
            {
                if (ModelState.IsValid)
                {
                    Debug.WriteLine("Returned Form");
                    try
                    {
                        Debug.WriteLine("Started Try");
                        model.passwordExpiryTime = 100;
                        model.userAccountExpiryDate = new DateTime().AddDays(100);
                        model.customerID = ((int?)(TempData["userType"] == "customer" ? TempData["userID"] : null));
                        model.adminID = ((int?)(TempData["userType"] == "admin" ? TempData["userID"] : null));
                        Debug.WriteLine("Filled Details");
                        Debug.WriteLine(model.customerID);
                        db.UserPassword.Add(model);
                        db.SaveChanges();

                        // Attach Shopping Cart
                        ShoppingCart shoppingCart = LoggedOutCartController.getCart(Session, db);
                        shoppingCart.customerID = (int)TempData["userID"];

                        //Debug.WriteLine("The id was: " + cart.customerID);
                        //db.ShoppingCart.Add(shoppingCart);
                        db.Entry(shoppingCart).State = EntityState.Modified;
                        db.SaveChanges();

                        // Update OrderStatus to be attached
                        shoppingCart.OrderStatus.status = "none";
                        shoppingCart.OrderStatus.statusDate = new DateTime();
                        db.Entry(shoppingCart.OrderStatus).State = EntityState.Modified;

                        //db.Entry(shoppingCart.OrderStatus).State = EntityState.Modified;
                        db.SaveChanges();


                        // Write Message to User
                        ViewBag.Message = "Successful Registration";
                        return View("Login");
                    } catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine(ex.InnerException);
                        TempData["userType"] = TempData["userType"];
                        TempData["userID"] = TempData["userID"];
                        ViewBag.Message = "Username already in use or other error";
                        return View("RegisterPassword");
                    }
                }

                ViewBag.Message = "Error";
                return View("Register");

            }

            public ActionResult Login()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Login(UserPassword objUser)
            {
                if (ModelState.IsValid)
                {
                    using (ICLOTHINGEntities db = new ICLOTHINGEntities())
                    {
                        var obj = db.UserPassword.Where(a => a.userAccountName.Equals(objUser.userAccountName) && a.userEncryptedPassword.Equals(objUser.userEncryptedPassword)).FirstOrDefault();
                        if (obj != null)
                        {
                            Session["UserName"] = obj.userAccountName.ToString();
                            if (obj.adminID != null)
                            {
                                Session["UserType"] = "admin";
                                Session["UserID"] = obj.adminID;
                                return RedirectToAction("AdminDashboard");
                            } else if (obj.customerID != null)
                            {
                                Session["UserType"] = "customer";
                                Session["UserID"] = obj.customerID;
                                return RedirectToAction("CustomerDashboard");
                            }
                        }
                    }
                }
                ViewBag.Message = "Invalid Username or Password";
                return View(objUser);
            }
            public ActionResult Logout()
            {
                Session.Abandon();
                Session.Clear();
                ViewBag.Title = "Logged Out";
                ViewBag.Message = "Successfully Logged Out";
                return View("Login");
            }

            public ActionResult AdminDashboard()
            {
                if (Session["UserType"] != null && Session["UserType"] == "admin")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            public ActionResult AdminManagementDashboard()
            {
                if (Session["UserType"] != null && Session["UserType"] == "admin")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            public ActionResult CustomerDashboard()
            {
                if (Session["UserType"] != null && Session["UserType"] == "customer")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
        }
    }
}