using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PoojaStores.Models;
using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Services.Interfaces;
using PoojaStores.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PoojaStores.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonDropsMgmtService cService;
        private readonly IProductMgmtService pService;
        private readonly IMasterMgmtService mService;
        private readonly IImageMgmtService iService;
        private readonly ISalesMgmtService sService;
        private readonly IUsersMgmtService uService;
        private readonly ICustomerMgmtService cusService;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, ICommonDropsMgmtService _cService, IProductMgmtService _pService, IMasterMgmtService _mService,
                                IImageMgmtService _iService, ISalesMgmtService _sService, IUsersMgmtService _uService,
                                IHostingEnvironment _hosting, ICustomerMgmtService _cusService)
        {
            _logger = logger;
            cService = _cService;
            pService = _pService;
            mService = _mService;
            iService = _iService;
            sService = _sService;
            uService = _uService;
            hostingEnvironment = _hosting;
            cusService = _cusService;

        }

        public IActionResult Index()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (loginCheckResponse == null)
            {
                ViewBag.prods = pService.GetFeaturedProductList();
            }
            else
            {
                ViewBag.prods = pService.GetFeaturedProductList(loginCheckResponse.userId);
            }
            ViewBag.CatImages = iService.GetHomeCatImages();
            ViewBag.homebanners = iService.GetAllHomeBanners();
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }
            ViewBag.banners = iService.GetAllHomeBanners();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ProductDetails(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }
            ProductMasterModel pmm = pService.GetProductDetail(id);
            pmm.OtherImages = pService.ProductOtherImgsUploaded(id);
            decimal percentvalue = 1 - (pmm.DiscountPercentage / 100);
            pmm.DiscountedCost = Math.Round(((decimal)pmm.SellingPrice * percentvalue), 2);
            ViewBag.test = false;
            ViewBag.cart = "Yes";
            if (loginCheckResponse != null)
            {
                bool res = sService.IsProduct(id, loginCheckResponse.userId);
                if (res)
                {
                    ViewBag.test = true;
                }
                bool result = sService.IsCart(loginCheckResponse.userId, id);
                if (result)
                {
                    ViewBag.cart = "No";
                }
            }
            return View(pmm);
        }
        
        public IActionResult ProductsList(string catDetail = "", string type="", int pageNumber = 1, int pageSize = 12,string ord="default")
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");           
            ViewBag.LoggedUser = loginCheckResponse;
            
            List<CategoriesWithSub> hCats = cService.GetCatsWithSubCats(); ;
            ViewBag.HeadCats = hCats;
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }

            List < ProductDisplayModel > feaprds = pService.GetFeaturedProductList();
            decimal fcnt = feaprds.Count;
            decimal f3c = (Math.Ceiling(fcnt / 3));
            int j = 0;
            List<featureModelGroup> flist = new List<featureModelGroup>();
            for(int i = 0; i < f3c; i++)
            {
                List<ProductDisplayModel> x = new List<ProductDisplayModel>();
                for (int k = 0; k < 3; k++)
                {                    
                    if (fcnt > j)
                    {
                        x.Add(feaprds[j]);
                    }
                    j++;
                }
                featureModelGroup y = new featureModelGroup();
                y.fpros = x;
                flist.Add(y);
            }
            ViewBag.fproducts = flist;
            List<ProductHomeDisplayModel> pm = new List<ProductHomeDisplayModel>();
            int lid = 0;
            if (loginCheckResponse != null)
            {
                lid = loginCheckResponse.userId;
            }
            
            string[] ctd = catDetail.Split(",");
            long cId = long.Parse(ctd[0]);
            int catId = Convert.ToInt32(cId);
            long sId = long.Parse(ctd[1]);
            int subcatId = Convert.ToInt32(sId);
            pm = pService.GetCustomerProducts(pageNumber, pageSize, catId, subcatId, type,ord,lid);
            int totalNoOfProducts = pService.GetProductsCount(catId, subcatId, type);
            ViewBag.TotalRecords = totalNoOfProducts;
            ViewBag.pn = pageNumber;
            ViewBag.ps = pageSize;
            ViewBag.ser = type;
            ViewBag.cid = catId;
            ViewBag.sCid = subcatId;
            ViewBag.odr = ord;
            
            ViewBag.totalPages= (Math.Ceiling((decimal)totalNoOfProducts / (decimal)pageSize));
            return View(pm);
        }

        public IActionResult ContactUs()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }
            return View();
        }
        public IActionResult SaveContactInfo(ContactUs req)
        {
            req.createdOn = DateTime.Now;
            ProcessResponse pr = cusService.SaveContactUs(req);
            return RedirectToAction("ContactUs");
        }
        public IActionResult AboutUs()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }
            return View();
        }
        public IActionResult GoToLogin()
        {
            return RedirectToAction("Login", "Authenticate");
        }
        public IActionResult GoToWishList()
        {
            return RedirectToAction("WishList", "Sales");
        }
        public IActionResult DashBoard()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate", new { url = "Home/DashBoard" });
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;
            ViewBag.wishlist = sService.GetWishListProducts(loginCheckResponse.userId);
            ViewBag.countries = cService.GetAllCountries();
            ViewBag.states = cService.GetAllStates(ViewBag.countries[0].CountryId);
            ViewBag.cities = cService.GetAllCities(ViewBag.states[0].StateId);
            ViewBag.address = sService.GetAddress(loginCheckResponse.userId);
            ViewBag.orders = sService.GetOrders(loginCheckResponse.userId);
            ViewBag.AddressTypes = cService.GetAddressTypes();
            UserMasterDisplay ud= uService.GetUserById(loginCheckResponse.userId);
            ud.PWord = PasswordEncryption.Decrypt(ud.PWord);
            ViewBag.profile = ud;
            List<AddressDisplayModel> ad = new List<AddressDisplayModel>();
            ad = sService.GetAddress(loginCheckResponse.userId);
            int c = ad.Count;
            if (ad.Count > 0)
            {
                ViewBag.shippingAddress = ad[0];
            }
            return View();
        }
        //public IActionResult Search(string catDetail="", string type = "",int pageNumber=1,int pageSize=12)
        //{
        //    LoginResponse loginCheckResponse = new LoginResponse();
        //    loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");            
        //    ViewBag.LoggedUser = loginCheckResponse;
        //    ViewBag.HeadCats = cService.GetCatsWithSubCats();

        //    string[] ctd = catDetail.Split(",");
        //    long cId = long.Parse(ctd[0]);
        //    int catId = Convert.ToInt32(cId);
        //    long sId = long.Parse(ctd[1]);
        //    int subcatId = Convert.ToInt32(sId);
        //    List<ProductDisplayModel> pd = new List<ProductDisplayModel>();
        //    pd = sService.ProductSearch(catId, subcatId, type);
        //    return RedirectToAction("ProductsList", new { catId,subcatId,type,pageNumber,pageSize });

        //}
        public IActionResult SaveUser(UserMasterDisplay request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate", new { url = "Home/DashBoard" });
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();

            Users a= uService.GetProfileByUserId(request.UserId); 
            if (request.EmailId != null)
            {
                var emailcheck = uService.GetUserByEmail(request.EmailId);
                if (emailcheck != null)
                {
                    if (emailcheck.UserId != a.UserId)
                    {
                        ModelState.AddModelError("EmailId", "EmailId Already Registred");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("EmailId", "EmailId should not be empty");
            }
            if (request.MobileNumber != null)
            {
                var mobilecheck = uService.GetUserByEmail(request.MobileNumber);
                if (mobilecheck != null)
                {
                    if (mobilecheck.UserId != a.UserId)
                    {
                        ModelState.AddModelError("MobileNumber", "Mobile Number Already Existed");
                    }
                }
                if (request.MobileNumber.Length > 10)
                {
                    ModelState.AddModelError("MobileNumber", "should not be more than 10 digits");
                }
            }
            else
            {
                ModelState.AddModelError("MobileNumber", "Mobile Number should not be empty");
            }
            string pw = request.PWord;
            if (request.PWord != null)
            {
                foreach (var x in pw)
                {
                    if (x == ' ')
                    {
                        ModelState.AddModelError("PWord", "spaces are not allowed in password");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                UserMasterDisplay um = new UserMasterDisplay();
                Users u = new Users();
                bool isProductImageUploaded = false;
                string productImageName = "";
                if (request.ProfileImageUploaded != null)
                {
                    var fileNameUploaded = Path.GetFileName(request.ProfileImageUploaded.FileName);
                    if (fileNameUploaded != null)
                    {
                        var contentType = request.ProfileImageUploaded.ContentType;
                        string filename = DateTime.UtcNow.ToString();
                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                        filename = filename + RandomGenerator.RandomString(4, false);
                        string extension = Path.GetExtension(fileNameUploaded);
                        filename += extension;
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                        var filePath = Path.Combine(uploads, filename);
                        request.ProfileImageUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                        productImageName = filename;
                        isProductImageUploaded = true;
                    }
                }
                if (request.UserId > 0)
                {
                    u = uService.GetProfileByUserId(request.UserId);
                    u.Firstname = request.FirstName;
                    u.LastName = request.LastName;
                    u.MobileNumber = request.MobileNumber;
                    u.EmailId=request.EmailId;
                    u.ProfileImage = isProductImageUploaded ? productImageName : u.ProfileImage;
                    if (request.PWord != null)
                    {
                        u.PWord = PasswordEncryption.Encrypt(request.PWord);
                    }
                }
                else
                {
                    CloneObjects.CopyPropertiesTo(request, u);
                    u.ProfileImage = isProductImageUploaded ? productImageName : null;
                }
                ProcessResponse pr = uService.SaveUsers(u);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("DashBoard");
                }
            }
            return RedirectToAction("DashBoard");
        }
        public IActionResult PrivacyPolicy()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }
            return View();
        }
        public IActionResult refundPolicy()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }
            return View();
        }
        public IActionResult FAQs()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }
            return View();
        }
        public IActionResult GetUserPassword(string pw)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate", new { url = "Home/DashBoard" });
            }

            UserMasterDisplay um = uService.GetUserById(loginCheckResponse.userId);
            string str = PasswordEncryption.Decrypt(um.PWord);
            string res = "No";
            if (str == pw)
            {
                res = "yes";
            }
            return Json(new { r = res });
        }
        public IActionResult CancelOrder(int id)
        {
            sService.CancelBookedOrder(id);
            return RedirectToAction("DashBoard");
        }
        public IActionResult ReturnOrder(int id)
        {
            sService.ReturnOrder(id);
            return RedirectToAction("DashBoard");
        }
        public IActionResult OrderDtails(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (loginCheckResponse != null)
            {
                List<CartDetails> list = sService.GetCartProducts(loginCheckResponse.userId);
                ViewBag.cartprods = list;
                ViewBag.CartCount = list.Count;
            }

            CustOrderDetail od=sService.GetDetailsOfOrder(id);

            return View(od);
        }
    }
}
