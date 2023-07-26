using Microsoft.AspNetCore.Http;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class UserMasterDisplay
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string PWord { get; set; }
        public int? UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        
        public bool? IsDeleted { get; set; }
        
        public string ProfileImage { get; set; }
        

        public IFormFile ProfileImageUploaded { get; set; }

    }
}
