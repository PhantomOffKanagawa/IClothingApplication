using System;
using System.ComponentModel.DataAnnotations;

namespace IClothingApplication.Models
{
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    { }

    public class CustomerMetadata
    {
        [Required(ErrorMessage = "Customer name is required")]
        public string customerName { get; set; }

        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string customerEmail { get; set; }

        [Required(ErrorMessage = "Shipping address is required")]
        public string customerShippingAddress { get; set; }

        [Required(ErrorMessage = "Billing address is required")]
        public string customerBillingAddress { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> customerDOB { get; set; }

        public string customerGender { get; set; }
    }
}
