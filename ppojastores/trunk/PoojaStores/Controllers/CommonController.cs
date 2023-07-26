using Microsoft.AspNetCore.Mvc;
using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Controllers
{
    public class CommonController : Controller
    {
        private readonly ICommonDropsMgmtService cService;
        public CommonController(ICommonDropsMgmtService _cService)
        {
            cService = _cService;
        }

        [HttpPost]
        public IActionResult GetAllCats()
        {
            List<CategoryDrop> myList = new List<CategoryDrop>();
            myList = cService.GetCatsDrop();
            return Json(new { result = myList });

        }

        [HttpPost]
        public IActionResult GetAllSubCats(int catId)
        {
            List<SubCategoryDrop> myList = new List<SubCategoryDrop>();
            myList = cService.GetSubCatsDrop(catId);
            return Json(new { result = myList });

        }

        public IActionResult GetAllCountries()
        {
            List<CountryDrop> mylist = new List<CountryDrop>();
            mylist = cService.GetAllCountries();
            return Json(new { result = mylist });
        }
        public IActionResult GetAllStates(int countryId)
        {
            List<StateDrop> mylist = cService.GetAllStates(countryId);
            return Json(new { result = mylist });
        }
        public IActionResult GetAllCities(int stateId)
        {
            List<CityDrop> mylist = cService.GetAllCities(stateId);
            return Json(new { result = mylist });
        }
        public IActionResult GetAllAddressTypes()
        {
            List<AddressTypeDrops> myList = cService.GetAddressesType();
            return Json(new { result = myList });
        }

    }
}
