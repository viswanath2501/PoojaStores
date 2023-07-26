using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Interfaces
{
     public interface IOrderMgmtRepo
    {
        List<OrdersMastersDisplay> GetOpenOrderMasters();
        OrderDetailPage GetDetailsOfOpenOrder(int id);
        ProcessResponse DispatchPODetail(int id);
        ProcessResponse SaveDispatchDetails(DispatchDetails request);
        List<OrdersMastersDisplay> GetDispatchedOrderMasters();
        OrderDetailPage GetDetailsOfDispatchedOrder(int id);
        DispatchDetails GetDispatchDetailsByPoId(int id);
        ProcessResponse UpdateDeliveryDetails(DispatchDetails request);
        ProcessResponse DeliverPODetail(int id);
        List<OrdersMastersDisplay> GetClosedOrderMasters();
        OrderDetailPage GetDetailsOfClosedOrder(int id);
        List<OrderDetailDisplay> GetCancledOrders();
        RefundDetailsModel GetRefundDetails(int id);
        List<OrderDetailDisplay> GetCancledOrdersReturn();
        List<OrderDetailDisplay> GetOrdersToBereturned();
        ProcessResponse ReturnAOrder(int id);
        List<OrderDetailDisplay> GetOrdersGorReturned();
        List<OrderDetailDisplay> GetOrdersForAmountReturned();
        Address GetDeliveryAddress(int id);
        DetailSmsModel getDisPatchDetailsOfPODetail(int id);
        
    }
}
