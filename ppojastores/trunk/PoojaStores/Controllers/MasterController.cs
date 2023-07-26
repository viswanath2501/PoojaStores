using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Services.Interfaces;
using PoojaStores.Models.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PoojaStores.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterMgmtService mService;
        private readonly ICommonDropsMgmtService comonService;
        private readonly IHostingEnvironment hostingEnvironment;

        public MasterController(IMasterMgmtService _mService, ICommonDropsMgmtService _comonService, IHostingEnvironment _hosting)
        {
            mService = _mService;
            comonService = _comonService;
            hostingEnvironment = _hosting;
        }

        #region User Types
        public IActionResult UserTypes()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<UserTypeMaster> mylist = new List<UserTypeMaster>();
            mylist = mService.GetAllUserTypes();

            return View(mylist);
        }
        public IActionResult UserTypeData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            UserTypeMaster cm = new UserTypeMaster();
            if (id > 0)
            {
                cm = mService.GetUserTypeById(id);
            }
            return View(cm);
        }
        [HttpPost]
        public IActionResult UserTypeData(UserTypeMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                request.IsDeleted = false;
                ProcessResponse pr = mService.SaveUserTypes(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("UserTypes");
                }
            }
            return View(request);

        }
        [HttpPost]
        public IActionResult DeleteUserType(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            UserTypeMaster cm = mService.GetUserTypeById(id);
            cm.IsDeleted = true;
            pr = mService.SaveUserTypes(cm);
            return Json(new { result = pr });
        }
        #endregion

        #region Categories
        public IActionResult Categories()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<CategoryMaster> mylist = new List<CategoryMaster>();
            mylist = mService.GetAllCategories();

            return View(mylist);
        }
        public IActionResult CategoryData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            CategoryMasterModel cmm = new CategoryMasterModel();

            CategoryMaster cm = new CategoryMaster();
            
            List<CategoryMaster> cml = mService.GetAllCategories();
            cmm.CatCount = cml.Count;
            cmm.sequenceDrop = new List<SeqNums>();
            int i = 1;
            foreach(CategoryMaster c in cml)
            {
                SeqNums s = new SeqNums();
                s.Seq = i;
                cmm.sequenceDrop.Add(s);
                i++;
            }
            if (id > 0)
            {
                cm = mService.GetCategoryById(id);
                CloneObjects.CopyPropertiesTo(cm, cmm);
            }
            else
            {
                SeqNums s = new SeqNums();
                s.Seq = i;
                cmm.sequenceDrop.Add(s);
                cmm.SequenceNumber = i;
            }
            return View(cmm);
        }
        [HttpPost]
        public IActionResult CategoryData(CategoryMasterModel request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                CategoryMaster cm = new CategoryMaster();
                bool isProductImageUploaded = false;
                string productImageName = "";
                if (request.ImageUpload != null)
                {
                    var fileNameUploaded = Path.GetFileName(request.ImageUpload.FileName);
                    if (fileNameUploaded != null)
                    {
                        var contentType = request.ImageUpload.ContentType;
                        string filename = DateTime.UtcNow.ToString();
                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                        filename = filename + RandomGenerator.RandomString(4, false);
                        string extension = Path.GetExtension(fileNameUploaded);
                        filename += extension;
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                        var filePath = Path.Combine(uploads, filename);
                        request.ImageUpload.CopyTo(new FileStream(filePath, FileMode.Create));
                        productImageName = filename;
                        isProductImageUploaded = true;
                    }
                }
                
                if (request.CategoryId > 0)
                {
                    cm = mService.GetCategoryById(request.CategoryId);
                    cm.CategoryName = request.CategoryName;
                    cm.SequenceNumber = request.SequenceNumber;
                    cm.CategoryImage = isProductImageUploaded ? productImageName : cm.CategoryImage;
                }
                else
                {
                    CloneObjects.CopyPropertiesTo(request, cm);
                    cm.CategoryImage = isProductImageUploaded ? productImageName : null;
                }
                ProcessResponse pr = mService.SaveCategory(cm);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Categories");
                }
            }
            return View(request);

        }
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            CategoryMaster cm = mService.GetCategoryById(id);
            cm.IsDeleted = true;
            pr = mService.SaveCategory(cm);
            return Json(new { result = pr });
        }
        #endregion

        #region SubCategories
        public IActionResult SubCategories(int catId = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<SubCategoryMaster> mylist = new List<SubCategoryMaster>();
            mylist = mService.GetAllSubCategories(catId);

            List<CategoryMaster> cm = new List<CategoryMaster>();
            cm = mService.GetAllCategories();
            ViewBag.cats = cm;
            if (catId != 0)
            {
                ViewBag.catId = catId;
            }


            return View(mylist);
        }
        public IActionResult SubCategoryData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            SubCategoryMaster sm = new SubCategoryMaster();
            if (id > 0)
            {
                sm = mService.GetSubCategoryById(id);
            }

            List<CategoryMaster> cm = new List<CategoryMaster>();
            cm = mService.GetAllCategories();
            ViewBag.cats = cm;
            return View(sm);
        }
        [HttpPost]
        public IActionResult SubCategoryData(SubCategoryMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SaveSubCategory(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("SubCategories", new { catId = request.CategoryId });
                }
            }
            List<CategoryMaster> cm = new List<CategoryMaster>();
            cm = mService.GetAllCategories();
            ViewBag.cats = cm;
            return View(request);

        }
        public IActionResult DeleteSubCategory(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            SubCategoryMaster cm = mService.GetSubCategoryById(id);
            cm.IsDeleted = true;
            pr = mService.SaveSubCategory(cm);
            return Json(new { result = pr });
        }
        #endregion

        #region Measurements
        public IActionResult Measurements()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<MeasurementMaster> mylist = new List<MeasurementMaster>();
            mylist = mService.GetAllMeasurements();

            return View(mylist);
        }
        public IActionResult MeasurementData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            MeasurementMaster cm = new MeasurementMaster();
            if (id > 0)
            {
                cm = mService.GetMeasurementById(id);
            }

            return View(cm);
        }
        [HttpPost]
        public IActionResult MeasurementData(MeasurementMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SaveMeasurements(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Measurements");
                }
            }
            return View(request);

        }
        public IActionResult DeleteMeasurements(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            MeasurementMaster cm = mService.GetMeasurementById(id);
            cm.IsDeleted = true;
            pr = mService.SaveMeasurements(cm);
            return Json(new { result = pr });
        }

        #endregion

        #region GST
        public IActionResult GST()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<GSTMaster> mylist = new List<GSTMaster>();
            mylist = mService.GetAllGST();

            return View(mylist);
        }
        public IActionResult GSTData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            GSTMaster cm = new GSTMaster();
            if (id > 0)
            {
                cm = mService.GetGSTById(id);
            }

            return View(cm);
        }
        [HttpPost]
        public IActionResult GSTData(GSTMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SaveGST(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("GST");
                }
            }
            return View(request);

        }
        public IActionResult DeleteGST(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            GSTMaster cm = mService.GetGSTById(id);
            cm.IsDeleted = true;
            pr = mService.SaveGST(cm);
            return Json(new { result = pr });
        }

        #endregion

        #region Detail Category
        public IActionResult DetailCategory(int catId = 0, int subcatId = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<DetailCategoryMaster> mylist = mService.GetDetailCatsOfSubCat(subcatId);

            List<CategoryDrop> cm = comonService.GetCatsDrop();

            if (catId == 0)
            {
                catId = cm[0].CategoryId;
            }
            List<SubCategoryDrop> sm = comonService.GetSubCatsDrop(catId);
            DetailCategoryModel dcm = new DetailCategoryModel();
            dcm.catDrops = cm;
            dcm.subCatDrops = sm;
            dcm.DetailedDetails = mylist;

            ViewBag.catId = catId;
            ViewBag.subCatId = sm[0].SubCategoryId;
            return View(dcm);
        }
        public IActionResult DetailCategoryData(int catId = 0, int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            DetailCategoryMaster dm = new DetailCategoryMaster();
            
            List<CategoryDrop> cm = comonService.GetCatsDrop();
            if (catId == 0)
            {
                catId = cm[0].CategoryId;
            }
            List<SubCategoryDrop> sm = comonService.GetSubCatsDrop(catId);

            DetailCategoryModel dcm = new DetailCategoryModel();
            dcm.catDrops = cm;
            dcm.subCatDrops = sm;
            if (id > 0)
            {
                dm = mService.GetDetailCategoryById(id);
                dcm.DetailCategoryName = dm.DetailCategoryName;
                dcm.DetailCategoryId = dm.DetailCategoryId;
                dcm.CategoryId = dm.CategoryId;
                dcm.SubCategoryId = dm.SubCategoryId;
                dcm.subCatDrops= comonService.GetSubCatsDrop((int)dm.CategoryId);
            }
            
            return View(dcm);
        }
        [HttpPost]
        public IActionResult DetailCategoryData(DetailCategoryMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SaveDetailCategory(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("DetailCategory", new { catId = request.CategoryId, subcatId = request.SubCategoryId });
                }
            }
            List<CategoryMaster> cm = new List<CategoryMaster>();
            cm = mService.GetAllCategories();
            ViewBag.cats = cm;


            return View(request);

        }
        public IActionResult DeleteDetailCategory(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            DetailCategoryMaster cm = mService.GetDetailCategoryById(id);
            cm.IsDeleted = true;
            pr = mService.SaveDetailCategory(cm);
            return Json(new { result = pr });
        }

        #endregion

        #region Pooja Item
        public IActionResult PojaItems()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<PojaItemMaster> mylist = new List<PojaItemMaster>();
            mylist = mService.GetAllPojaItem();

            return View(mylist);
        }
        public IActionResult PojaItemData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            PojaItemMaster cm = new PojaItemMaster();
            if (id > 0)
            {
                cm = mService.GetPojaItemById(id);
            }

            return View(cm);
        }
        [HttpPost]
        public IActionResult PojaItemData(PojaItemMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SavePojaItem(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("PojaItems");
                }
            }
            return View(request);

        }
        public IActionResult DeletePojaItems(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            PojaItemMaster cm = mService.GetPojaItemById(id);
            cm.IsDeleted = true;
            pr = mService.SavePojaItem(cm);
            return Json(new { result = pr });
        }

        #endregion

        #region Pooja Service
        public IActionResult PojaServices()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<PojaServiceMaster> mylist = new List<PojaServiceMaster>();
            mylist = mService.GetAllPojaServices();

            return View(mylist);
        }
        public IActionResult PojaServiceData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            PojaServiceMaster cm = new PojaServiceMaster();
            if (id > 0)
            {
                cm = mService.GetPojaServiceById(id);
            }

            return View(cm);
        }
        [HttpPost]
        public IActionResult PojaServiceData(PojaServiceMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SavePojaService(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("PojaServices");
                }
            }
            return View(request);

        }
        public IActionResult DeletePojaServices(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            PojaServiceMaster cm = mService.GetPojaServiceById(id);
            cm.IsDeleted = true;
            pr = mService.SavePojaService(cm);
            return Json(new { result = pr });
        }

        #endregion

        #region Specialities
        public IActionResult Specialities()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<SpecialMaster> mylist = new List<SpecialMaster>();
            mylist = mService.GetAllSpecialities();

            return View(mylist);
        }
        public IActionResult SpecialityData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            SpecialMaster cm = new SpecialMaster();
            if (id > 0)
            {
                cm = mService.GetSpecialityById(id);
            }

            return View(cm);
        }
        [HttpPost]
        public IActionResult SpecialityData(SpecialMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SaveSpeciality(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Specialities");
                }
            }
            return View(request);

        }
        public IActionResult DeleteSpecialities(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            SpecialMaster cm = mService.GetSpecialityById(id);
            cm.IsDeleted = true;
            pr = mService.SaveSpeciality(cm);
            return Json(new { result = pr });
        }

        #endregion

        #region Discounts
        public IActionResult Discounts()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<DiscountMaster> mylist = new List<DiscountMaster>();
            mylist = mService.GetAllDiscounts();

            return View(mylist);
        }
        public IActionResult DiscountData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            DiscountMaster cm = new DiscountMaster();
            if (id > 0)
            {
                cm = mService.GetDiscountById(id);
            }

            return View(cm);
        }
        [HttpPost]
        public IActionResult DiscountData(DiscountMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SaveDiscountPercent(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Discounts");
                }
            }
            return View(request);

        }
        public IActionResult DeleteDiscounts(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            DiscountMaster cm = mService.GetDiscountById(id);
            cm.IsDeleted = true;
            pr = mService.SaveDiscountPercent(cm);
            return Json(new { result = pr });
        }

        #endregion

        #region Deliveries
        public IActionResult Deliveries()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<DeliveryMaster> mylist = new List<DeliveryMaster>();
            mylist = mService.GetAllDeliveryTypes();

            return View(mylist);
        }
        public IActionResult DeliveryData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            DeliveryMaster cm = new DeliveryMaster();
            if (id > 0)
            {
                cm = mService.GetDeliveryTypeById(id);
            }

            return View(cm);
        }
        [HttpPost]
        public IActionResult DeliveryData(DeliveryMaster request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SaveDeliveryType(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("Deliveries");
                }
            }
            return View(request);

        }
        public IActionResult DeleteDeliveries(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            DeliveryMaster cm = mService.GetDeliveryTypeById(id);
            cm.IsDeleted = true;
            pr = mService.SaveDeliveryType(cm);
            return Json(new { result = pr });
        }

        #endregion

        #region AddressTypes
        public IActionResult AddressTypes()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<AddressTypes> mylist = new List<AddressTypes>();
            mylist = mService.GetAddressTypes();

            return View(mylist);
        }
        public IActionResult AddressTypesData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            AddressTypes cm = new AddressTypes();
            if (id > 0)
            {
                cm = mService.GetAddressTypeById(id);
            }

            return View(cm);
        }
        [HttpPost]
        public IActionResult AddressTypesData(AddressTypes request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            if (ModelState.IsValid)
            {
                ProcessResponse pr = mService.SaveAddressType(request);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("AddressTypes");
                }
            }
            return View(request);

        }
        public IActionResult DeleteAddressTypes(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            AddressTypes cm = mService.GetAddressTypeById(id);
            cm.IsDeleted = true;
            pr = mService.SaveAddressType(cm);
            return Json(new { result = pr });
        }

        #endregion
    }
}
