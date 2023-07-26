using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class SpecialMaster
    {
        [Key]
        public int PrId { get; set; }
        [Required(ErrorMessage = "Speciality Name is Required")]
        public string SpecialityName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
