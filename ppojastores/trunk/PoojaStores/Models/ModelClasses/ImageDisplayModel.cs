using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class ImageDisplayModel
    {
        public int ImageId { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
        public int? RelatedCategoryId { get; set; }
        public int? ImageNumber { get; set; }
        [Required(ErrorMessage = "DiscountPercentage Required")]
        [RegularExpression("[0-9]*", ErrorMessage = "Invalid DiscountPercentages ")]
        public int? DiscountPercent { get; set; }
        [Required(ErrorMessage = "ImageTitle1 Required")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid ImageTitle1 ")]
        public string ImageTitle1 { get; set; }
        [Required(ErrorMessage = "ImageTitle2 Required")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid ImageTitle2 ")]
        public string ImageTitle2 { get; set; }
        public string ImageDescription { get; set; }
        public int? OldCost { get; set; }
        public int? NewCost { get; set; }
        [Required(ErrorMessage = "Starting Price Required")]
        [RegularExpression("[0-9]*", ErrorMessage = "Invalid Starting Price ")]
        public int? StartingAt { get; set; }
        [Required(ErrorMessage = "TextOnButton Required")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Invalid TextOnButton ")]
        public string TextOnButton { get; set; }
        public string ImageSize { get; set; }

        public IFormFile CategoryImageUpload { get; set; }

        public List<CategoryDrop> CategoryDrops { get; set; }
    }
}
