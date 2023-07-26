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
    public class OrderMgmtService : IOrderMgmtService
    {
        private readonly IOrderMgmtRepo oRepo;
        private readonly INotificationService nService;
        private readonly ISalesMgmtRepo sRepo;
        public OrderMgmtService(IOrderMgmtRepo _oRepo,INotificationService _nService,ISalesMgmtRepo _sRepo)
        {
            oRepo = _oRepo;
            nService = _nService;
            sRepo = _sRepo;
        }
        public List<OrdersMastersDisplay> GetOpenOrderMasters()
        {
            return oRepo.GetOpenOrderMasters();
        }
        public OrderDetailPage GetDetailsOfOpenOrder(int id)
        {
            return oRepo.GetDetailsOfOpenOrder(id);
        }
        public ProcessResponse DispatchPODetail(int id)
        {
            ProcessResponse p= oRepo.DispatchPODetail(id);
            if (p.statusCode == 1)
            {
                DetailSmsModel dd = oRepo.getDisPatchDetailsOfPODetail(id);
                nService.SendDispatchOrderMobile(dd.MobileNumber, dd.OrderId, dd.DispatchedBy);
            }
            return p;
        }
        public ProcessResponse SaveDispatchDetails(DispatchDetails request)
        {
            return oRepo.SaveDispatchDetails(request);
        }
        public List<OrdersMastersDisplay> GetDispatchedOrderMasters()
        {
            return oRepo.GetDispatchedOrderMasters();
        }
        public OrderDetailPage GetDetailsOfDispatchedOrder(int id)
        {
            return oRepo.GetDetailsOfDispatchedOrder(id);
        }
        public DispatchDetails GetDispatchDetailsByPoId(int id)
        {
            return oRepo.GetDispatchDetailsByPoId(id);
        }
        public ProcessResponse UpdateDeliveryDetails(DispatchDetails request)
        {
            return oRepo.UpdateDeliveryDetails(request);
        }
        public ProcessResponse DeliverPODetail(int id)
        {
            ProcessResponse pr = oRepo.DeliverPODetail(id);
            if (pr.statusCode == 1)
            {
                DetailSmsModel dd = oRepo.getDisPatchDetailsOfPODetail(id);
                nService.SendDelivertoMobile(dd.MobileNumber, dd.OrderId);
            }
            return pr;
        }
        public List<OrdersMastersDisplay> GetClosedOrderMasters()
        {
            return oRepo.GetClosedOrderMasters();
        }
        public OrderDetailPage GetDetailsOfClosedOrder(int id)
        {
            return oRepo.GetDetailsOfClosedOrder(id);
        }
        public List<OrderDetailDisplay> GetCancledOrders()
        {
            return oRepo.GetCancledOrders();
        }
        public RefundDetailsModel GetRefundDetails(int id)
        {
            return oRepo.GetRefundDetails(id);
        }
        public List<OrderDetailDisplay> GetCancledOrdersReturn()
        {
            return oRepo.GetCancledOrdersReturn();
        }
        public List<OrderDetailDisplay> GetOrdersToBereturned()
        {
            return oRepo.GetOrdersToBereturned();
        }
        public ProcessResponse ReturnAOrder(int id)
        {
            ProcessResponse pr = oRepo.ReturnAOrder(id);
            Users u = sRepo.GetUserByOrderId(id);
            PODetails pod = sRepo.GetPODetailsByPODetId(id);
            nService.SendReturntoMobile(u.MobileNumber, pod.OrderId);
            return pr;
        }
        public List<OrderDetailDisplay> GetOrdersGorReturned()
        {
            return oRepo.GetOrdersGorReturned();
        }
        public List<OrderDetailDisplay> GetOrdersForAmountReturned()
        {
            return oRepo.GetOrdersForAmountReturned();
        }
        public Address GetDeliveryAddress(int id)
        {
            return oRepo.GetDeliveryAddress(id);
        }
    }
}
