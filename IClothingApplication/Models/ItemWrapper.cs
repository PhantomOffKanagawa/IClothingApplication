//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IClothingApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemWrapper
    {
        public int productID { get; set; }
        public int cartID { get; set; }
        public int productQty { get; set; }
    
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual Product Product { get; set; }
    }
}
