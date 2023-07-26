using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class HomePageImages
    {
        [Key]
        public int ImageId { get; set; }
        public string Image { get; set; }

        public int? RelatedCategoryId { get; set; }
        public int? ImageNumber { get; set; }
        public int? DiscountPercent { get; set; }
        public string ImageTitle1 { get; set; }
        public string ImageTitle2 { get; set; }
        public string ImageDescription { get; set; }
        public int? OldCost { get; set; }
        public int? NewCost { get; set; }
        public int? StartingAt { get; set; }
        public string TextOnButton { get; set; }
        public string ImageSize { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
