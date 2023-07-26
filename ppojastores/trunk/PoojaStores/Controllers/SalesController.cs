using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class SalesController : Controller
    {
        private readonly ISalesMgmtService iService;
        private readonly ICommonDropsMgmtService cService;
        private readonly IProductMgmtService pService;
        private readonly IPaymentService payService;
        private readonly IConfiguration _config;
        private readonly IUsersMgmtService uService;
        private readonly INotificationService notificationService;
        private readonly IMasterMgmtService mService;
        public SalesController(ISalesMgmtService _iService, ICommonDropsMgmtService _cService, IProductMgmtService _pService,
            IPaymentService _payService, IConfiguration config, IUsersMgmtService _uService , INotificationService _NService,IMasterMgmtService _mService)
        {
            iService = _iService;
            cService = _cService;
            pService = _pService;
            payService = _payService;
            _config = config;
            uService = _uService;
            notificationService = _NService;
            mService = _mService;
        }
        [HttpPost]
        public IActionResult SaveWishList(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                ProcessResponse p = new ProcessResponse();
                p.statusCode = 5;
                p.statusMessage = "Please Login";
                return Json(new { result = p });
            }
            ViewBag.LoggedUser = loginCheckResponse;
            bool res = iService.IsProduct(id, loginCheckResponse.userId);
            ProcessResponse pr = new ProcessResponse();
            if (res)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Already Existed";
                return Json(new { result = pr });
            }
            else
            {
                bool resu = iService.IsCart(loginCheckResponse.userId, id);
                if (resu)
                {
                    pr.statusCode = 4;
                    pr.statusMessage = "Already In Cart";
                    return Json(new { result = pr });
                }
                else
                {
                    WishList wl = new WishList();
                    wl.UserId = loginCheckResponse.userId;
                    wl.ProductId = id;
                    wl.AddedOn = DateTime.Now;
                    wl.IsDeleted = false;
                    pr = iService.SaveWishList(wl);
                    return Json(new { result = pr });
                }
            }
        }

        public IActionResult MoveToWishlist(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                ProcessResponse p = new ProcessResponse();
                p.statusCode = 5;
                p.statusMessage = "Please Login";
                return Json(new { result = p });
            }
            ViewBag.LoggedUser = loginCheckResponse;

            CartDetails cd = iService.GetCartDetailsMaster(id);
            cd.IsDeleted = true;        
            ProcessResponse pr= iService.SaveCartDetails(cd);
            if (pr.statusCode == 1)
            {
                WishList wl = new WishList();
                wl.UserId = loginCheckResponse.userId;
                wl.ProductId = cd.ProductId;
                wl.AddedOn = DateTime.Now;
                wl.IsDeleted = false;
                pr = iService.SaveWishList(wl);                
            }
            return RedirectToAction("WishList");
        }

        public IActionResult WishList()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate", new { url = "Sales/Wishlist" });
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();

            List<ProductDisplayModel> pm = iService.GetWishListProducts(loginCheckResponse.userId);

            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;
            return View(pm);
        }

        public IActionResult SaveCart(int id, int qty = 0)
        {
            ProcessResponse pr = new ProcessResponse();
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                pr.statusCode = 5;
                pr.statusMessage = "Please Login";
                return Json(new { result = pr });
            }
            ProductHomeDisplayModel prod = pService.GetProDisplMdl(id);
            if (qty == 0)
            {
                qty = (int)prod.MinimumOrderQty;
            }
            if (qty > prod.QtyAvailable || qty<prod.MinimumOrderQty)
            {
                
                if (qty > prod.QtyAvailable)
                {
                    pr.statusCode = 2;
                    pr.statusMessage = "Out Of Stock";
                }
                else
                {
                    pr.statusCode = 3;
                    pr.statusMessage = "Please Select Minimim Order Quantitty";
                }
                return Json(new { result = pr });
            }
            else
            {
                Cart old = iService.GetCartById(loginCheckResponse.userId);
                if (old == null)
                {
                    Cart c = new Cart();

                    c.CartUserId = loginCheckResponse.userId;
                    c.CreatedOn = DateTime.Now;
                    c.IsDeleted = false;
                    c.CurrentStatus = "Open";
                    pr = iService.SaveCart(c);
                    old = iService.GetCartById(loginCheckResponse.userId);
                }
                CartDetails cd = iService.GetCartDetailsById(old.CartId, id);
                if (cd == null)
                {
                    CartDetails crtdet = new CartDetails();
                    crtdet.CartId = old.CartId;
                    crtdet.ProductId = id;
                    crtdet.Image1 = prod.Image1;
                    crtdet.Title = prod.Title;
                    crtdet.Price = prod.DiscountedPrice;
                    crtdet.GStPrice = prod.GSTPrice;
                    crtdet.NumberProducts = qty;
                    crtdet.CurrentStatus = "Open";
                    crtdet.AdededOn = DateTime.Now;
                    crtdet.IsDeleted = false;
                    pr = iService.SaveCartDetails(crtdet);
                }
                else
                {
                    pr.statusCode = 0;
                    pr.statusMessage = "Already Exists";
                }

                WishList w = iService.GetProductsByProductId(loginCheckResponse.userId, id);

                if (w != null)
                {
                    w.IsDeleted = true;
                    ProcessResponse p = iService.SaveWishList(w);
                }
                return Json(new { result = pr });
            }
        }

        public IActionResult Cart(int id)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate", new { url = "Sales/Cart" });
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;

            return View(list);
        }

        public IActionResult UpdateCart(int cartId, int newCnt)
        {
            ProcessResponse pr = new ProcessResponse();
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                pr.statusCode = 5;
                pr.statusMessage = "Please Login";
                return Json(new { result = pr });
            }

            CartDetails c = iService.GetCartDetailsMaster(cartId);

            ProductMain pm = pService.GetProductById((int)c.ProductId);
            if (pm.QtyAvailable >= newCnt)
            {
                c.NumberProducts = newCnt;
                pr = iService.SaveCartDetails(c);
            }
            else
            {
                pr.statusCode = 0;
                pr.statusMessage = "Requested Qty Exceds Available Qty";
            }
            return Json(new { result = pr });
        }

        public IActionResult SaveAddress(Address request)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");

            request.UserId = loginCheckResponse.userId;
            request.IsDeleted = false;
            ProcessResponse pr = iService.SaveAddress(request);
            return Json(new { result = pr });
        }

        public IActionResult GetAddressById(int id)
        {
            Address ad = new Address();
            if (id > 0)
            {
                ad = iService.GetAddressById(id);
            }
            return Json(new { result = ad });


        }

        public IActionResult DeleteAddress(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            Address cm = iService.GetAddressById(id);
            cm.IsDeleted = true;
            pr = iService.SaveAddress(cm);
            return Json(new { result = pr });
        }

        public IActionResult DeleteCart(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                pr.statusCode = 5;
                pr.statusMessage = "Please Login";
                return Json(new { result = pr });
            }

            CartDetails cm = iService.GetCartDetailsMaster(id);
            cm.IsDeleted = true;
            pr = iService.SaveCartDetails(cm);
            return Json(new { result = pr });
        }

        public IActionResult DeleteWishList(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            WishList wl = iService.GetWishListById(id);
            wl.IsDeleted = true;
            pr = iService.SaveWishList(wl);
            return Json(new { result = pr });
        }

        public IActionResult CheckOut()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;
            ViewBag.countries = cService.GetAllCountries();
            ViewBag.states = cService.GetAllStates(ViewBag.countries[0].CountryId);
            ViewBag.cities = cService.GetAllCities(ViewBag.states[0].StateId);
            ViewBag.AddressTypes = cService.GetAddressesType();
            ViewBag.Prods = iService.GetCartProducts(loginCheckResponse.userId);
            List<AddressDisplayModel> ad = iService.GetAddress(loginCheckResponse.userId);
            ViewBag.address = ad;
            ViewBag.addressCount = ad.Count();

            return View();
        }
        public IActionResult AcceptCheckout()
        {
            LoginResponse loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);

            bool res = true;
            foreach(CartDetails cd in list)
            {
                ProductMain pm = pService.GetProductById((int)cd.ProductId);
                if (pm.QtyAvailable < cd.NumberProducts)
                {
                    res = false;
                }
            }

            return Json(new { r = res });
        }
        public IActionResult CodSuccess(int adId)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;
            UserMasterDisplay u = uService.GetUserById(loginCheckResponse.userId);
            Cart activeCart = iService.GetCartById(loginCheckResponse.userId);
            try
            {
                if (list != null)
                {
                    POMaster poM = new POMaster();
                    poM.CartId = activeCart.CartId;
                    poM.CreatedOn = DateTime.Now;
                    poM.CurrentStatus = "UnPaid";
                    poM.InstrumentDetails = "COD";
                    poM.IsDeleted = false;
                    poM.OrderStatus = "Ready To Dispatch";
                    decimal tot = 0;
                    foreach (CartDetails d in list)
                    {
                        for (int i = 0; i < d.NumberProducts; i++)
                        {
                            tot += (decimal)(d.GStPrice + d.Price);
                        }
                    }
                    poM.OrderAmount = tot;
                    poM.PaidAmount = 0;
                    poM.PaidDate = null;
                    poM.BankCharges = 0;
                    poM.BankTaxes = 0;
                    poM.PaymentMethod = "COD";
                    poM.PONumber = "DP-ON-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + loginCheckResponse.userId.ToString() + "-" + RandomGenerator.RandomString(3, false);
                    poM.TransactionId = "NA";
                    poM.CustomerId = loginCheckResponse.userId;
                    ProcessResponse pr = iService.SavePOMaster(poM);
                    int x = 0;
                    int previousdetailId = iService.GetPreviousDetailId();
                    if (pr.statusCode == 1)
                    {
                        poM.POID = pr.currentId;
                        foreach (CartDetails d in list)
                        {
                            ProductMain pm = pService.GetProductById((int)d.ProductId);
                            DiscountMaster dim = mService.GetDiscountById((int)pm.DiscountMasterId);
                            PODetails pde = new PODetails();
                            pde.AddedOn = d.AdededOn;
                            pde.CurrentStatus = "Ready To Dispatch";
                            pde.GST = d.GStPrice;
                            pde.IsDeleted = false;
                            pde.NumberOfItems = d.NumberProducts;
                            pde.POMasterId = poM.POID;
                            pde.ProductId = d.ProductId;
                            pde.PaymentStatus = "UnPaid";
                            pde.RetunStatus = false;
                            pde.CancelStatus = false;
                            pde.RefundedAmount = 0;
                            pde.TotalPrice = (d.NumberProducts * (d.GStPrice + d.Price));
                            pde.Discount = dim.DiscountPercentage;
                            if (x > 0)
                            {
                                pde.OrderId = "D-P" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + (++pr.currentId);
                            }
                            else
                            {
                                pde.OrderId = "D-P" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + (++previousdetailId);
                            }
                            pr = iService.SavePODetails(pde);
                            
                            //Send Emails and sms
                            notificationService.SendOrderReceived(u.MobileNumber, pde.OrderId);
                            x++;
                        }
                        // place an entry in followup
                        PoFollowUp pf = new PoFollowUp();
                        pf.FollowUpBy = loginCheckResponse.userId;
                        pf.FolloUpOn = DateTime.Now;
                        pf.FollowUpRemarks = "Order Created on " + DateTime.Now.ToString() + ". Packing process will be initiated shortly";
                        pf.POMID = poM.POID;
                        //pf.FollowUpRemarks = "DP-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + loginCheckResponse.userId.ToString() + "-" + RandomGenerator.RandomString(3, false); ;
                        pf.isDeleted = false;
                        payService.SavePoFollowUp(pf);                        
                    }
                    // update cart and details status
                    activeCart.CurrentStatus = "Purchased";
                    pr = iService.SaveCart(activeCart);
                    foreach (CartDetails d in list)
                    {
                        d.CurrentStatus = "Purchased";
                        var s = iService.SaveCartDetails(d);
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("CheckOut");
            }
            return RedirectToAction("CODDone");
        }
        public IActionResult CreateOrder(int adId,string note)
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;

            UserMasterDisplay u = uService.GetUserById(loginCheckResponse.userId);
            Address ad = iService.GetAddressById(adId);
            string rKey = _config.GetValue<string>("OtherConfig:RazorKey");
            string rSecret = _config.GetValue<string>("OtherConfig:RazorSecret");
            Random randomObj = new Random();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();
            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(rKey, rSecret);
            Dictionary<string, object> options = new Dictionary<string, object>();
            decimal tot = 0;
            foreach (CartDetails d in list)
            {
                for (int i = 0; i < d.NumberProducts; i++)
                {
                    tot += (decimal)(d.GStPrice + d.Price);
                }
            }
            int toPayAmount = (int)(tot*100);
            options.Add("amount", toPayAmount);
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0");

            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();
            Cart activeCart = iService.GetCartById(loginCheckResponse.userId);
            activeCart.addressId = adId;
            activeCart.OrderNotes = note;
            ProcessResponse pr = iService.SaveCart(activeCart);
            OrderModel orderModel = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                address = ad.Address1,
                amount = toPayAmount,
                contactNumber = u.MobileNumber,
                currency = "INR",
                description = "Purchase of items",
                email = u.EmailId,
                name = u.FirstName,
                razorpayKey = rKey
            };
            
            return View(orderModel);
        }
        public IActionResult Complete()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);

            UserMasterDisplay u = uService.GetUserById(loginCheckResponse.userId);
            string paymentId = Request.Form["rzp_paymentid"];

            //This is orderId 
            var orderId = Request.Form["rzp_orderid"];
            string rKey = _config.GetValue<string>("OtherConfig:RazorKey");
            string rSecret = _config.GetValue<string>("OtherConfig:RazorSecret");
            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(rKey, rSecret);

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];
            RazorOrderResult rResult = new RazorOrderResult();
            rResult.id = paymentCaptured.Attributes["id"];
            ///rResult.acquirer_data.bank_transaction_id = paymentCaptured.Attributes["acquirer_data"]["bank_transaction_id"];
            rResult.amount = paymentCaptured.Attributes["amount"];
            rResult.amount_refunded = paymentCaptured.Attributes["amount_refunded"];
            rResult.bank = paymentCaptured.Attributes["bank"];
            rResult.captured = paymentCaptured.Attributes["captured"];
            rResult.card_id = paymentCaptured.Attributes["card_id"];
            rResult.contact = paymentCaptured.Attributes["contact"];
            rResult.created_at = paymentCaptured.Attributes["created_at"];
            rResult.currency = paymentCaptured.Attributes["currency"];
            rResult.description = paymentCaptured.Attributes["description"];
            rResult.email = paymentCaptured.Attributes["email"];
            rResult.entity = paymentCaptured.Attributes["entity"];
            rResult.error_code = paymentCaptured.Attributes["error_code"];
            rResult.error_description = paymentCaptured.Attributes["error_description"];
            rResult.error_reason = paymentCaptured.Attributes["error_reason"];
            rResult.error_source = paymentCaptured.Attributes["error_source"];
            rResult.error_step = paymentCaptured.Attributes["error_step"];
            rResult.fee = paymentCaptured.Attributes["fee"];
            rResult.id = paymentCaptured.Attributes["id"];
            rResult.international = paymentCaptured.Attributes["international"];
            rResult.invoice_id = paymentCaptured.Attributes["invoice_id"];
            rResult.method = paymentCaptured.Attributes["method"];
            // rResult.notes.address= paymentCaptured.Attributes["notes"]["address"];
            rResult.order_id = paymentCaptured.Attributes["order_id"];
            rResult.refund_status = paymentCaptured.Attributes["refund_status"];
            rResult.status = paymentCaptured.Attributes["status"];
            rResult.tax = paymentCaptured.Attributes["tax"];
            rResult.vpa = paymentCaptured.Attributes["vpa"];
            rResult.wallet = paymentCaptured.Attributes["wallet"];

            // Check payment made successfully

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                // Create these action method
                Cart activeCart = iService.GetCartById(loginCheckResponse.userId);
                try
                {
                    if (list != null)
                    {
                        POMaster poM = new POMaster();
                        poM.CartId = activeCart.CartId;
                        poM.CreatedOn = DateTime.Now;
                        poM.CurrentStatus = "PaymentSuccess";
                        poM.InstrumentDetails = rResult.order_id;
                        poM.IsDeleted = false;
                        poM.OrderStatus = "Ready To Dispatch";
                        poM.addressId = activeCart.addressId;
                        poM.OrderNotes = activeCart.OrderNotes;
                        decimal tot = 0;
                        foreach (CartDetails d in list)
                        {
                            for (int i = 0; i < d.NumberProducts; i++)
                            {
                                tot += (decimal)(d.GStPrice + d.Price);
                            }
                        }
                        poM.OrderAmount = tot;
                        poM.PaidAmount = rResult.amount / 100;
                        poM.PaidDate = DateTime.Now;
                        poM.BankCharges = rResult.fee / 100;
                        poM.BankTaxes = rResult.tax / 100;
                        poM.PaymentMethod = rResult.method;
                        poM.PONumber = "DP-ON-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + loginCheckResponse.userId.ToString() + "-" + RandomGenerator.RandomString(3, false);
                        poM.TransactionId = rResult.id;
                        poM.CustomerId = loginCheckResponse.userId;
                        ProcessResponse pr = iService.SavePOMaster(poM);
                        int x = 0;
                        int previousdetailId = iService.GetPreviousDetailId();
                        if (pr.statusCode == 1)
                        {
                            poM.POID = pr.currentId;
                            foreach (CartDetails d in list)
                            {
                                ProductMain pm = pService.GetProductById((int)d.ProductId);
                                DiscountMaster dim = mService.GetDiscountById((int)pm.DiscountMasterId);
                                PODetails pde = new PODetails();
                                pde.AddedOn = d.AdededOn;
                                pde.CurrentStatus = "Ready To Dispatch";
                                pde.GST = d.GStPrice;
                                pde.IsDeleted = false;
                                pde.NumberOfItems = d.NumberProducts;
                                pde.POMasterId = poM.POID;
                                pde.ProductId = d.ProductId;
                                pde.RetunStatus = false;
                                pde.CancelStatus = false;
                                pde.RefundedAmount = 0;
                                pde.RefundStatus = false;
                                
                                pde.PaymentStatus = "Payment Success";
                                pde.TotalPrice = (d.NumberProducts * (d.GStPrice + d.Price));
                                pde.Discount = dim.DiscountPercentage;
                                if (x > 0)
                                {
                                    pde.OrderId = "D-P" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + (++pr.currentId);
                                }
                                else
                                {
                                    pde.OrderId = "D-P" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + (++previousdetailId);
                                }
                                pr = iService.SavePODetails(pde);
                                
                                //Send Emails and sms
                                notificationService.SendOrderReceived(u.MobileNumber, pde.OrderId);
                                x++;
                            }
                            // place an entry in followup
                            PoFollowUp pf = new PoFollowUp();
                            pf.FollowUpBy = loginCheckResponse.userId;
                            pf.FolloUpOn = DateTime.Now;
                            pf.FollowUpRemarks = "Order Created on " + DateTime.Now.ToString() + ". Packing process will be initiated shortly";
                            pf.POMID = poM.POID;
                            pf.isDeleted = false;
                            payService.SavePoFollowUp(pf);                            
                        }
                        // post values in razor result table
                        RazorPaymentResult rm = new RazorPaymentResult();
                        rm.TransId = rResult.id;
                        rm.amount = rResult.amount;
                        rm.order_id = rResult.order_id;
                        rm.method = rResult.method;
                        rm.bank = rResult.bank;
                        rm.card_id = rResult.card_id;
                        rm.wallet = rResult.wallet;
                        rm.vpa = rResult.vpa;
                        rm.fee = rResult.fee;
                        rm.tax = rResult.tax;

                        rm.POID = poM.POID;
                        rm.IsDeleted = false;
                        var rpr = payService.SaveRazorresult(rm);

                        // update cart and details status
                        activeCart.CurrentStatus = "Purchased";
                        pr = iService.SaveCart(activeCart);
                        foreach (CartDetails d in list)
                        {
                            ProductMain promin = pService.GetProductById((int)d.ProductId);
                            d.CurrentStatus = "Purchased";
                            var s = iService.SaveCartDetails(d);
                            promin.QtyAvailable -= d.NumberProducts;
                            pService.UpdateProduct(promin);

                        }
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("CheckOut");
                }
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                return RedirectToAction("PaymentFailed");
            }
        }
        public IActionResult PaymentSuccess()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {   
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;

            return View();
        }
        public IActionResult CODDone()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;

            return View();
        }
        public IActionResult PaymentFailed()
        {
            LoginResponse loginCheckResponse = new LoginResponse();
            loginCheckResponse = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loginCheckResponse == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.LoggedUser = loginCheckResponse;
            ViewBag.HeadCats = cService.GetCatsWithSubCats();
            List<CartDetails> list = iService.GetCartProducts(loginCheckResponse.userId);
            ViewBag.cartprods = list;
            ViewBag.CartCount = list.Count;

            return View();
        }
    }
}
