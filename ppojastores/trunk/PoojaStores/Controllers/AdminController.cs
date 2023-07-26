using Microsoft.AspNetCore.Mvc;
using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Services.Interfaces;
using PoojaStores.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISalesMgmtService sService;
        public AdminController(ISalesMgmtService _sServices)
        {
            sService = _sServices;
        }
        public IActionResult Dashboard()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null || loginCheckResponse.userTypeName != "Admin")
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;

            AdminDashboard ad = sService.GetDashboardModel();
            return View(ad);

            
        }
    }
}
