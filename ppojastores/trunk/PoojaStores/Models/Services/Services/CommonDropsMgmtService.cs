using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using PoojaStores.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Services
{
    public class CommonDropsMgmtService : ICommonDropsMgmtService
    {
        private readonly ICommonDropsMgmtRepo cRepo;
        private readonly IProductMainMgmtRepo pRepo;
        public CommonDropsMgmtService(ICommonDropsMgmtRepo _cRepo, IProductMainMgmtRepo _pRepo)
        {
            cRepo = _cRepo;
            pRepo = _pRepo;
        }
        public List<CategoryDrop> GetCatsDrop()
        {
            return cRepo.GetCatsDrop();
        }
        public List<SubCategoryDrop> GetSubCatsDrop(int id)
        {
            List<SubCategoryDrop> scd = cRepo.GetSubCatsDrop(id)
;
            SubCategoryDrop sc = new SubCategoryDrop();
            sc.SubCategoryId = 0;
            sc.SubCategoryName = "-- Select Sub Category--";
            scd.Insert(0, sc);

            return scd;
           
        }
        public List<DetailCategoryDrop> GetDetailCatsDrop(int id)
        {
            return cRepo.GetDetailCatsDrop(id);
        }
        public List<GSTDrop> GetGSTDrop()
        {
            return cRepo.GetGSTDrop();
        }
        public List<MeasurementDrop> GetMeasurementDrop()
        {
            return cRepo.GetMeasurementDrop();
        }
        public List<PojaItemDrop> GetPojaItemDrop()
        {
            return cRepo.GetPojaItemDrop();
        }
        public List<PojaServiceDrop> GetPojaServiceDrop()
        {
            return cRepo.GetPojaServiceDrop();
        }
        public List<SpecialityDrop> GetSpecialityDrop()
        {
            return cRepo.GetSpecialityDrop();
        }
        public List<DiscountDrop> GetDiscountDrop()
        {
            return cRepo.GetDiscountDrop();
        }
        public List<DeliveryDrop> GetDeliveryDrop()
        {
            return cRepo.GetDeliveryDrop();
        }
        public List<SubCategoryDrop> GetSubs(int id)
        {
            return cRepo.GetSubCatsDrop(id);
        }
        public List<CategoriesWithSub> GetCatsWithSubCats()
        {
            List<CategoriesWithSub> cwsl = new List<CategoriesWithSub>();
            List<CategoryDrop> cdl = GetCatsDrop();
                        
            foreach (CategoryDrop c in cdl)
            {
                CategoriesWithSub cs = new CategoriesWithSub();
                cs.CategoryId = c.CategoryId;
                cs.CategoryName = c.CategoryName;
                cs.CategoryImage = c.CategoryImage;
                cs.ProductsCount = pRepo.GetCategoryProductCount(c.CategoryId);
                cs.SubCats = GetSubs(c.CategoryId);
                foreach(SubCategoryDrop s in cs.SubCats)
                {
                    s.ProductCount = pRepo.GetSubCategoryProductsCount(s.SubCategoryId);
                }

                cs.subCount = cs.SubCats.Count;
                cwsl.Add(cs);
            }
            return cwsl;
        }
        public List<CountryDrop> GetAllCountries()
        {
            return cRepo.GetAllCountries();
        }
        public List<StateDrop> GetAllStates(int countryId)
        {
            return cRepo.GetAllStates(countryId);
        }
        public List<CityDrop> GetAllCities(int stateId)
        {
            return cRepo.GetAllCities(stateId);
        }
        public List<AddressTypesDrop> GetAddressTypes()
        {
            return cRepo.GetAddressTypes();
        }
        public List<AddressTypeDrops> GetAddressesType()
        {
            return cRepo.GetAddressesType();
        }
    }
}
