using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class DeliveryMaster
    {
        [Key]
        public int DeliveryId { get; set; }
        [Required(ErrorMessage ="Delivery Type Required")]
        public string DeliveryType { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
