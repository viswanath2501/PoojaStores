using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class DiscountMaster
    {
        [Key]
        public int DiscountId { get; set; }
        [Required(ErrorMessage ="Discount Name is Required")]
        public string DiscountName { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
