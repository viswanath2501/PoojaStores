using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
    public interface ISalesMgmtService
    {
        ProcessResponse SaveWishList(WishList request);
        WishList GetWishListById(int id);
        List<ProductDisplayModel> GetWishListProducts(int id);
        bool IsProduct(int pid, int uid);
        List<ProductDisplayModel> ProductSearch(int catId, int subcatId, string type = "");
        ProcessResponse SaveCart(Cart request);
        bool IsCart(int uid, int pid);
        Cart GetCartById(int id);
        //Cart GetCartProductByUserId(int pid, int uid);
        List<CartDetails> GetCartProducts(int id);
        WishList GetProductsByProductId(int uid, int pid);
        ProcessResponse SaveAddress(Address request);
        List<OrderDisplayForCustomer> GetOrders(int id);
        CustOrderDetail GetDetailsOfOrder(int id);
        ProcessResponse CancelBookedOrder(int id);
        ProcessResponse ReturnOrder(int id);
        List<AddressDisplayModel> GetAddress(int id);
        Address GetAddressById(int id);
        ProcessResponse SaveCartDetails(CartDetails request);
        CartDetails GetCartDetailsById(int cid, int pid);
        CartDetails GetCartDetailsMaster(int id);
        List<CartDetails> GetCartDetailsByCartId(int id);
        ProcessResponse SavePOMaster(POMaster request);
        POMaster GetPOMasterByPOMId(int id);

        public POMaster getPOMasterByCartId(int cartId);

        public ProcessResponse SavePODetails(PODetails request);

        public PODetails GetPODetailsByPODetId(int id);

        public PODetails getPODetailsByMasterId(int id);
        int GetPreviousDetailId();
        AdminDashboard GetDashboardModel();
    }
}
