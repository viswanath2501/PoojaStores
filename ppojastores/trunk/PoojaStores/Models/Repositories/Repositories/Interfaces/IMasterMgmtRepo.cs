using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Interfaces
{
    public interface IMasterMgmtRepo
    {
        ProcessResponse SaveUserType(UserTypeMaster request);
        List<UserTypeMaster> GetAllUserTypes();
        UserTypeMaster GetUserTypeById(int id);
        UserTypeMaster GetUserTypeByName(string type = "");
        ProcessResponse UpdateUserTypes(UserTypeMaster request);

        ProcessResponse SaveCategory(CategoryMaster request);
        List<CategoryMaster> GetAllCategories();
        CategoryMaster GetCategoryById(int id);
        ProcessResponse UpdateCategories(CategoryMaster request);

        ProcessResponse SaveSubCategory(SubCategoryMaster request);
        List<SubCategoryMaster> GetAllSubCategories(int catId);
        SubCategoryMaster GetSubCategoryById(int id);
        ProcessResponse UpdateSubCategories(SubCategoryMaster request);

        ProcessResponse SaveDeatailCategory(DetailCategoryMaster request);
        List<DetailCategoryMaster> GetAllDetailCategories();
        DetailCategoryMaster GetDetailCategoryById(int id);
        List<DetailCategoryMaster> GetDetailCatBySubCatId(int id);
        ProcessResponse UpdateDeatailCategories(DetailCategoryMaster request);

        ProcessResponse SaveMeasurement(MeasurementMaster request);
        List<MeasurementMaster> GetAllMeasurements();
        MeasurementMaster GetMeasurementById(int id);
        ProcessResponse UpdateMeasurements(MeasurementMaster request);

        
        ProcessResponse SaveGST(GSTMaster request);
        List<GSTMaster> GetAllGST();
        GSTMaster GetGSTById(int id);
        ProcessResponse UpdateGST(GSTMaster request);

        ProcessResponse SavePojaItem(PojaItemMaster request);
        List<PojaItemMaster> GetAllPojaItems();
        PojaItemMaster GetPojaItemById(int id);
        ProcessResponse UpdatePojaItem(PojaItemMaster request);

        ProcessResponse SavePojaService(PojaServiceMaster request);
        List<PojaServiceMaster> GetAllPojaServices();
        PojaServiceMaster GetPojaServiceById(int id);
        ProcessResponse UpdatePoojaService(PojaServiceMaster request);

        ProcessResponse SaveSpecial(SpecialMaster request);
        List<SpecialMaster> GetAllSpecials();
        SpecialMaster GetSpecialById(int id);
        ProcessResponse UpdateSpecial(SpecialMaster request);

        ProcessResponse SaveDelivery(DeliveryMaster request);
        List<DeliveryMaster> GetAllDeliverys();
        DeliveryMaster GetDeliveryById(int id);
        ProcessResponse UpdateDelivery(DeliveryMaster request);

        ProcessResponse SaveDiscount(DiscountMaster request);
        List<DiscountMaster> GetAllDiscountss();
        DiscountMaster GetDiscountsById(int id);
        ProcessResponse UpdateDiscount(DiscountMaster request);

        ProcessResponse SaveAddressType(AddressTypes request);
        List<AddressTypes> GetAddressTypes();
        AddressTypes GetAddressTypesById(int id);
        ProcessResponse UpdateAddressTypes(AddressTypes request);

        void LogError(Exception ex);

    }
}
