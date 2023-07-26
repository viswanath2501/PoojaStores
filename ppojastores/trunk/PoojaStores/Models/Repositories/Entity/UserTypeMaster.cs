using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class UserTypeMaster
    {
        [Key]
        public int TypeId { get; set; }
        [Required(ErrorMessage ="Type Name is Required")]
        public string TypeName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
