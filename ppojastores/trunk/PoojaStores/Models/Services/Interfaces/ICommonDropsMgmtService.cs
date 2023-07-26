using PoojaStores.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
    public interface ICommonDropsMgmtService
    {
        List<CategoryDrop> GetCatsDrop();
        List<SubCategoryDrop> GetSubCatsDrop(int id);
        List<DetailCategoryDrop> GetDetailCatsDrop(int id);
        List<GSTDrop> GetGSTDrop();
        List<MeasurementDrop> GetMeasurementDrop();
        List<PojaItemDrop> GetPojaItemDrop();
        List<PojaServiceDrop> GetPojaServiceDrop();
        List<SpecialityDrop> GetSpecialityDrop();
        List<DiscountDrop> GetDiscountDrop();
        List<DeliveryDrop> GetDeliveryDrop();
        List<CategoriesWithSub> GetCatsWithSubCats();
        List<CountryDrop> GetAllCountries();
        List<StateDrop> GetAllStates(int countryId);
        List<CityDrop> GetAllCities(int stateId);
        public List<AddressTypesDrop> GetAddressTypes();
        List<AddressTypeDrops> GetAddressesType();
        List<SubCategoryDrop> GetSubs(int id);

    }
}
