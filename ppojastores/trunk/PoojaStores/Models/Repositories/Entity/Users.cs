using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class Users
    {
        [Key]

        public int UserId { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage ="Please Enter First Name")]
        public string Firstname { get; set; }
        [Required(ErrorMessage ="Please Enter Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Please Enter EmailId")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string PWord { get; set; }
        [Required(ErrorMessage ="Please Enter Mobile Number")]
        public string MobileNumber { get; set; }
        public DateTime? RegisteredOn { get; set; }
        public DateTime? ActivatedOn { get; set; }
        public string CurrentStatus { get; set; }
        public int? UserTypeId { get; set; }
        public bool? IsEmailVerified { get; set; }
        public bool? IsDeleted { get; set; }
        public string ProfileImage { get; set; }
    }
}
