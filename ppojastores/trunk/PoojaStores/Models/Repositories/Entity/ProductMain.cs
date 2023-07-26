using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class ProductMain
    {
        [Key]
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
        public int? SubcategoryId { get; set; }
        public int? DetailcatgoryId { get; set; }
        public bool? IsDeleted { get; set; }
        public string CurrentStatus { get; set; }
        public int? MeasurementMasterId { get; set; }
        public int? PoojaItemMasterId { get; set; }
        public int? PoojaServiceMasterId { get; set; }
        public int? SpecialMasterId { get; set; }
        public int? ReturnPolicyId { get; set; }
        public int? DeliveryMasterId { get; set; }
        public int? DiscountMasterId { get; set; }
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
    }
}
