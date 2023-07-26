using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class ContactUs
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public DateTime? createdOn { get; set; }
        public string createdBy { get; set; }
        public string message { get; set; }
        public bool IsDeleted { get; set; }
    }
}
