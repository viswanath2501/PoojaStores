using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.ModelClasses
{
    
        public class CategoryDrop
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string CategoryImage { get; set; }
            public int? SequenceNumber { get; set; }
        //public List<ProductsDisplaySales> products { get; set; }
    }

        public class SubCategoryDrop
        {
            public int SubCategoryId { get; set; }
            public string SubCategoryName { get; set; }
            public int ProductCount { get; set; }
        }
        public class CategoriesWithSub
        {
            public int CategoryId { get; set; }
            public String CategoryName { get; set; }
            public string CategoryImage { get; set; }
            public int ProductsCount { get; set; }
            public List<SubCategoryDrop> SubCats { get; set; }
            public int subCount { get; set; }
        }
        public class UserTypeDrop
        {
            public int UserTypeId { get; set; }
            public string UserTypeName { get; set; }
        }
        public class CityDrop
        {
            public int CityId { get; set; }
            public string CityName { get; set; }
        }
        public class CountryDrop
        {
            public int CountryId { get; set; }
            public string CountryName { get; set; }
        }
        public class StateDrop
        {
            public int StateId { get; set; }
            public string StateName { get; set; }
        }
      public class DetailCategoryDrop
      {
        public int DetailCategoryId { get; set; }
        public string DetailCategoryName { get; set; }
      }
    public class GSTDrop
    {
        public int MasterId { get; set; }
        public string GSTName { get; set; }
        public decimal? GSTTaxValue { get; set; }
    }
    public class MeasurementDrop
    {
        public int MeasurementId { get; set; }
        public string MeasurementName { get; set; }
    }
    public class PojaItemDrop
    {
        public int PrId { get; set; }
        public string ItemName { get; set; }
    }
    public class PojaServiceDrop
    {
        public int PrId { get; set; }
        public string ServiceName { get; set; }
    }
    public class SpecialityDrop
    {
        public int PrId { get; set; }
        public string SpecialityName { get; set; }
    }
    public class DiscountDrop
    {
        public int DiscountId { get; set; }
        public string DiscountName { get; set; }
        public decimal? DiscountPercentage { get; set; }
    }
    public class DeliveryDrop
    {
        public int DeliveryId { get; set; }
        public string DeliveryType { get; set; }
        
    }
    public class AddressTypesDrop
    {
        public int Id { get; set; }
        public string AddressTypeName { get; set; }
    }
    public class AddressTypeDrops
    {
        public int addressTypeId { get; set; }
        public string AddressTypeName { get; set; }
    }

}
