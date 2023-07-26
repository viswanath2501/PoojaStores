using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class DetailCategoryModel
    {
        public int DetailCategoryId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        [Required(ErrorMessage = "DetailCategory Name id Required")]
        public string DetailCategoryName { get; set; }
        public List<CategoryDrop> catDrops { get; set; }
        public List<SubCategoryDrop> subCatDrops { get; set; }
        public List<DetailCategoryMaster> DetailedDetails { get; set; }
    }
}
