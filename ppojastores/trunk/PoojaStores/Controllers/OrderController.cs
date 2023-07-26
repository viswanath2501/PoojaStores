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
    public class OrderController : Controller
    {
        private readonly IOrderMgmtService oService;
        private readonly ISalesMgmtService sService;
        private readonly IHostingEnvironment hostingEnvironment;
        public OrderController(IOrderMgmtService _oSer,IHostingEnvironment host,ISalesMgmtService sSer)
        {            
            oService = _oSer;
            hostingEnvironment = host;
            sService = sSer;
        }
        public IActionResult OpenOrders()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrdersMastersDisplay> res= oService.GetOpenOrderMasters();
            return View(res);
        }
        public IActionResult OpenOrderDetails(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            OrderDetailPage result = oService.GetDetailsOfOpenOrder(id);
            return View(result);
        }
        public IActionResult DispatchDetails(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            DispatchDetailModel dd = new DispatchDetailModel();
            dd.PoDetailId = id;
            return View(dd);
        }
        [HttpPost]
        public IActionResult DispatchDetails(DispatchDetailModel request)
        {
            if (ModelState.IsValid)            {
                
                string mainfilename = "";
                if (request.InvoiceDocument != null)
                {
                        var fileNameUploaded = Path.GetFileName(request.InvoiceDocument.FileName);
                        if (fileNameUploaded != null)
                        {
                            var conentType = request.InvoiceDocument.ContentType;
                            string filename = DateTime.UtcNow.ToString();
                            filename = Regex.Replace(filename, @"[\[\]\\\^\$\.\|\?\*\+\(\)\{\}%,;: ><!@#&\-\+\/]", "");
                            filename = Regex.Replace(filename, "[A-Za-z ]", "");
                            filename = filename + RandomGenerator.RandomString(4, false);
                            string extension = Path.GetExtension(fileNameUploaded);
                            filename += extension;
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProductImages");
                            var filePath = Path.Combine(uploads, filename);
                            request.InvoiceDocument.CopyTo(new FileStream(filePath, FileMode.Create));
                            
                            mainfilename = filename;
                    }
                }
                DispatchDetails dd = new DispatchDetails();
                CloneObjects.CopyPropertiesTo(request, dd);
                dd.InvoiceDocumentUrl = mainfilename;
                dd.IsDeleted = false;
                ProcessResponse pr = oService.SaveDispatchDetails(dd);
                if (pr.statusCode == 1)
                {
                    pr = oService.DispatchPODetail((int)request.PoDetailId);
                    if (pr.statusCode == 2)
                    {
                        return RedirectToAction("DispatchedOrders");
                    }
                    else
                    {
                        return RedirectToAction("OpenOrderDetails", new { id = pr.currentId });
                    }
                }
            }
            return View(request);
        }
        public IActionResult DispatchedOrders()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrdersMastersDisplay> res = oService.GetDispatchedOrderMasters();
            return View(res);
        }
        public IActionResult DispatchedOrderDetails(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            OrderDetailPage result = oService.GetDetailsOfDispatchedOrder(id);
            return View(result);
        }
        public IActionResult DeliveryDetails(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            DispatchDetails dd = oService.GetDispatchDetailsByPoId(id);
            return View(dd);
        }
        [HttpPost]
        public IActionResult DeliveryDetails(DispatchDetails request)
        {
            ProcessResponse pr = oService.UpdateDeliveryDetails(request);
            if (pr.statusCode == 1)
            {
                pr = oService.DeliverPODetail((int)request.PoDetailId);
                if (pr.statusCode == 2)
                {
                    return RedirectToAction("ClosedOrders");
                }
                else
                {
                    return RedirectToAction("DispatchedOrderDetails", new { id = pr.currentId });
                }
            }
            return View(request);
        }
        public IActionResult ClosedOrders()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrdersMastersDisplay> res = oService.GetClosedOrderMasters();
            return View(res);
        }
        public IActionResult ClosedOrderDetails(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            OrderDetailPage result = oService.GetDetailsOfClosedOrder(id);
            return View(result);
        }
        public IActionResult CancledOrders()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrderDetailDisplay> result = oService.GetCancledOrders();
            return View(result);
        }
        public IActionResult RefundDetails(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ViewBag.detailsId = id;
            RefundDetailsModel res = oService.GetRefundDetails(id);
            return View(res);
        }
        public IActionResult RefundedAmount(int id)
        {
            PODetails pod = sService.GetPODetailsByPODetId(id);
            pod.RefundedOn = DateTime.Now;
            pod.PaymentStatus = "Refunded";
            pod.RefundStatus = true;
            sService.SavePODetails(pod);

            return RedirectToAction("CancledOrders");
        }
        public IActionResult CancledRefundOrders()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrderDetailDisplay> result = oService.GetCancledOrdersReturn();
            return View(result);
        }
        
        public IActionResult OrdersToBeRetuened()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrderDetailDisplay> result = oService.GetOrdersToBereturned();
            return View(result);
        }
        public IActionResult ReturnAOrder(int id)
        {
            ProcessResponse pr= oService.ReturnAOrder(id);
            if (pr.statusCode == 1)
            {
                return RedirectToAction("ToBeRefundedOrders");
            }
            else
            {
                return RedirectToAction("OrdersToBeRetuened");
            }
        }
        public IActionResult ToBeRefundedOrders()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrderDetailDisplay> result = oService.GetOrdersForAmountReturned();
            return View(result);
        }
        public IActionResult RefundDetailsOfReturnedOrder(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            ViewBag.detailsId = id;
            RefundDetailsModel res = oService.GetRefundDetails(id);
            return View(res);
        }
        public IActionResult RefundedReturnedAmount(int id)
        {
            PODetails pod = sService.GetPODetailsByPODetId(id);
            pod.RefundedOn = DateTime.Now;
            pod.PaymentStatus = "Refunded";
            pod.RefundStatus = true;
            sService.SavePODetails(pod);

            return RedirectToAction("RetunedOrders");
        }
        public IActionResult RetunedOrders()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrderDetailDisplay> result = oService.GetOrdersGorReturned();
            return View(result);
        }
    }
}
