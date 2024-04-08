using System;
using System.ComponentModel.DataAnnotations;

namespace IClothingApplication.Models
{
    // Customer constraints
    public class DateInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateValue = (DateTime)value;
                DateTime maxDate = DateTime.Now.AddTicks(1);

                if (dateValue > maxDate)
                {
                    return new ValidationResult($"The date must not be in the future.");
                }
            }

            return ValidationResult.Success;
        }
    }

    // Constraints
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer
    { }

    public class CustomerMetadata
    {
        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(255, ErrorMessage = "Customer name cannot be greater than 255 characters")]
        public string customerName { get; set; }

        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(255, ErrorMessage = "Email cannot be greater than 255 characters")]
        public string customerEmail { get; set; }

        [Required(ErrorMessage = "Shipping address is required")]
        [StringLength(255, ErrorMessage = "Shipping address cannot be greater than 255 characters")]
        public string customerShippingAddress { get; set; }

        [Required(ErrorMessage = "Billing address is required")]
        [StringLength(255, ErrorMessage = "Billing address cannot be greater than 255 characters")]
        public string customerBillingAddress { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateInFuture(ErrorMessage = "Date of birth must not be in the future.")]
        public Nullable<DateTime> customerDOB { get; set; }

        [StringLength(10, ErrorMessage = "Gender cannot be greater than 10 characters")]
        public string customerGender { get; set; }
    }

    [MetadataType(typeof(AdminMetadata))]
    public partial class Administrator
    { }

    public class AdminMetadata
    {
        [Required(ErrorMessage = "Admin name is required")]
        [StringLength(255, ErrorMessage = "Admin name cannot be greater than 255 characters")]
        public string adminName { get; set; }

        [Required(ErrorMessage = "Admin email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(255, ErrorMessage = "Email cannot be greater than 255 characters")]
        public string adminEmail { get; set; }

        [Display(Name = "Date Hired")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DateInFuture(ErrorMessage = "Date of birth must not be in the future.")]
        public Nullable<DateTime> dateHired { get; set; }
    }

    [MetadataType(typeof(UserPasswordMetadata))]
    public partial class UserPassword
    { }

    public class UserPasswordMetadata
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(255, ErrorMessage = "Username cannot be greater than 255 characters")]
        public string userAccountName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Email cannot be greater than 255 characters")]
        public string userEncryptedPassword { get; set; }

        public int passwordExpiryTime { get; set; }

        [Display(Name = "Account Expiry Date")]
        [DataType(DataType.DateTime)]
        [DateInFuture(ErrorMessage = "Expiry date must not be in the future.")]
        public Nullable<DateTime> userAccountExpiryDate { get; set; }
    }

    [MetadataType(typeof(AboutUsMetadata))]
    public partial class AboutUs
    { }

    public class AboutUsMetadata
    {
        [Required(ErrorMessage = "Company address is required")]
        [StringLength(255, ErrorMessage = "Company address cannot be greater than 255 characters")]
        public string companyAddress { get; set; }

        [Required(ErrorMessage = "Company shipping policy is required")]
        public string companyShippingPolicy { get; set; }

        [Required(ErrorMessage = "Company return policy is required")]
        public string companyReturnPolicy { get; set; }

        [Required(ErrorMessage = "Company contact info is required")]
        public string companyContactInfo { get; set; }

        [Required(ErrorMessage = "Company business description is required")]
        public string companyBusinessDescription { get; set; }
    }

    [MetadataType(typeof(UserQueryMetadata))]
    public partial class UserQuery
    { }

    public class UserQueryMetadata
    {

        [Display(Name = "Query Date")]
        [DataType(DataType.Date)]
        [DateInFuture(ErrorMessage = "Query date must not be in the future.")]
        public Nullable<DateTime> queryDate { get; set; }

        [Required(ErrorMessage = "Query is required")]
        [StringLength(255, ErrorMessage = "Query cannot be greater than 255 characters")]
        public string queryDescription { get; set; }
    }

    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department
    { }

    public class DepartmentMetadata
    {
        [Required(ErrorMessage = "Department name is required")]
        [StringLength(255, ErrorMessage = "Department name cannot be greater than 255 characters")]
        public string departmentName { get; set; }

        [Required(ErrorMessage = "Department description is required")]
        [StringLength(255, ErrorMessage = "Deparment description cannot be greater than 255 characters")]
        public string departmentDescription { get; set; }
    }

    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    { }

    public class CategoryMetadata
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(255, ErrorMessage = "Category name cannot be greater than 255 characters")]
        public string categoryName { get; set; }

        [StringLength(255, ErrorMessage = "Category description cannot be greater than 255 characters")]
        public string categoryDescription { get; set; }
    }

    [MetadataType(typeof(BrandMetadata))]
    public partial class Brand
    { }

    public class BrandMetadata
    {
        [Required(ErrorMessage = "Brand name is required")]
        [StringLength(255, ErrorMessage = "Brand name cannot be greater than 255 characters")]
        public string brandName { get; set; }

        [StringLength(255, ErrorMessage = "Brand description cannot be greater than 255 characters")]
        public string brandDescription { get; set; }
    }

    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    { }

    public class ProductMetadata
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(255, ErrorMessage = "Product name cannot be greater than 255 characters")]
        public string productName { get; set; }

        [StringLength(255, ErrorMessage = "Product description cannot be greater than 255 characters")]
        public string productDescription { get; set; }

        [Required(ErrorMessage = "Product price is required")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal productPrice { get; set; }

        [Required(ErrorMessage = "Product quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int productQty { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int categoryID { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public int brandID { get; set; }
    }

    [MetadataType(typeof(UserCommentsMetadata))]
    public partial class UserComments
    { }

    public class UserCommentsMetadata
    {
        [Display(Name = "Comment Date")]
        [Required(ErrorMessage = "Comment date is required")]
        [DataType(DataType.Date)]
        [DateInFuture(ErrorMessage = "Expiry date must not be in the future.")]
        public DateTime commentDate { get; set; }

        [Required(ErrorMessage = "Comment description is required")]
        [StringLength(255, ErrorMessage = "Comment description cannot be greater than 255 characters")]
        public string commentDescription { get; set; }

        [Required(ErrorMessage = "Customer ID is required")]
        public int customerID { get; set; }
    }

    [MetadataType(typeof(ShoppingCartMetadata))]
    public partial class ShoppingCart
    { }

    public class ShoppingCartMetadata
    { }

    [MetadataType(typeof(ItemWrapperMetadata))]
    public partial class ItemWrapper
    { }

    public class ItemWrapperMetadata
    {
        [Required(ErrorMessage = "Product ID is required")]
        public int productID { get; set; }

        [Required(ErrorMessage = "Cart ID is required")]
        public int cartID { get; set; }


        [Required(ErrorMessage = "Product quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be negative")]
        public int productQty { get; set; }
    }

    [MetadataType(typeof(OrderStatusMetadata))]
    public partial class OrderStatus
    { }

    public class OrderStatusMetadata
    {
        [Required(ErrorMessage = "Cart ID is required")]
        public int cartID { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(255, ErrorMessage = "Status cannot be greater than 255 characters")]
        public string status { get; set; }


        [Display(Name = "Status Date")]
        [Required(ErrorMessage = "Status date is required")]
        [DataType(DataType.Date)]
        [DateInFuture(ErrorMessage = "Status date must not be in the future.")]
        public DateTime statusDate { get; set; }
    }

    [MetadataType(typeof(EmailMetadata))]
    public partial class Email
    { }

    public class EmailMetadata
    {
        [Display(Name = "Email Date")]
        [Required(ErrorMessage = "Email date is required")]
        [DataType(DataType.Date)]
        [DateInFuture(ErrorMessage = "Email date must not be in the future.")]
        public DateTime emailDate { get; set; }

        [Required(ErrorMessage = "Email subject is required")]
        [StringLength(255, ErrorMessage = "Email subject cannot be greater than 255 characters")]
        public string emailSubject { get; set; }

        [Required(ErrorMessage = "Email body is required")]
        public string emailBody { get; set; }

        [Required(ErrorMessage = "Customer ID is required")]
        public int customerID { get; set; }

        [Required(ErrorMessage = "Admin ID is required")]
        public int adminID { get; set; }
    }

    [MetadataType(typeof(ItemDeliveryMetadata))]
    public partial class ItemDelivery
    { }

    public class ItemDeliveryMetadata
    {
        [Required(ErrorMessage = "Cart ID is required")]
        public int cartID { get; set; }

        [Display(Name = "Sticker Date")]
        [Required(ErrorMessage = "Sticker date is required")]
        [DataType(DataType.Date)]
        public DateTime stickerDate { get; set; }
    }
}