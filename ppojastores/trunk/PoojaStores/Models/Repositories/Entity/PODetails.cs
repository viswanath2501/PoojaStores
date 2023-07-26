using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class PODetails
    {
        [Key]
        public int PODetailId { get; set; }
        public int? POMasterId { get; set; }
        public int? ProductId { get; set; }
        public int? NumberOfItems { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? GST { get; set; }
        public decimal? Discount { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime? AddedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string OrderId { get; set; }
        public string PaymentStatus { get; set; }
        public bool? RetunStatus { get; set; }
        public bool? CancelStatus { get; set; }
        public DateTime? RefundedOn { get; set; }
        public decimal? RefundedAmount { get; set; }
        public bool? RefundStatus { get; set; }
    }
}
