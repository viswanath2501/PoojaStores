using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class CommonDropsMgmtRepo : ICommonDropsMgmtRepo
    {
        private readonly MyDbContext context;
        public CommonDropsMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }
        public List<CategoryDrop> GetCatsDrop()
        {
            return context.categoryMasters.Where(a => a.IsDeleted == false)
                .Select(b => new CategoryDrop
                {
                    CategoryId = b.CategoryId,
                    CategoryName = b.CategoryName,
                    CategoryImage=b.CategoryImage,
                    SequenceNumber=b.SequenceNumber
                })
                .OrderBy(b => b.SequenceNumber).ToList();
        }
        public List<SubCategoryDrop> GetSubCatsDrop(int id)
        {
            return context.subCategoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == id)
                .Select(b => new SubCategoryDrop
                {
                    SubCategoryId = b.SubCategoryId,
                    SubCategoryName = b.SubCategoryName
                })
                .OrderBy(b => b.SubCategoryName).ToList();
        }
        public List<DetailCategoryDrop> GetDetailCatsDrop(int id)
        {
            return context.detailCategoryMasters.Where(a => a.IsDeleted == false && a.SubCategoryId==id)
                .Select(b => new DetailCategoryDrop
                {
                    DetailCategoryId = b.DetailCategoryId,
                    DetailCategoryName = b.DetailCategoryName
                })
                .OrderBy(b => b.DetailCategoryName).ToList();
        }
        public List<GSTDrop> GetGSTDrop()
        {
            return context.gSTMasters.Where(a => a.IsDeleted == false)
                .Select(b => new GSTDrop
                {
                    MasterId = b.MasterId,
                    GSTTaxValue=b.GSTTaxValue,
                    GSTName = b.GSTName
                })
                .OrderBy(b => b.GSTName).ToList();
        }
        public List<MeasurementDrop> GetMeasurementDrop()
        {
            return context.measurementMasters.Where(a => a.IsDeleted == false)
                .Select(b => new MeasurementDrop
                {
                    MeasurementId = b.MeasurementId,
                    MeasurementName = b.MeasurementName
                    
                })
                .OrderBy(b => b.MeasurementName).ToList();
        }
        public List<PojaItemDrop> GetPojaItemDrop()
        {
            return context.pojaItemMasters.Where(a => a.IsDeleted == false)
                .Select(b => new PojaItemDrop
                {
                    PrId = b.PrId,
                    ItemName = b.ItemName

                })
                .OrderBy(b => b.ItemName).ToList();
        }
        public List<PojaServiceDrop> GetPojaServiceDrop()
        {
            return context.pojaServiceMasters.Where(a => a.IsDeleted == false)
                .Select(b => new PojaServiceDrop
                {
                    PrId = b.PrId,
                    ServiceName = b.ServiceName

                })
                .OrderBy(b => b.ServiceName).ToList();
        }
        public List<SpecialityDrop> GetSpecialityDrop()
        {
            return context.specialMasters.Where(a => a.IsDeleted == false)
                .Select(b => new SpecialityDrop
                {
                    PrId = b.PrId,
                    SpecialityName = b.SpecialityName

                })
                .OrderBy(b => b.SpecialityName).ToList();
        }
        public List<DiscountDrop> GetDiscountDrop()
        {
            return context.discountMasters.Where(a => a.IsDeleted == false)
                .Select(b => new DiscountDrop
                {
                    DiscountId = b.DiscountId,
                    DiscountName = b.DiscountName,
                    DiscountPercentage=b.DiscountPercentage

                })
                .OrderBy(b => b.DiscountName).ToList();
        }
        public List<DeliveryDrop> GetDeliveryDrop()
        {
            return context.deliveryMasters.Where(a => a.IsDeleted == false)
                .Select(b => new DeliveryDrop
                {
                    DeliveryId = b.DeliveryId,
                    DeliveryType = b.DeliveryType
                    

                })
                .OrderBy(b => b.DeliveryType).ToList();
        }
        public List<CountryDrop> GetAllCountries()
        {
            return context.countryMasters.Where(a => a.IsDeleted == false)
                .Select(b => new CountryDrop
                {
                    CountryName=b.CountryName,
                    CountryId=b.Id


                }).OrderBy(b=>b.CountryName).ToList();
        }
        public List<StateDrop> GetAllStates(int countryId)
        {
            return context.stateMasters.Where(a => a.IsDeleted == false&& a.CountryId==countryId)
                .Select(b => new StateDrop
                {
                    StateId=b.Id,
                    StateName=b.StateName


                }).OrderBy(b => b.StateName).ToList();
        }       
        public List<CityDrop> GetAllCities(int stateId)
        {
            return context.cityMasters.Where(a => a.IsDeleted == false && a.StateId == stateId)
               .Select(b => new CityDrop
               {
                   CityId = b.Id,
                   CityName = b.CityName


               }).OrderBy(b => b.CityName).ToList();
        }
        public List<AddressTypesDrop> GetAddressTypes()
        {
            return context.addressTypes.Where(a => a.IsDeleted == false)
               .Select(b => new AddressTypesDrop
               {
                   Id = b.Id,
                   AddressTypeName = b.AddressTypeName


               }).OrderBy(b => b.AddressTypeName).ToList();
        }
        public List<AddressTypeDrops> GetAddressesType()
        {
            return context.addressTypes.Where(a => a.IsDeleted == false)
               .Select(b => new AddressTypeDrops
               {
                   addressTypeId = b.Id,
                   AddressTypeName = b.AddressTypeName


               }).OrderBy(b => b.AddressTypeName).ToList();
        }
    }
}
