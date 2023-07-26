using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class CategoriesCount
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryCount { get; set; }
        public List<SubCatCount> SubCsts { get; set; }
    }
    public class SubCatCount
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public int SubCategoryCount { get; set; }
    }
}
