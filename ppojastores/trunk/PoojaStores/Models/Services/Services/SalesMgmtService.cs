using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using PoojaStores.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Services
{
    public class SalesMgmtService : ISalesMgmtService
    {
        private readonly ISalesMgmtRepo sRepo;
        private readonly INotificationService nService;

        public SalesMgmtService(ISalesMgmtRepo _sRepo,INotificationService _nService)
        {
            sRepo = _sRepo;
            nService = _nService;
        }

        public ProcessResponse SaveWishList(WishList request)
        {
            ProcessResponse response = new ProcessResponse();
            if(request.WishListId>0)
            {
                response = sRepo.UpdateWishList(request);
            }
            else
            {
                response = sRepo.SaveWishList(request);
            }
            return response;
        }

        public WishList GetWishListById(int id)
        {
            return sRepo.GetWishListById(id);
        }

        public List<ProductDisplayModel> GetWishListProducts(int id)
        {
            return sRepo.GetWishListProducts(id);
        }

        public bool IsProduct(int pid,int uid)
        {
            return sRepo.IsProductInWishList(pid,uid);
        }

        public List<ProductDisplayModel> ProductSearch(int catId,int subcatId,string type="")
        {
            return sRepo.ProductSearch(catId, subcatId, type);
        }
            
        public ProcessResponse SaveCart(Cart request)
        {
            ProcessResponse pr = new ProcessResponse();
            if(request.CartId>0)
            {
                pr = sRepo.UpdateCarts(request);
            }
            else
            {
                pr = sRepo.SaveCart(request);
            }
            return pr;
        }

        public List<CartDetails> GetCartProducts(int id)
        {
            return sRepo.GetCartProducts(id);
        }
        public List<OrderDisplayForCustomer> GetOrders(int id)
        {
            return sRepo.GetOredrs(id);
        }
        public CustOrderDetail GetDetailsOfOrder(int id)
        {
            return sRepo.GetDetailsOfOrder(id);
        }
        public ProcessResponse CancelBookedOrder(int id)
        {
            ProcessResponse pr = sRepo.CancelBookedOrder(id);
            Users u = sRepo.GetUserByOrderId(id);
            PODetails pod = sRepo.GetPODetailsByPODetId(id);
            nService.SendCanceltoMobile(u.MobileNumber, pod.OrderId);
            return pr;
        }
        public ProcessResponse ReturnOrder(int id)
        {
            return sRepo.ReturnOrder(id);
        }
        public Cart GetCartById(int id)
        {
            return sRepo.GetCartById(id);
        }

        //public Cart GetCartProductByUserId(int pid,int uid)
        //{
        //    return sRepo.GetCartProductByUserId(pid, uid);
        //}
        public WishList GetProductsByProductId(int uid,int pid)
        {
            return sRepo.GetProductsByProductId(uid, pid);
        }

        public ProcessResponse SaveAddress(Address request)
        {
            ProcessResponse pr = new ProcessResponse();
            if(request.Id>0)
            {
                pr = sRepo.UpdateAddress(request);
            }
            else
            {
                pr = sRepo.SaveAddress(request);
            }
            return pr;
        }

        public List<AddressDisplayModel> GetAddress(int id)
        {
            return sRepo.GetAddress(id);

        }

        public Address GetAddressById(int id)
        {
            return sRepo.GetAddressById(id);
        }

        public bool IsCart(int uid,int pid)
        {
            return sRepo.IsCart(uid, pid);
        }
        public ProcessResponse SaveCartDetails(CartDetails request)
        {
            ProcessResponse pr = new ProcessResponse();
            if (request.CartDetailId > 0)
            {
                pr = sRepo.UpdateCartDetails(request);
            }
            else
            {
                pr = sRepo.SaveCartDetails(request);
            }
            return pr;
        }
        public CartDetails GetCartDetailsById(int cid,int pid)
        {
            return sRepo.GetCartDetailsByProductId(cid, pid);
        }
        public CartDetails GetCartDetailsMaster(int id)
        {
            return sRepo.GetCartDetailsById(id);
        }
        public List<CartDetails> GetCartDetailsByCartId(int id)
        {
            return sRepo.GetCartDetailsByCartId(id);
        }
        public ProcessResponse SavePOMaster(POMaster request)
        {
            ProcessResponse p = new ProcessResponse();
            if (request.POID > 0)
            {
                p = sRepo.UpdatePOMaster(request);
            }else
            {
                p = sRepo.SavePOMaster(request);
            }
            return p;
        }
        public POMaster GetPOMasterByPOMId(int id)
        {
            return sRepo.GetPOMasterByPOMId(id);
        }
        public POMaster getPOMasterByCartId(int cartId)
        {
            return sRepo.getPOMasterByCartId(cartId);
        }
        public ProcessResponse SavePODetails(PODetails request)
        {
            ProcessResponse p = new ProcessResponse();
            if (request.PODetailId > 0)
            {
                p = sRepo.UpdatePODetails(request);
            }
            else
            {
                p = sRepo.SavePODETails(request);
            }
            return p;
        }
        public PODetails GetPODetailsByPODetId(int id)
        {
            return sRepo.GetPODetailsByPODetId(id);
        }
        public PODetails getPODetailsByMasterId(int id)
        {
            return sRepo.getPODetailsByMasterId(id);    
        }

        public int GetPreviousDetailId()
        {
            return sRepo.GetPreviousDetailId();
        }
        public AdminDashboard GetDashboardModel()
        {
            return sRepo.GetDashboardModel();
        }
    }
}
