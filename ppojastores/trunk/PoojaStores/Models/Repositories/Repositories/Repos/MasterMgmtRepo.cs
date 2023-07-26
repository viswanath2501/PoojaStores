using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class MasterMgmtRepo : IMasterMgmtRepo
    {
        private readonly MyDbContext context;

        public MasterMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        #region UserType
        public ProcessResponse SaveUserType(UserTypeMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.userTypeMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.TypeId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<UserTypeMaster> GetAllUserTypes()
        {
            List<UserTypeMaster> response = new List<UserTypeMaster>();
            try
            {
                response = context.userTypeMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public UserTypeMaster GetUserTypeById(int id)
        {
            UserTypeMaster response = new UserTypeMaster();
            try
            {
                response = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public UserTypeMaster GetUserTypeByName(string type="")
        {
            UserTypeMaster response = new UserTypeMaster();
            return response = context.userTypeMasters.Where(m => m.TypeName == type && m.IsDeleted==false).FirstOrDefault();
        }
        public ProcessResponse UpdateUserTypes(UserTypeMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                UserTypeMaster um = GetUserTypeById(request.TypeId);
                context.Entry(um).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.TypeId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Category
        public ProcessResponse SaveCategory(CategoryMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.categoryMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.CategoryId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<CategoryMaster> GetAllCategories()
        {
            List<CategoryMaster> response = new List<CategoryMaster>();
            try
            {
                response = context.categoryMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public CategoryMaster GetCategoryById(int id)
        {
            CategoryMaster response = new CategoryMaster();
            try
            {
                response = context.categoryMasters.Where(a => a.IsDeleted == false && a.CategoryId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateCategories(CategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                request.IsDeleted = false;
                response.currentId = request.CategoryId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Sub Category
        public ProcessResponse SaveSubCategory(SubCategoryMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.subCategoryMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.SubCategoryId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<SubCategoryMaster> GetAllSubCategories(int catId)
        {
            List<SubCategoryMaster> response = new List<SubCategoryMaster>();
            try
            {
                response = context.subCategoryMasters.Where(a => a.IsDeleted == false && a.CategoryId==catId).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public SubCategoryMaster GetSubCategoryById(int id)
        {
            SubCategoryMaster response = new SubCategoryMaster();
            try
            {
                response = context.subCategoryMasters.Where(a => a.IsDeleted == false && a.SubCategoryId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateSubCategories(SubCategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.SubCategoryId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Detail Category
        public ProcessResponse SaveDeatailCategory(DetailCategoryMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.detailCategoryMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.DetailCategoryId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<DetailCategoryMaster> GetAllDetailCategories()
        {
            List<DetailCategoryMaster> response = new List<DetailCategoryMaster>();
            try
            {
                response = context.detailCategoryMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public DetailCategoryMaster GetDetailCategoryById(int id)
        {
            DetailCategoryMaster response = new DetailCategoryMaster();
            try
            {
                response = context.detailCategoryMasters.Where(a => a.IsDeleted == false && a.DetailCategoryId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public List< DetailCategoryMaster> GetDetailCatBySubCatId(int id)
        {
            List<DetailCategoryMaster> response = new List<DetailCategoryMaster>();
            try
            {
                response = context.detailCategoryMasters.Where(a => a.IsDeleted == false && a.SubCategoryId == id).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateDeatailCategories(DetailCategoryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.DetailCategoryId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Measurement
        public ProcessResponse SaveMeasurement(MeasurementMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.measurementMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.MeasurementId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<MeasurementMaster> GetAllMeasurements()
        {
            List<MeasurementMaster> response = new List<MeasurementMaster>();
            try
            {
                response = context.measurementMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public MeasurementMaster GetMeasurementById(int id)
        {
            MeasurementMaster response = new MeasurementMaster();
            try
            {
                response = context.measurementMasters.Where(a => a.IsDeleted == false && a.MeasurementId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateMeasurements(MeasurementMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.MeasurementId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region GST
        public ProcessResponse SaveGST(GSTMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.gSTMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.MasterId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<GSTMaster> GetAllGST()
        {
            List<GSTMaster> response = new List<GSTMaster>();
            try
            {
                response = context.gSTMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public GSTMaster GetGSTById(int id)
        {
            GSTMaster response = new GSTMaster();
            try
            {
                response = context.gSTMasters.Where(a => a.IsDeleted == false && a.MasterId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateGST(GSTMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.MasterId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Poja Item
        public ProcessResponse SavePojaItem(PojaItemMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.pojaItemMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.PrId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<PojaItemMaster> GetAllPojaItems()
        {
            List<PojaItemMaster> response = new List<PojaItemMaster>();
            try
            {
                response = context.pojaItemMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public PojaItemMaster GetPojaItemById(int id)
        {
            PojaItemMaster response = new PojaItemMaster();
            try
            {
                response = context.pojaItemMasters.Where(a => a.IsDeleted == false && a.PrId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdatePojaItem(PojaItemMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.PrId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Poja Service
        public ProcessResponse SavePojaService(PojaServiceMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.pojaServiceMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.PrId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<PojaServiceMaster> GetAllPojaServices()
        {
            List<PojaServiceMaster> response = new List<PojaServiceMaster>();
            try
            {
                response = context.pojaServiceMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public PojaServiceMaster GetPojaServiceById(int id)
        {
            PojaServiceMaster response = new PojaServiceMaster();
            try
            {
                response = context.pojaServiceMasters.Where(a => a.IsDeleted == false && a.PrId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdatePoojaService(PojaServiceMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.PrId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Special
        public ProcessResponse SaveSpecial(SpecialMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.specialMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.PrId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<SpecialMaster> GetAllSpecials()
        {
            List<SpecialMaster> response = new List<SpecialMaster>();
            try
            {
                response = context.specialMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public SpecialMaster GetSpecialById(int id)
        {
            SpecialMaster response = new SpecialMaster();
            try
            {
                response = context.specialMasters.Where(a => a.IsDeleted == false && a.PrId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateSpecial(SpecialMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.PrId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Delivery
        public ProcessResponse SaveDelivery(DeliveryMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.deliveryMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.DeliveryId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<DeliveryMaster> GetAllDeliverys()
        {
            List<DeliveryMaster> response = new List<DeliveryMaster>();
            try
            {
                response = context.deliveryMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public DeliveryMaster GetDeliveryById(int id)
        {
            DeliveryMaster response = new DeliveryMaster();
            try
            {
                response = context.deliveryMasters.Where(a => a.IsDeleted == false && a.DeliveryId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateDelivery(DeliveryMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.DeliveryId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region Discount
        public ProcessResponse SaveDiscount(DiscountMaster request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.discountMasters.Add(request);
                context.SaveChanges();
                response.currentId = request.DiscountId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<DiscountMaster> GetAllDiscountss()
        {
            List<DiscountMaster> response = new List<DiscountMaster>();
            try
            {
                response = context.discountMasters.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public DiscountMaster GetDiscountsById(int id)
        {
            DiscountMaster response = new DiscountMaster();
            try
            {
                response = context.discountMasters.Where(a => a.IsDeleted == false && a.DiscountId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateDiscount(DiscountMaster request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.DiscountId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        #region AddressType
        public ProcessResponse SaveAddressType(AddressTypes request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.addressTypes.Add(request);
                context.SaveChanges();
                response.currentId = request.Id;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }
        public List<AddressTypes> GetAddressTypes()
        {
            List<AddressTypes> response = new List<AddressTypes>();
            try
            {
                response = context.addressTypes.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }

        public AddressTypes GetAddressTypesById(int id)
        {
            AddressTypes response = new AddressTypes();
            try
            {
                response = context.addressTypes.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateAddressTypes(AddressTypes request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.Id;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;

        }
        #endregion

        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }
    }
}
