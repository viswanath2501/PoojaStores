using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using PoojaStores.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Services
{
    public class MasterMgmtService : IMasterMgmtService
    {
        private readonly IMasterMgmtRepo mRepo;
        public MasterMgmtService(IMasterMgmtRepo _mRepo)
        {
            mRepo = _mRepo;
        }
        #region UserTypes
        public ProcessResponse SaveUserTypes(UserTypeMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.TypeId > 0)
                {
                    UserTypeMaster utm = mRepo.GetUserTypeById(request.TypeId);
                    utm.TypeName = request.TypeName;
                    utm.IsDeleted = request.IsDeleted;

                    response = mRepo.UpdateUserTypes(utm);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveUserType(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<UserTypeMaster> GetAllUserTypes()
        {
            return mRepo.GetAllUserTypes();
        }
        public UserTypeMaster GetUserTypeById(int id)
        {
            return mRepo.GetUserTypeById(id);
        }
        public UserTypeMaster GetUserTypeByName(string type = "")
        {
            return mRepo.GetUserTypeByName(type);
        }
        #endregion

        #region Category
        public ProcessResponse SaveCategory(CategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.CategoryId > 0)
                {
                    CategoryMaster cm = new CategoryMaster();
                    cm = mRepo.GetCategoryById(request.CategoryId);
                    cm.CategoryName = request.CategoryName;

                    response = mRepo.UpdateCategories(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveCategory(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<CategoryMaster> GetAllCategories()
        {
            return mRepo.GetAllCategories();
        }
        public CategoryMaster GetCategoryById(int id)
        {
            return mRepo.GetCategoryById(id);
        }
        #endregion

        #region Sub Category
        public ProcessResponse SaveSubCategory(SubCategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.SubCategoryId > 0)
                {
                    SubCategoryMaster sm = new SubCategoryMaster();
                    sm = mRepo.GetSubCategoryById(request.SubCategoryId);
                    sm.SubCategoryName = request.SubCategoryName;

                    response = mRepo.UpdateSubCategories(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveSubCategory(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<SubCategoryMaster> GetAllSubCategories(int catId)
        {
            return mRepo.GetAllSubCategories(catId);
        }
        public SubCategoryMaster GetSubCategoryById(int id)
        {
            return mRepo.GetSubCategoryById(id);
        }
        #endregion

        #region Deatail Category
        public ProcessResponse SaveDetailCategory(DetailCategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.DetailCategoryId > 0)
                {
                    DetailCategoryMaster dm = mRepo.GetDetailCategoryById(request.DetailCategoryId);
                    dm.DetailCategoryName = request.DetailCategoryName;

                    response = mRepo.UpdateDeatailCategories(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveDeatailCategory(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<DetailCategoryMaster> GetAllDetailCategories()
        {
            return mRepo.GetAllDetailCategories();
        }
        public DetailCategoryMaster GetDetailCategoryById(int id)
        {
            return mRepo.GetDetailCategoryById(id);
        }
        public List<DetailCategoryMaster> GetDetailCatsOfSubCat(int id)
        {
            return mRepo.GetDetailCatBySubCatId(id);
        }
        #endregion

        #region Measurements
        public ProcessResponse SaveMeasurements(MeasurementMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.MeasurementId > 0)
                {
                    MeasurementMaster mm = mRepo.GetMeasurementById(request.MeasurementId);
                    mm.MeasurementName = request.MeasurementName;

                    response = mRepo.UpdateMeasurements(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveMeasurement(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<MeasurementMaster> GetAllMeasurements()
        {
            return mRepo.GetAllMeasurements();
        }
        public MeasurementMaster GetMeasurementById(int id)
        {
            return mRepo.GetMeasurementById(id);
        }
        #endregion

        #region GST
        public ProcessResponse SaveGST(GSTMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.MasterId > 0)
                {
                    GSTMaster gm = mRepo.GetGSTById(request.MasterId);
                    gm.GSTName = request.GSTName;
                    gm.GSTTaxValue = request.GSTTaxValue;

                    response = mRepo.UpdateGST(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveGST(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<GSTMaster> GetAllGST()
        {
            return mRepo.GetAllGST();
        }
        public GSTMaster GetGSTById(int id)
        {
            return mRepo.GetGSTById(id);
        }
        #endregion

        #region PojaItem
        public ProcessResponse SavePojaItem(PojaItemMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.PrId > 0)
                {
                    PojaItemMaster pm = mRepo.GetPojaItemById(request.PrId);
                    pm.ItemName = request.ItemName;

                    response = mRepo.UpdatePojaItem(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SavePojaItem(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<PojaItemMaster> GetAllPojaItem()
        {
            return mRepo.GetAllPojaItems();
        }
        public PojaItemMaster GetPojaItemById(int id)
        {
            return mRepo.GetPojaItemById(id);
        }
        #endregion

        #region Poja Service
        public ProcessResponse SavePojaService(PojaServiceMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.PrId > 0)
                {
                    PojaServiceMaster pm = mRepo.GetPojaServiceById(request.PrId);
                    pm.ServiceName = request.ServiceName;

                    response = mRepo.UpdatePoojaService(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SavePojaService(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<PojaServiceMaster> GetAllPojaServices()
        {
            return mRepo.GetAllPojaServices();
        }
        public PojaServiceMaster GetPojaServiceById(int id)
        {
            return mRepo.GetPojaServiceById(id);
        }
        #endregion

        #region Special
        public ProcessResponse SaveSpeciality(SpecialMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.PrId > 0)
                {
                    SpecialMaster sm = mRepo.GetSpecialById(request.PrId);
                    sm.SpecialityName = request.SpecialityName;

                    response = mRepo.UpdateSpecial(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveSpecial(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<SpecialMaster> GetAllSpecialities()
        {
            return mRepo.GetAllSpecials();
        }
        public SpecialMaster GetSpecialityById(int id)
        {
            return mRepo.GetSpecialById(id);
        }
        #endregion
        #region Delivery
        public ProcessResponse SaveDeliveryType(DeliveryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.DeliveryId > 0)
                {
                    DeliveryMaster sm = mRepo.GetDeliveryById(request.DeliveryId);
                    sm.DeliveryType = request.DeliveryType;

                    response = mRepo.UpdateDelivery(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveDelivery(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<DeliveryMaster> GetAllDeliveryTypes()
        {
            return mRepo.GetAllDeliverys();
        }
        public DeliveryMaster GetDeliveryTypeById(int id)
        {
            return mRepo.GetDeliveryById(id);
        }
        #endregion
        #region Discount
        public ProcessResponse SaveDiscountPercent(DiscountMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.DiscountId > 0)
                {
                    DiscountMaster dm = mRepo.GetDiscountsById(request.DiscountId);
                    dm.DiscountName = request.DiscountName;
                    dm.DiscountPercentage = request.DiscountPercentage;
                    response = mRepo.UpdateDiscount(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveDiscount(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<DiscountMaster> GetAllDiscounts()
        {
            return mRepo.GetAllDiscountss();
        }
        public DiscountMaster GetDiscountById(int id)
        {
            return mRepo.GetDiscountsById(id);
        }
        #endregion

        #region AddressType
        public ProcessResponse SaveAddressType(AddressTypes request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.Id > 0)
                {
                    AddressTypes at = mRepo.GetAddressTypesById(request.Id);
                    at.AddressTypeName = request.AddressTypeName;
                 
                    response = mRepo.UpdateAddressTypes(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = mRepo.SaveAddressType(request);
                }


            }
            catch (Exception ex)
            {
                mRepo.LogError(ex);
            }

            return response;
        }
        public List<AddressTypes> GetAddressTypes()
        {
            return mRepo.GetAddressTypes();
        }
        public AddressTypes GetAddressTypeById(int id)
        {
            return mRepo.GetAddressTypesById(id);
        }
        #endregion
    }
}
