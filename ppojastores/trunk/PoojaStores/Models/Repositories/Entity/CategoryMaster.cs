using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class CategoryMaster
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Category Name Required")]
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryImage { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? DisplayInHome { get; set; }
        public int? SequenceNumber { get; set; }
    }
}
