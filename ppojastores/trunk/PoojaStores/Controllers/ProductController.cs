using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class ProductController : Controller
    {
        private readonly IProductMgmtService pService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ICommonDropsMgmtService cService;

        public ProductController(IProductMgmtService _pService, IHostingEnvironment _hosting, ICommonDropsMgmtService _cService)
        {
            pService = _pService;
            hostingEnvironment = _hosting;
            cService = _cService;
        }
        public IActionResult ProductList(int pageNumber = 1, int pageSize = 10, string search = "", int CategoryId = 0, int SubCategoryId = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                loginCheckResponse = new LoginResponse();
                loginCheckResponse.userId = 0;
                loginCheckResponse.userName = "NA"; return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ProductDisplayModelBase pb = new ProductDisplayModelBase();            
            pb.products = pService.GetProductList(pageNumber, pageSize, CategoryId, SubCategoryId, search);
            int totalNoOfProducts= pService.GetProductsCount(CategoryId, SubCategoryId, search);
            ViewBag.TotalRecords = totalNoOfProducts;
            ViewBag.TotalCount = pb.products.Count;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.search = search;
            ViewBag.totalPages= (Math.Ceiling((decimal)totalNoOfProducts / (decimal)pageSize));
            pb.categoryDrops = cService.GetCatsDrop();
            pb.subCategoryDrops = new List<SubCategoryDrop>();
            pb.CategoryId = CategoryId;
            pb.SubCategoryId = SubCategoryId;
            return View(pb);
        }
        public IActionResult ProductData(int id = 0)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ProductMain pm = new ProductMain();
            ProductMasterModel pd = new ProductMasterModel();
            int catId = 0;
            if (id > 0)
            {
                pm = pService.GetProductById(id);
                CloneObjects.CopyPropertiesTo(pm, pd);
                catId = (int)pd.CategoryId;
            }
            else
            {
                pd.IsFeatured = false;
            }

            pd.categoryDrops = cService.GetCatsDrop();
            
            pd.subCategoryDrops = cService.GetSubCatsDrop(catId>0?(int)pd.CategoryId:pd.categoryDrops[0].CategoryId);
            pd.gstDrop = cService.GetGSTDrop();
            pd.measurementDrops = cService.GetMeasurementDrop();
            pd.pojaItemDrops = cService.GetPojaItemDrop();
            pd.pojaServiceDrops = cService.GetPojaServiceDrop();
            pd.specialityDrops = cService.GetSpecialityDrop();
            pd.discountDrops = cService.GetDiscountDrop();
            pd.deliveryDrops = cService.GetDeliveryDrop();

            return View(pd);
        }
        [HttpPost]
        public IActionResult ProductData(ProductMasterModel request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            if (request.ProductId > 0)
            {
                ModelState.Remove("ProductMainImageUploaded");                
            }
            if (request.MinimumOrderQty > request.QtyAvailable)
            {
                ModelState.AddModelError("MinimumOrderQty", "Minimum Order Quantity should not be greater than Available quantity");
            }
            if (ModelState.IsValid)
            {
                ProductMain pm = new ProductMain();
                bool isProductImageUploaded = false;
                string productImageName = "";
                if (request.ProductMainImageUploaded != null)
                {
                    var fileNameUploaded = Path.GetFileName(request.ProductMainImageUploaded.FileName);
                    if (fileNameUploaded != null)
                    {
                        var contentType = request.ProductMainImageUploaded.ContentType;
                        string filename = DateTime.UtcNow.ToString();
                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                        filename = filename + RandomGenerator.RandomString(4, false);
                        string extension = Path.GetExtension(fileNameUploaded);
                        filename += extension;
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                        var filePath = Path.Combine(uploads, filename);
                        request.ProductMainImageUploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                        productImageName = filename;
                        isProductImageUploaded = true;
                    }
                    else
                    {
                        productImageName = "dummy.jpg";
                    }
                }
                else
                {
                    productImageName = "dummy.jpg";
                }

                bool isProductImage2Uploaded = false;
                string productImage2Name = "";
                if (request.ProductMainImage2Uploaded != null)
                {
                    var fileNameUploaded = Path.GetFileName(request.ProductMainImage2Uploaded.FileName);
                    if (fileNameUploaded != null)
                    {
                        var contentType = request.ProductMainImage2Uploaded.ContentType;
                        string filename = DateTime.UtcNow.ToString();
                        filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                        filename = Regex.Replace(filename, "[A-Za-z ]", "");
                        filename = filename + RandomGenerator.RandomString(4, false);
                        string extension = Path.GetExtension(fileNameUploaded);
                        filename += extension;
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                        var filePath = Path.Combine(uploads, filename);
                        request.ProductMainImage2Uploaded.CopyTo(new FileStream(filePath, FileMode.Create));
                        productImage2Name = filename;
                        isProductImage2Uploaded = true;
                    }
                    else
                    {
                        productImage2Name = "dummy.jpg";
                    }
                }
                else
                {
                    productImage2Name = "dummy.jpg";
                }
                if (request.ProductId > 0)
                {
                    pm = pService.GetProductById(request.ProductId);
                    pm.CategoryId = request.CategoryId;
                    pm.SubcategoryId = request.SubcategoryId;
                    pm.ProductTitle = request.ProductTitle;
                    pm.ProductDescription = request.ProductDescription;
                    pm.ActualPrice = request.ActualPrice;
                    pm.SellingPrice = request.SellingPrice;
                    pm.ProductCode = request.ProductCode;
                    pm.SQUID = request.SQUID;
                    pm.BrandName = request.BrandName;
                    pm.MeasurementMasterId = request.MeasurementMasterId;
                    pm.PoojaItemMasterId = request.PoojaItemMasterId;
                    pm.PoojaServiceMasterId = request.PoojaServiceMasterId;
                    pm.SpecialMasterId = request.SpecialMasterId;
                    pm.ReturnPolicyId = request.ReturnPolicyId;
                    pm.DeliveryMasterId = request.DeliveryMasterId;
                    pm.DiscountMasterId = request.DiscountMasterId;
                    pm.GSTMasterId = request.GSTMasterId;
                    pm.MinimumOrderQty = request.MinimumOrderQty;
                    pm.QtyAvailable = request.QtyAvailable;
                    pm.PackLength = request.PackLength;
                    pm.PackHeight = request.PackHeight;
                    pm.PackWidth = request.PackWidth;
                    pm.ActualWeight = request.ActualWeight;
                    pm.VolumetricWeight = request.VolumetricWeight;
                    pm.ProductColor = request.ProductColor;
                    pm.ProductSize = request.ProductSize;
                    pm.IsFeatured = request.IsFeatured;
                    pm.ProductMainImage1 = isProductImageUploaded ? productImageName : pm.ProductMainImage1;
                    pm.ProductMainImage2 = isProductImage2Uploaded ? productImage2Name : pm.ProductMainImage2;
                }
                else
                {
                    CloneObjects.CopyPropertiesTo(request, pm);
                    pm.ProductMainImage1 =productImageName;
                    pm.ProductMainImage2 = productImage2Name;
                    pm.PostedBy = loginCheckResponse.userId;
                    pm.PostedOn = DateTime.Now;
                }
                ProcessResponse pr = pService.UpdateProduct(pm);
                if (pr.statusCode == 1)
                {
                    //save Profile Image
                    if (request.ProductOtherImgsUploaded != null)
                    {
                        foreach (FormFile file in request.ProductOtherImgsUploaded)
                        {
                            var fileNameUploaded = Path.GetFileName(file.FileName);
                            if (fileNameUploaded != null)
                            {
                                var conentType = file.ContentType;
                                string filename = DateTime.UtcNow.ToString();
                                filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                                filename = Regex.Replace(filename, "[A-Za-z ]", "");
                                filename = filename + RandomGenerator.RandomString(4, false);
                                string extension = Path.GetExtension(fileNameUploaded);
                                filename += extension;
                                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                                var filePath = Path.Combine(uploads, filename);
                                file.CopyTo(new FileStream(filePath, FileMode.Create));
                                ProductImages detimages = new ProductImages();
                                detimages.ImageUrl = filename;
                                detimages.IsDeleted = false;
                                detimages.ProductId = request.ProductId;
                                pService.UpdateProductImages(detimages);
                            }
                        }
                    }
                    return RedirectToAction("ProductList");
                }
            }
            

            request.categoryDrops = cService.GetCatsDrop();
            request.subCategoryDrops = cService.GetSubCatsDrop(request.categoryDrops[0].CategoryId);
            request.gstDrop = cService.GetGSTDrop();
            request.measurementDrops = cService.GetMeasurementDrop();
            request.pojaItemDrops = cService.GetPojaItemDrop();
            request.pojaServiceDrops = cService.GetPojaServiceDrop();
            request.specialityDrops = cService.GetSpecialityDrop();
            request.discountDrops = cService.GetDiscountDrop();
            request.deliveryDrops = cService.GetDeliveryDrop();

            return View(request);
        }
       
        
        public IActionResult DeleteProduct(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            ProductMain cm = pService.GetProductById(id);
            cm.IsDeleted = true;
            pr = pService.UpdateProduct(cm);
            return Json(new { result = pr });
        }

        
    }
}
