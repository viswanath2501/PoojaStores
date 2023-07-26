using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class WishList
    {
        [Key]
        public int WishListId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductDetailId { get; set; }
        public DateTime? AddedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
