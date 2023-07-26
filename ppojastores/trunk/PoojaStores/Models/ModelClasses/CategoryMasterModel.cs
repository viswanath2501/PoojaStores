using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class CategoryMasterModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name Required")]
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryImage { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? DisplayInHome { get; set; }
        public int? SequenceNumber { get; set; }
        public int CatCount { get; set; }
        public List<SeqNums> sequenceDrop { get; set; }
        public IFormFile ImageUpload { get; set; }
    }
    public class SeqNums
    {
        public int Seq { get; set; }
    }
}
