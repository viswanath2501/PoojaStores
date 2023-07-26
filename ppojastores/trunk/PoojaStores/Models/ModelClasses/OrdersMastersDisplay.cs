using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    public class OrdersMastersDisplay
    {
        public int POID { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string PaymentMethod { get; set; }
        public string CurrentStatus { get; set; }
        public decimal? OrderAmount { get; set; }
    }
    public class OrderDetailPage
    {
        public List<OrderDetailDisplay> odetails { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string LocationStreet { get; set; }
        public string LandMark { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
        public string FullName { get; set; }
        public string OrderNotes { get; set; }
    }
    public class OrderDetailDisplay
    {
        public int PODetailId { get; set; }
        public int? POMasterId { get; set; }
        public int? ProductId { get; set; }
        public int? NumberOfItems { get; set; }
        public string OrderId { get; set; }
        public string Image { get; set; }
        public string ProductCode { get; set; }
        public string DispatchedBy { get; set; }
        public DateTime? DispatchedOn { get; set; }
        public string DispatchNumber { get; set; }
        public DateTime? DeliveredOn { get; set; }
        public string CurrentStatus { get; set; }
        public string PaymentStatus { get; set; }
    }
    public class OrderDisplayForCustomer
    {
        public string OrderId { get; set; }
        public DateTime? AddedOn { get; set; }
        public decimal? TotalPrice { get; set; }
        public string CurrentStatus { get; set; }
        public int PODetailId { get; set; }
        public string Image { get; set; }
        public string ProductCode { get; set; }
        public int? ProductId { get; set; }
        public decimal? Discount { get; set; }
        public TimeSpan? orderspan { get; set; }
        public int orderdays { get; set; }
    }
    public class CustOrderDetail
    {
        public int PODetailId { get; set; }
        public string Image { get; set; }
        public string OrderId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal? TotalValue { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string LocationStreet { get; set; }
        public string LandMark { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
        public string FullName { get; set; }
        public string PaymentStatus { get; set; }
        public string InvoiceDocumentUrl { get; set; }
        public decimal? GST { get; set; }
        public decimal? UnitPrice { get; set; }
    }
    public class DispatchDetailModel
    {
        public int DisPatchId { get; set; }
        public int? PoDetailId { get; set; }        
        public string DispatchedThrough { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DispatchedBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Required")]
        public DateTime? DispatchedOn { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DispatchNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveredOn { get; set; }
        public IFormFile InvoiceDocument { get; set; }
    }
    public class RefundDetailsModel
    {
        public DateTime? orderedOn { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal CancelationCharges { get; set; }
        public string TransId { get; set; }
        public string order_id { get; set; }
    }
    public class DetailSmsModel
    {
        public string DispatchedBy { get; set; }
        public string MobileNumber { get; set; }
        public string OrderId { get; set; }
    }
}
