using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class DispatchDetails
    {
        [Key]
        public int DisPatchId { get; set; }
        public int? PoDetailId { get; set; }
        public bool? IsDeleted { get; set; }        
        public string DispatchedThrough { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DispatchedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Required")]
        public DateTime? DispatchedOn { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DispatchNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveredOn { get; set; }
        public string InvoiceDocumentUrl { get; set; }
    }
}
