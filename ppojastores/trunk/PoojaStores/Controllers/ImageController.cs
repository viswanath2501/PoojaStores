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
    public class ImageController : Controller
    {
        private readonly IImageMgmtService iService;
        private readonly ICommonDropsMgmtService cService;
        private readonly IHostingEnvironment hostingEnvironment;
        public ImageController(IImageMgmtService _iService, ICommonDropsMgmtService _cService, IHostingEnvironment _hosting)
        {
            iService = _iService;
            cService = _cService;
            hostingEnvironment = _hosting;
        }
        public IActionResult AllCategoryImages()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<ImageDisplayModel> image = iService.GetAllHomeImages();

            return View(image);
        }
        public IActionResult CategoryImageData(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ImageDisplayModel hi = new ImageDisplayModel();
            if(id>0)
            {
                HomePageImages hm= iService.GetImageById(id);
                CloneObjects.CopyPropertiesTo(hm, hi);
            }
            hi.CategoryDrops = cService.GetCatsDrop();

            return View(hi);
        }
        [HttpPost]
        public IActionResult CategoryImageData(ImageDisplayModel request)
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
                HomePageImages hm = new HomePageImages();
                bool isCategoryImageUploaded = false;
                string categoryImageName = "";
                if (request.CategoryImageUpload != null)
                {
                    var fileNameUploaded = Path.GetFileName(request.CategoryImageUpload.FileName);
                    if (fileNameUploaded != null)
                    {
                        var contentType = request.CategoryImageUpload.ContentType;
                        string filename = DateTime.UtcNow.ToString();
                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                        filename = filename + RandomGenerator.RandomString(4, false);
                        string extension = Path.GetExtension(fileNameUploaded);
                        filename += extension;
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                        var filePath = Path.Combine(uploads, filename);
                        request.CategoryImageUpload.CopyTo(new FileStream(filePath, FileMode.Create));
                        categoryImageName = filename;
                        isCategoryImageUploaded = true;
                    }
                }
                if(request.ImageId>0)
                {

                    hm = iService.GetImageById(request.ImageId);
                    hm.Image = isCategoryImageUploaded ? categoryImageName : hm.Image;
                    hm.DiscountPercent = request.DiscountPercent;
                    hm.ImageDescription = request.ImageDescription;

                    hm.ImageTitle1 = request.ImageTitle1;
                    hm.ImageTitle2 = request.ImageTitle2;
                    hm.NewCost = request.NewCost;
                    hm.OldCost = request.OldCost;
                    if (request.ImageId != 1)
                    {
                        hm.RelatedCategoryId = request.RelatedCategoryId;
                    }                    
                    hm.StartingAt = request.StartingAt;
                    hm.TextOnButton = request.TextOnButton;
                }
                else
                {                    
                    CloneObjects.CopyPropertiesTo(request, hm);
                    hm.Image = isCategoryImageUploaded ? categoryImageName : hm.Image;

                }
                ProcessResponse pr = iService.SaveHomeImage(hm);
                if(pr.statusCode==1)
                {
                    return RedirectToAction("AllCategoryImages");
                }
            }
            request.CategoryDrops = cService.GetCatsDrop();
            return View(request);
        }
        public IActionResult AllHomePageBanners()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<ImageDisplayModel> ims = iService.GetAllHomeBanners();

            return View(ims);
        }
        public IActionResult AllProductPageBanners()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<ImageDisplayModel> ims = iService.GetProductPageBanners();

            return View(ims);
        }
        public IActionResult HomePageBannerData(int id=0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ImageDisplayModel hi = new ImageDisplayModel();
            if (id > 0)
            {
                HomePageImages hm = iService.GetImageById(id);
                CloneObjects.CopyPropertiesTo(hm, hi);
            }           
            hi.CategoryDrops = cService.GetCatsDrop();

            return View(hi);
        }
        [HttpPost]
        public IActionResult HomePageBannerData(ImageDisplayModel request)
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
                HomePageImages hm = new HomePageImages();
                bool isCategoryImageUploaded = false;
                string categoryImageName = "";
                if (request.CategoryImageUpload != null)
                {
                    var fileNameUploaded = Path.GetFileName(request.CategoryImageUpload.FileName);
                    if (fileNameUploaded != null)
                    {
                        var contentType = request.CategoryImageUpload.ContentType;
                        string filename = DateTime.UtcNow.ToString();
                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                        filename = filename + RandomGenerator.RandomString(4, false);
                        string extension = Path.GetExtension(fileNameUploaded);
                        filename += extension;
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                        var filePath = Path.Combine(uploads, filename);
                        request.CategoryImageUpload.CopyTo(new FileStream(filePath, FileMode.Create));
                        categoryImageName = filename;
                        isCategoryImageUploaded = true;
                    }
                }
                if (request.ImageId > 0)
                {

                    hm = iService.GetImageById(request.ImageId);
                    hm.Image = isCategoryImageUploaded ? categoryImageName : hm.Image;
                    hm.DiscountPercent = request.DiscountPercent;
                    hm.ImageDescription = request.ImageDescription;
                    hm.ImageNumber = 10;
                    hm.ImageTitle1 = request.ImageTitle1;
                    hm.ImageTitle2 = request.ImageTitle2;
                    hm.NewCost = request.NewCost;
                    hm.OldCost = request.OldCost;
                    hm.RelatedCategoryId = request.RelatedCategoryId;
                    hm.StartingAt = request.StartingAt;
                    hm.TextOnButton = request.TextOnButton;
                }
                else
                {

                    CloneObjects.CopyPropertiesTo(request, hm);
                    hm.Image = isCategoryImageUploaded ? categoryImageName : hm.Image;
                    hm.ImageNumber = 10;
                    hm.IsDeleted = false;
                    hm.ImageSize = "	900 * 447 px";
                }
                ProcessResponse pr = iService.SaveHomeImage(hm);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("AllHomePageBanners");
                }
            }
            request.CategoryDrops = cService.GetCatsDrop();
            return View(request);
        }
        [HttpPost]
        public IActionResult DeleteBanners(int id)
        {
            ProcessResponse pr = new ProcessResponse();
           HomePageImages cm = iService.GetImageById(id);
            cm.IsDeleted = true;
            pr = iService.SaveHomeImage(cm);
            return Json(new { result = pr });
        }
        public IActionResult ProductPageBannerData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ImageDisplayModel hi = new ImageDisplayModel();
            if (id > 0)
            {
                HomePageImages hm = iService.GetImageById(id);
                CloneObjects.CopyPropertiesTo(hm, hi);
            }
            hi.CategoryDrops = cService.GetCatsDrop();

            return View(hi);
        }
        [HttpPost]
        public IActionResult ProductPageBannerData(ImageDisplayModel request)
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
                HomePageImages hm = new HomePageImages();
                bool isCategoryImageUploaded = false;
                string categoryImageName = "";
                if (request.CategoryImageUpload != null)
                {
                    var fileNameUploaded = Path.GetFileName(request.CategoryImageUpload.FileName);
                    if (fileNameUploaded != null)
                    {
                        var contentType = request.CategoryImageUpload.ContentType;
                        string filename = DateTime.UtcNow.ToString();
                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                        filename = filename + RandomGenerator.RandomString(4, false);
                        string extension = Path.GetExtension(fileNameUploaded);
                        filename += extension;
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                        var filePath = Path.Combine(uploads, filename);
                        request.CategoryImageUpload.CopyTo(new FileStream(filePath, FileMode.Create));
                        categoryImageName = filename;
                        isCategoryImageUploaded = true;
                    }
                }
                if (request.ImageId > 0)
                {

                    hm = iService.GetImageById(request.ImageId);
                    hm.Image = isCategoryImageUploaded ? categoryImageName : hm.Image;
                    hm.DiscountPercent = request.DiscountPercent;
                    hm.ImageDescription = request.ImageDescription;
                    hm.ImageNumber = 11;
                    hm.ImageTitle1 = request.ImageTitle1;
                    hm.ImageTitle2 = request.ImageTitle2;
                    hm.NewCost = request.NewCost;
                    hm.OldCost = request.OldCost;
                    hm.RelatedCategoryId = request.RelatedCategoryId;
                    hm.StartingAt = request.StartingAt;
                    hm.TextOnButton = request.TextOnButton;
                }
                else
                {

                    CloneObjects.CopyPropertiesTo(request, hm);
                    hm.Image = isCategoryImageUploaded ? categoryImageName : hm.Image;
                    hm.ImageNumber = 11;
                    hm.IsDeleted = false;
                    hm.ImageSize= "1520 * 290 px";
                }
                ProcessResponse pr = iService.SaveHomeImage(hm);
                if (pr.statusCode == 1)
                {
                    return RedirectToAction("AllProductPageBanners");
                }
            }
            request.CategoryDrops = cService.GetCatsDrop();
            return View(request);
        }
    }
}
