using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
    public interface IMasterMgmtService
    {
        ProcessResponse SaveUserTypes(UserTypeMaster request);
        List<UserTypeMaster> GetAllUserTypes();
        UserTypeMaster GetUserTypeById(int id);
        UserTypeMaster GetUserTypeByName(string type = "");

        ProcessResponse SaveCategory(CategoryMaster request);
        List<CategoryMaster> GetAllCategories();
        CategoryMaster GetCategoryById(int id);

        ProcessResponse SaveSubCategory(SubCategoryMaster request);
        List<SubCategoryMaster> GetAllSubCategories(int catId);
        SubCategoryMaster GetSubCategoryById(int id);

        ProcessResponse SaveMeasurements(MeasurementMaster request);
        List<MeasurementMaster> GetAllMeasurements();
        MeasurementMaster GetMeasurementById(int id);

        ProcessResponse SaveDetailCategory(DetailCategoryMaster request);
        List<DetailCategoryMaster> GetAllDetailCategories();
        DetailCategoryMaster GetDetailCategoryById(int id);
        List<DetailCategoryMaster> GetDetailCatsOfSubCat(int id);

        ProcessResponse SaveGST(GSTMaster request);
        List<GSTMaster> GetAllGST();
        GSTMaster GetGSTById(int id);

        ProcessResponse SavePojaItem(PojaItemMaster request);
        List<PojaItemMaster> GetAllPojaItem();
        PojaItemMaster GetPojaItemById(int id);

        ProcessResponse SavePojaService(PojaServiceMaster request);
        List<PojaServiceMaster> GetAllPojaServices();
        PojaServiceMaster GetPojaServiceById(int id);

        ProcessResponse SaveSpeciality(SpecialMaster request);
        List<SpecialMaster> GetAllSpecialities();
        SpecialMaster GetSpecialityById(int id);

        ProcessResponse SaveDeliveryType(DeliveryMaster request);
        List<DeliveryMaster> GetAllDeliveryTypes();
        DeliveryMaster GetDeliveryTypeById(int id);


        ProcessResponse SaveDiscountPercent(DiscountMaster request);
        List<DiscountMaster> GetAllDiscounts();
        DiscountMaster GetDiscountById(int id);

        ProcessResponse SaveAddressType(AddressTypes request);
        List<AddressTypes> GetAddressTypes();
        AddressTypes GetAddressTypeById(int id);

    }
}
