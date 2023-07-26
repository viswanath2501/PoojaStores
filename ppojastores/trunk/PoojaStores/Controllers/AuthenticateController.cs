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
    public class AuthenticateController : Controller
    {
        private readonly IUsersMgmtService uService;
        private readonly ICommonDropsMgmtService cService;
        private readonly IMasterMgmtService mService;
        public AuthenticateController(IUsersMgmtService _uService, ICommonDropsMgmtService _cService, IMasterMgmtService _mService)
        {
            uService = _uService;
            cService = _cService;
            mService = _mService;
        }
        public IActionResult Login(string url="Home/Index")
        {
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            ViewBag.url = url;
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (string.IsNullOrEmpty(request.emailid) || string.IsNullOrEmpty(request.pword))
            {
                ViewBag.ErrorMessage = "Fill Mandatory fields";
                return View(request);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var loginCheck = uService.LoginCheck(request);
                    if (loginCheck.statusCode == 1)
                    {
                        SessionHelper.SetObjectAsJson(HttpContext.Session, "loggedUser", loginCheck);
                        if (loginCheck.userTypeName == "Admin")
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else
                        {
                            string[] pag = request.url.Split("/");
                            return RedirectToAction(pag[1], pag[0]);
                        }                        
                    }
                    else
                    {
                        ViewBag.ErrorMessage = loginCheck.statusMessage;
                        ViewBag.url = request.url;
                        return View(request);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid emailid / mobile number or password";
                    // ViewBag.CaptchaKey = "6LeplcYUAAAAAJlmUhStKiuJ6ucEqdotoWTYomZf";
                    ViewBag.url = request.url;
                    return View(request);
                }
            }
        }

        public IActionResult Logout()
        {
            
            SessionHelper.SetObjectAsJson(HttpContext.Session, "loggedUser", null);
            return RedirectToAction("Index", "Home");


        }

        public IActionResult Register()
        {
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            Users u = new Users();
            return View(u);
        }
        [HttpPost]
        public IActionResult Register(Users request)
        {

            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (request.EmailId != null)
            {
                var emailcheck = uService.GetUserByEmail(request.EmailId);
                if (emailcheck != null)
                {
                    ModelState.AddModelError("EmailId", "EmailId Already Registred");
                }
            }
            if (request.MobileNumber != null)
            {
                var mobilecheck = uService.GetUserByEmail(request.MobileNumber);
                if (mobilecheck != null)
                {
                    ModelState.AddModelError("MobileNumber", "Mobile Number Already Registered");
                }
            }
            try
            {
                int x = request.MobileNumber.Length;
                if (x > 10)
                {
                    ModelState.AddModelError("MobileNumber", "More than 10 digits not allowed");
                }
                long pho = long.Parse(request.MobileNumber);
            }catch(Exception e)
            {
                ModelState.AddModelError("MobileNumber", "Please enter only Numbers");
            }
            string pw = request.PWord;
            foreach(var a in pw)
            {
                if(a==' ')
                {
                    ModelState.AddModelError("PWord", "spaces are not allowed in password");
                }
            }
            if (ModelState.IsValid)
            {               
                    request.IsDeleted = false;
                    request.ActivatedOn = DateTime.Now;
                    request.RegisteredOn = DateTime.Now;
                    request.CurrentStatus = "Active";
                    request.IsEmailVerified = false;
                    request.ProfileImage = "Dummy.png";
                    UserTypeMaster um = mService.GetUserTypeByName("Customer");
                    request.UserTypeId = um.TypeId;
                    ProcessResponse p = uService.SaveUsers(request);

                    if (p.statusCode == 1)
                    {
                        return RedirectToAction("Login");
                    }                
            }           
            return View(request);
        }
        public IActionResult ForgetPassword()
        {
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(LoginRequestForPWChange req)
        {
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            if (ModelState.IsValid)
            {
                ProcessResponse ps = uService.InitiateResetPassword(req.emailId);
                if (ps.statusCode == 1)
                {
                    return RedirectToAction("ResetPassword", new { cc = ps.currentId });
                }
                else
                {
                    ViewBag.ErrorMessage = ps.statusMessage;
                }
            }
            return View();
        }
        public IActionResult ResetPassword(int cc)
        {
            ViewBag.HeadCats = cService.GetCatsWithSubCats();

            PasswordChangeRequest request = new PasswordChangeRequest();
            request.UserId = cc;
            return View(request);
        }
        [HttpPost]
        public IActionResult ResetPassword(PasswordChangeRequest request)
        {
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            ViewBag.LoggedUser = null;
            if (ModelState.IsValid)
            {
                ProcessResponse ps = uService.CompletePasswordRequest(request.key, request.UserId, request.password);
                if (ps.statusCode == 1)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.ErrorMessage = ps.statusMessage;
                }
            }
            return View(request);
        }
    }
}
