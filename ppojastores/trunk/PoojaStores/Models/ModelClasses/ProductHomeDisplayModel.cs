using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class ProductHomeDisplayModel
    {
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }        
        public string Title { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public decimal? SellingPrice { get; set; }       
        public decimal? Discount { get; set; }
        public decimal? GSTPrice { get; set; }
        public int MinimumOrderQty { get; set; }
        public int QtyAvailable { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public bool IsWishlist { get; set; }
        public bool IsCart { get; set; }
    }
    public class ProductGetModel
    {
        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Title { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountedPrice { get; set; }        
    }
}
