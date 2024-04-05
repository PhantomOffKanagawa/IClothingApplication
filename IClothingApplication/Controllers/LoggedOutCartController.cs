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

            // Check if logged in
            if (!loggedIn(Session))
            {
                // Check if not logged in but have cart
                if (Session == null || Session["CartID"] == null)
                {
                    // Give User Shopping Cart
                    var shoppingCart = new ShoppingCart { };
                    db.ShoppingCart.Add(shoppingCart);
                    db.SaveChanges();

                    Session["CartID"] = shoppingCart.cartID;

                    // Give User Order Status
                    var orderStatus = new OrderStatus
                    {
                        cartID = (int)shoppingCart.cartID,
                        status = "unattached",
                        statusDate = new DateTime()
                    };
                    db.OrderStatus.Add(orderStatus);
                    db.SaveChanges();


                    return shoppingCart;
                }
                else
                {
                    ShoppingCart shoppingCart = db.ShoppingCart.Find((int)Session["CartID"]);
                    return shoppingCart;
                }
            }
            else
            {
                int userID = (int) Session["UserID"];
                ShoppingCart shoppingCart = db.ShoppingCart.Where(s => s.customerID != null && s.customerID ==  userID).Where(c => c.OrderStatus.status.Equals("none")).FirstOrDefault();

                return shoppingCart;
            }
        }
    }
}
