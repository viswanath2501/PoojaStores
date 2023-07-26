using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class PoFollowUp
    {
        [Key]
        public int FollowUpId { get; set; }
        public int? FollowUpBy { get; set; }
        public DateTime? FolloUpOn { get; set; }
        public string FollowUpRemarks { get; set; }
        public int? POMID { get; set; }
        public bool? isDeleted { get; set; }
    }
}
