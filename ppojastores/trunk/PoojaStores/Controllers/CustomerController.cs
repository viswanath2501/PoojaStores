using Microsoft.AspNetCore.Mvc;
using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Services.Interfaces;
using PoojaStores.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerMgmtService cusService;
        public CustomerController(ICustomerMgmtService _cService)
        {
            cusService = _cService;
        }
        public IActionResult AllCustomers(string search = "", int pageNumber = 1,int pageSize=10)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<Users> result = cusService.GetUsers(search, pageNumber, pageSize);
            int totalNoOfProducts = cusService.GetUserCount(search);
            ViewBag.TotalRecords = totalNoOfProducts;
            ViewBag.TotalCount = result.Count;
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;
            ViewBag.search = search;
            ViewBag.totalPages = (Math.Ceiling((decimal)totalNoOfProducts / (decimal)pageSize));
            return View(result);
        }
        public IActionResult CustomerOrders(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrdersMastersDisplay> res = cusService.GetAllCustomerOrders(id);
            return View(res);
        }
        public IActionResult CustomerOrderDetails(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<OrderDetailDisplay> res = cusService.GetDetailsOfCustomerOrder(id);
            return View(res);
        }
        public IActionResult DeleteCustomer(int id)
        {
            ProcessResponse pr = cusService.DeleteUser(id);
            return Json(new { res = pr });
        }
        public IActionResult AllContactedDetails()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            List<ContactUs> res = cusService.GetAllContactUs();
            return View(res);
        }
    }
}
