using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int? CartUserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CurrentStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int addressId { get; set; }
        public string OrderNotes { get; set; }
    }
}
