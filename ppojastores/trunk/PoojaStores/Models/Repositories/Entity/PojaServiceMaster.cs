using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class PojaServiceMaster
    {
        [Key]
        public int PrId { get; set; }
        [Required(ErrorMessage ="Service Name is Required")]
        public string ServiceName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
