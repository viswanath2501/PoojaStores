using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class MeasurementMaster
    {
        [Key]
        public int MeasurementId { get; set; }
        [Required(ErrorMessage ="Measurement Name Reqiured")]
        public string MeasurementName { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
