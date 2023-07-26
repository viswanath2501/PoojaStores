using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Entity
{
    public class RazorPaymentResult
    {
        [Key]
        public int TRID { get; set; }
        public string TransId { get; set; }
        public string entity { get; set; }
        public int? amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string order_id { get; set; }
        public string invoice_id { get; set; }
        public bool? international { get; set; }
        public string method { get; set; }
        public int? amount_refunded { get; set; }
        public string refund_status { get; set; }
        public bool? captured { get; set; }
        public string description { get; set; }
        public string card_id { get; set; }
        public string bank { get; set; }
        public string wallet { get; set; }
        public string vpa { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public string notes { get; set; }
        public int? fee { get; set; }
        public int? tax { get; set; }
        public string error_code { get; set; }
        public string error_description { get; set; }
        public string error_source { get; set; }
        public string error_step { get; set; }
        public string error_reason { get; set; }
        public string acquirer_data { get; set; }
        public int? created_at { get; set; }
        public int? POID { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
