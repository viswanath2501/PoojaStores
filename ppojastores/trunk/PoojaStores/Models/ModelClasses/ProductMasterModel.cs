using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class ProductMasterModel
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product Title Required")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid Name ")]
        public string ProductTitle { get; set; }
        [Required(ErrorMessage = "Product Description is Required")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "QtyAvailable Required")]
        [RegularExpression("[0-9]*", ErrorMessage = "Please enter a positive number. ")]
        public int? QtyAvailable { get; set; }
        [Required(ErrorMessage = "Actual Price Required")]
        public decimal? ActualPrice { get; set; }
        [Required(ErrorMessage = "Selling Price Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive number.")]
        public decimal? SellingPrice { get; set; }
        [Required(ErrorMessage = "ProductCode Required")]
        [RegularExpression("[A-Za-z0-9 ]*", ErrorMessage = "Invalid Name ")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "SQUID Required")]
        [RegularExpression("[0-9]*", ErrorMessage = "Invalid SQUID ")]
        public string SQUID { get; set; }
        [Required(ErrorMessage = "BrandName Required")]
        [RegularExpression("[A-Za-z]*", ErrorMessage = "Invalid Name ")]
        public string BrandName { get; set; }
        
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? SubcategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int? DetailcatgoryId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public int? MeasurementMasterId { get; set; }
        public string MeasurementName { get; set; }
        public int? PoojaItemMasterId { get; set; }
        public string PoojaItemName { get; set; }
        public int? PoojaServiceMasterId { get; set; }
        public string PoojaServiceName { get; set; }
        public int? SpecialMasterId { get; set; }
        public string SpecialityName { get; set; }
        public int? ReturnPolicyId { get; set; }
        public int? DeliveryMasterId { get; set; }
        public string DeliveryName { get; set; }
        public int? DiscountMasterId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountedCost { get; set; }
        public int? GSTMasterId { get; set; }
        public string GSTName { get; set; }
        public decimal GSTValue { get; set; }
        public int? PostedBy { get; set; }
        public DateTime? PostedOn { get; set; }
        public int? ProductOwner { get; set; }
      
        public string ProductMainImage1 { get; set; }
        public string ProductMainImage2 { get; set; }
        [Required(ErrorMessage = "MinimumOrderQty Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive number.")]
        public int? MinimumOrderQty { get; set; }
        public int? ViewCount { get; set; }
        public int? ReviewCount { get; set; }
        public decimal? CurrentRating { get; set; }
        [Required(ErrorMessage = "PackLength Required")]
        [RegularExpression("[0-9]*", ErrorMessage = "Invalid PackLength ")]
        public decimal? PackLength { get; set; }
        [Required(ErrorMessage = "PackWidth Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive number.")]
        public decimal? PackWidth { get; set; }
        [Required(ErrorMessage = "PackHeight Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive number.")]
        public decimal? PackHeight { get; set; }
        [Required(ErrorMessage = "ActualWeight Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive number.")]
        public decimal? ActualWeight { get; set; }
        [Required(ErrorMessage = "VolumetricWeight Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive number.")]

        public decimal? VolumetricWeight { get; set; }
        public decimal? PackingCharges { get; set; }
        [Required(ErrorMessage = "ProductColor Required")]
        [RegularExpression("[A-Za-z]*", ErrorMessage = "Please enter a ProductColor  Name ")]
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public bool? IsFeatured { get; set; }

        // other fields
        [Required(ErrorMessage ="PrimaryImage Required")]
        public IFormFile ProductMainImageUploaded { get; set; }
        [Required(ErrorMessage = "PrimaryImage2 Required")]
        public IFormFile ProductMainImage2Uploaded { get; set; }
        public List<IFormFile> ProductOtherImgsUploaded { get; set; }
        public List<string> OtherImages  { get; set; }
        //[Required(ErrorMessage = "OtherImages Required")]
        public List<CategoryDrop> categoryDrops { get; set; }
        public List<SubCategoryDrop> subCategoryDrops { get; set; }
        public List<GSTDrop> gstDrop { get; set; }
        public List<MeasurementDrop> measurementDrops { get; set; }
        public List<PojaItemDrop> pojaItemDrops { get; set; }
        public List<PojaServiceDrop> pojaServiceDrops { get; set; }
        public List<SpecialityDrop> specialityDrops { get; set; }
        public List<DiscountDrop> discountDrops { get; set; }
        public List<DeliveryDrop> deliveryDrops { get; set; }
    }
}
