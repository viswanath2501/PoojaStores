using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Interfaces
{
    public interface ISalesMgmtRepo
    {
        ProcessResponse SaveWishList(WishList request);

        WishList GetWishListById(int id);
        
        bool IsProductInWishList(int pid, int uid);

        ProcessResponse UpdateWishList(WishList request);
        
        List<ProductDisplayModel> GetWishListProducts(int id);
        
        ProcessResponse SaveCart(Cart request);
        Cart GetCartById(int id);


        List<CartDetails> GetCartProducts(int id);

        WishList GetProductsByProductId(int uid, int pid);
        List<ProductDisplayModel> ProductSearch(int catId = 0, int subcatId = 0, string type = "", int pagenumber = 1, int pageSize = 12);
        ProcessResponse UpdateCarts(Cart request);
        List<OrderDisplayForCustomer> GetOredrs(int id);
        CustOrderDetail GetDetailsOfOrder(int id);
        ProcessResponse CancelBookedOrder(int id);
        ProcessResponse ReturnOrder(int id);
        bool IsCart(int uid, int pid);
        //Cart GetCartProductByUserId(int pid, int uid);
        ProcessResponse SaveAddress(Address request);

        List<AddressDisplayModel> GetAddress(int id);

        Address GetAddressById(int id);

        ProcessResponse UpdateAddress(Address request);
        ProcessResponse SaveCartDetails(CartDetails request);
        List<CartDetails> GetCartDetails();
        CartDetails GetCartDetailsById(int id);
        List<CartDetails> GetCartDetailsByCartId(int id);
        CartDetails GetCartDetailsByProductId(int cid, int pid);
        ProcessResponse UpdateCartDetails(CartDetails requst);
        ProcessResponse SavePOMaster(POMaster request);
        POMaster GetPOMasterByPOMId(int id);
        POMaster getPOMasterByCartId(int cartId);
        ProcessResponse UpdatePOMaster(POMaster request);
        ProcessResponse SavePODETails(PODetails request);
        PODetails GetPODetailsByPODetId(int id);
        PODetails getPODetailsByMasterId(int id);
        ProcessResponse UpdatePODetails(PODetails request);
        int GetPreviousDetailId();
        AdminDashboard GetDashboardModel();
        Users GetUserByOrderId(int id);
        void LogError(Exception ex);
    }
}
