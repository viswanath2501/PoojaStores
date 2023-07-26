using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class ProductDisplayModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public int? QtyAvailable { get; set; }
        public decimal? ActualPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string ProductCode { get; set; }
        public string SQUID { get; set; }
        public string BrandName { get; set; }
        public int? CategoryId { get; set; }
        public string Category_Name { get; set; }
        public int? SubcategoryId { get; set; }
        public string Subcategory_Name { get; set; }
        public int? DetailcatgoryId { get; set; }
        public string Detailcategory_Name { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public int? MeasurementMasterId { get; set; }
        public string Measurement_Name { get; set; }
        public int? PoojaItemMasterId { get; set; }
        public int? PoojaServiceMasterId { get; set; }
        public int? SpecialMasterId { get; set; }
        public int? ReturnPolicyId { get; set; }
        public int? DeliveryMasterId { get; set; }
        public int? DiscountMasterId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int? GSTMasterId { get; set; }
        public int? PostedBy { get; set; }
        public DateTime? PostedOn { get; set; }
        public int? ProductOwner { get; set; }
        public string ProductMainImage1 { get; set; }
        public string ProductMainImage2 { get; set; }
        public int? MinimumOrderQty { get; set; }
        public int? ViewCount { get; set; }
        public int? ReviewCount { get; set; }
        public decimal? CurrentRating { get; set; }
        public decimal? PackLength { get; set; }
        public decimal? PackWidth { get; set; }
        public decimal? PackHeight { get; set; }
        public decimal? ActualWeight { get; set; }
        public decimal? VolumetricWeight { get; set; }
        public decimal? PackingCharges { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public bool? IsFeatured { get; set; }
        public int cartCount { get; set; }
        public int CartId { get; set; }
        public bool isInCart { get; set; }
        public bool isInWishlist { get; set; }
        public decimal DiscountedCost { get; set; }
        public decimal? GSTTaxPercent { get; set; }
        public decimal GSTTaxValue { get; set; }
    }
    public class ProductDisplayModelBase
    {
        public List<ProductListModel> products { set; get; }
        public List<CategoryDrop> categoryDrops { get; set; }
        public List<SubCategoryDrop> subCategoryDrops { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
    public class ProductListModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public int? QtyAvailable { get; set; }
        public decimal? SellingPrice { get; set; }
        public string ProductCode { get; set; }
        public string BrandName { get; set; }
        public int? CategoryId { get; set; }
        public string Category_Name { get; set; }
        public int? SubcategoryId { get; set; }
        public string Measurement_Name { get; set; }
        public int? DiscountMasterId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string ProductMainImage1 { get; set; }
        public decimal? PackLength { get; set; }
        public decimal? PackWidth { get; set; }
        public decimal? PackHeight { get; set; }
        public string ProductColor { get; set; }
    }
    public class ProductsCountFromSQL
    {
        public int cnt { get; set; }
    }
    public class featureModelGroup
    {
        public List<ProductDisplayModel> fpros { get; set; }
    }
}
