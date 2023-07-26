using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class GSTMaster
    {
        [Key]
        public int MasterId { get; set; }
        [Required(ErrorMessage ="GST Name is Required")]
        public string GSTName { get; set; }
        public decimal? GSTTaxValue { get; set; }
        public bool? IsDeleted { get; set; }
        public string GSTSource { get; set; }
    }
}
