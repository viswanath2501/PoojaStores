using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class CartDetails
    {
        [Key]
        public int CartDetailId { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
        public string Image1 { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public int? NumberProducts { get; set; }
        public decimal? GStPrice { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime AdededOn { get; set; }
    }
}
