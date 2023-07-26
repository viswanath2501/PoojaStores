using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class DetailCategoryMaster
    {
        [Key]
        public int DetailCategoryId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        [Required(ErrorMessage ="DetailCategory Name id Required")]
        public string DetailCategoryName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
