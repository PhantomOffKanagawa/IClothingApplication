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
    public class LoggedOutCartController : Controller
    {
        public static bool loggedIn(HttpSessionStateBase Session)
        {
            return (Session != null && Session["UserID"] != null);
        }

        public static ShoppingCart getCart(HttpSessionStateBase Session)
        {
            // Initialize Variables
            ICLOTHINGEntities db = new ICLOTHINGEntities();
            return getCart(Session, db);
        }

        public static ShoppingCart getCart(HttpSessionStateBase Session, ICLOTHINGEntities db)
        {
            // Check if logged in
            if (!loggedIn(Session))
            {
                Debug.WriteLine("Not Logged In");
                // Check if not logged in but have cart
                if (Session == null || Session["CartID"] == null)
                {
                    Debug.WriteLine("Making Cart");

                    // Give User Shopping Cart
                    var shoppingCart = new ShoppingCart { };
                    db.ShoppingCart.Add(shoppingCart);
                    db.SaveChanges();

                    Session["CartID"] = shoppingCart.cartID;

                    // Give User Order Status
                    var orderStatus = new OrderStatus
                    {
                        cartID = (int)shoppingCart.cartID,
                        currentStatus = "unattached",
                        statusDate = DateTime.Now
                    };
                    db.OrderStatus.Add(orderStatus);
                    db.SaveChanges();


                    return shoppingCart;
                }
                else
                {
                    Debug.WriteLine("Returning Cart By ID");
                    ShoppingCart shoppingCart = db.ShoppingCart.Find((int)Session["CartID"]);
                    return shoppingCart;
                }
            }
            else
            {
                Debug.WriteLine("Returning Cart By UserID");
                int userID = (int)Session["UserID"];
                ShoppingCart shoppingCart = db.ShoppingCart.Where(s => s.customerID != null && s.customerID == userID).Where(c => c.OrderStatus.currentStatus.Equals("none")).FirstOrDefault();

                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCart { 
                        customerID = userID
                    };

                    db.ShoppingCart.Add(shoppingCart);
                    db.SaveChanges();

                    Session["CartID"] = shoppingCart.cartID;

                    // Give User Order Status
                    var orderStatus = new OrderStatus
                    {
                        cartID = (int)shoppingCart.cartID,
                        currentStatus = "none",
                        statusDate = DateTime.Now
                    };
                    db.OrderStatus.Add(orderStatus);
                    db.SaveChanges();
                }

                return shoppingCart;
            }
        }
    }
}
