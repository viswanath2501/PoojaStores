using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class POMaster
    {
        [Key]
        public int POID { get; set; }
        public int? CartId { get; set; }
        public string TransactionId { get; set; }
        public int? CustomerId { get; set; }
        public string PONumber { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal? PaidAmount { get; set; }
        public string InstrumentDetails { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? RefundedOn { get; set; }
        public decimal? RefundedAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CurrentStatus { get; set; }
        public decimal? BankCharges { get; set; }
        public decimal? Taxes { get; set; }
        public decimal? BankTaxes { get; set; }
        public decimal? OrderAmount { get; set; }
        public decimal? Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public int addressId { get; set; }
        public string OrderNotes { get; set; }
    }
}
