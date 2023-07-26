using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class ProductImages
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageType { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ProductId { get; set; }
    }
}
