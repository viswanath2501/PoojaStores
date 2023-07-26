using PoojaStores.Models.ModelClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Interfaces
{
    public interface ICommonDropsMgmtRepo
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
        List<CountryDrop> GetAllCountries();
        List<StateDrop> GetAllStates(int countryId);
        List<CityDrop> GetAllCities(int stateId);
        List<AddressTypesDrop> GetAddressTypes();
        List<AddressTypeDrops> GetAddressesType();
    }
}
