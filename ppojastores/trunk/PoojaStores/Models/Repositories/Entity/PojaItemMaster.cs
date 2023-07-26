using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class PojaItemMaster
    {
        [Key]
        public int PrId { get; set; }
        [Required(ErrorMessage ="Item Name is Required")]
        public string ItemName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
