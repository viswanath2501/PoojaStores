using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class OrderMgmtRepo : IOrderMgmtRepo
    {
        private readonly MyDbContext context;
        public OrderMgmtRepo(MyDbContext _con)
        {
            context = _con;
        }
        public List<OrdersMastersDisplay> GetOpenOrderMasters()
        {
            List<OrdersMastersDisplay> result = new List<OrdersMastersDisplay>();
            try
            {
                result = (from a in context.pOMasters
                          join u in context.users on a.CustomerId equals u.UserId
                          where a.IsDeleted == false && (a.OrderStatus == "Ready To Dispatch" || a.OrderStatus == "Partially Dispatched" || 
                            a.OrderStatus == "Partially Delivered and Dispatched" || a.OrderStatus== "Ready to Dispatched and Partially Delivered")
                          select new OrdersMastersDisplay
                          {
                              POID=a.POID,
                              CustomerId=a.CustomerId,
                              CustomerName=u.Firstname,
                              CreatedOn=a.CreatedOn,
                              PaymentMethod=a.PaymentMethod,
                              CurrentStatus=a.CurrentStatus,
                              OrderAmount=a.OrderAmount
                          }).ToList();
            }catch(Exception e)
            {

            }
            return result;
        }
        public OrderDetailPage GetDetailsOfOpenOrder(int id)
        {
            OrderDetailPage res = new OrderDetailPage();
            res.odetails = new List<OrderDetailDisplay>();
            try
            {                
                res.odetails = (from a in context.pODetails
                          join p in context.productMains on a.ProductId equals p.ProductId
                          where a.IsDeleted == false && a.CurrentStatus == "Ready To Dispatch" && a.POMasterId == id && a.CancelStatus==false && a.RetunStatus==false
                          select new OrderDetailDisplay
                          {
                              PODetailId=a.PODetailId,
                              POMasterId=a.POMasterId,
                              ProductId=a.ProductId,
                              NumberOfItems=a.NumberOfItems,
                              OrderId=a.OrderId,
                              Image=p.ProductMainImage1,
                              ProductCode=p.ProductCode
                          }).ToList();
                POMaster po = context.pOMasters.Where(b => b.IsDeleted == false && b.POID == id).FirstOrDefault();
                Address ad = context.addresses.Where(a => a.Id == po.addressId).FirstOrDefault();
                res.Address1 = ad.Address1;
                res.Address2 = ad.Address2;
                res.CityId = ad.CityId;
                res.CityName = context.cityMasters.Where(c => c.IsDeleted == false && c.Id == ad.CityId).Select(d => d.CityName).FirstOrDefault();
                res.StateName = context.stateMasters.Where(c => c.IsDeleted == false && c.Id == ad.StateId).Select(d => d.StateName).FirstOrDefault();
                res.CountryName = context.countryMasters.Where(c => c.IsDeleted == false && c.Id == ad.CountryId).Select(d => d.CountryName).FirstOrDefault();
                res.LocationStreet = ad.LocationStreet;
                res.LandMark = ad.LandMark;
                res.ZipCode = ad.ZipCode;
                res.FullName = ad.FullName;
                res.OrderNotes = context.pOMasters.Where(a => a.IsDeleted == false && a.POID == id).Select(b => b.OrderNotes).FirstOrDefault();
            }
            catch (Exception e)
            {

            }
            return res;
        }
        public ProcessResponse DispatchPODetail(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                PODetails podOld = context.pODetails.Where(a => a.IsDeleted == false && a.PODetailId == id && a.CurrentStatus== "Ready To Dispatch").FirstOrDefault();
                PODetails podNew = podOld;
                podOld.CurrentStatus = "Dispatched";
                
                context.Entry(podNew).CurrentValues.SetValues(podOld);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Successfully Dispatched";
                POMaster pomNew = context.pOMasters.Where(a => a.IsDeleted == false && a.POID == podNew.POMasterId).FirstOrDefault();
                POMaster pomOld = pomNew;
                List<PODetails> allRtdDetails = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == podOld.POMasterId && a.CurrentStatus == "Ready To Dispatch").ToList();
                List<PODetails> allDelDetails = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == podOld.POMasterId && a.CurrentStatus == "Delivered").ToList();
                if (allDelDetails.Count == 0 && allRtdDetails.Count==0)
                {
                    pomNew.OrderStatus = "Dispatched";
                    pr.statusCode = 2;
                }
                else if(allDelDetails.Count==0 && allRtdDetails.Count >0)
                {
                    pomNew.OrderStatus = "Partially Dispatched";
                    pr.statusCode = 3;
                    pr.currentId = pomNew.POID;
                }
                else if(allDelDetails.Count>0 && allRtdDetails.Count==0)
                {
                    pomNew.OrderStatus = "Partially Delivered";
                    if (pomNew.PaymentMethod == "COD") {
                        podNew.CurrentStatus = "Partially Paid";
                    }
                    pr.statusCode = 2;
                }
                else
                {
                    pomNew.OrderStatus = "Partially Delivered and Dispatched";
                    if (pomNew.PaymentMethod == "COD")
                    {
                        podNew.CurrentStatus = "Partially Paid";
                    }
                    pr.statusCode = 3;
                    pr.currentId = pomNew.POID;
                }
                context.Entry(pomOld).CurrentValues.SetValues(pomNew);
                context.SaveChanges();
            }
            catch(Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Some Thing Went Wrong Please Try Again";
            }
            return pr;
        }
        public ProcessResponse SaveDispatchDetails(DispatchDetails request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                context.dispatchDetails.Add(request);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Sucess";
            }catch(Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Failed";
            }
            return pr;
        }
        public List<OrdersMastersDisplay> GetDispatchedOrderMasters()
        {
            List<OrdersMastersDisplay> result = new List<OrdersMastersDisplay>();
            try
            {
                result = (from a in context.pOMasters
                          join u in context.users on a.CustomerId equals u.UserId
                          where a.IsDeleted == false && (a.OrderStatus == "Dispatched" || a.OrderStatus == "Partially Dispatched" || a.OrderStatus == "Partially Delivered" || a.OrderStatus == "Partially Delivered and Dispatched")
                          select new OrdersMastersDisplay
                          {
                              POID = a.POID,
                              CustomerId = a.CustomerId,
                              CustomerName = u.Firstname,
                              CreatedOn = a.CreatedOn,
                              PaymentMethod = a.PaymentMethod,
                              CurrentStatus = a.CurrentStatus,
                              OrderAmount = a.OrderAmount
                          }).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public OrderDetailPage GetDetailsOfDispatchedOrder(int id)
        {
            OrderDetailPage res = new OrderDetailPage();
            res.odetails = new List<OrderDetailDisplay>();
            try
            {
                res.odetails = (from a in context.pODetails
                          join p in context.productMains on a.ProductId equals p.ProductId
                          join dd in context.dispatchDetails on a.PODetailId equals dd.PoDetailId
                          where a.IsDeleted == false && a.CurrentStatus == "Dispatched" && a.POMasterId == id && a.CancelStatus == false && a.RetunStatus == false
                                select new OrderDetailDisplay
                          {
                              PODetailId = a.PODetailId,
                              POMasterId = a.POMasterId,
                              ProductId = a.ProductId,
                              NumberOfItems = a.NumberOfItems,
                              OrderId = a.OrderId,
                              Image = p.ProductMainImage1,
                              ProductCode = p.ProductCode,
                              DispatchedBy=dd.DispatchedBy,
                              DispatchedOn=dd.DispatchedOn,
                              DispatchNumber=dd.DispatchNumber
                          }).ToList();
                POMaster po = context.pOMasters.Where(b => b.IsDeleted == false && b.POID == id).FirstOrDefault();
                Address ad = context.addresses.Where(a => a.Id == po.addressId).FirstOrDefault();
                res.Address1 = ad.Address1;
                res.Address2 = ad.Address2;
                res.CityId = ad.CityId;
                res.CityName = context.cityMasters.Where(c => c.IsDeleted == false && c.Id == ad.CityId).Select(d => d.CityName).FirstOrDefault();
                res.StateName = context.stateMasters.Where(c => c.IsDeleted == false && c.Id == ad.StateId).Select(d => d.StateName).FirstOrDefault();
                res.CountryName = context.countryMasters.Where(c => c.IsDeleted == false && c.Id == ad.CountryId).Select(d => d.CountryName).FirstOrDefault();
                res.LocationStreet = ad.LocationStreet;
                res.LandMark = ad.LandMark;
                res.ZipCode = ad.ZipCode;
                res.FullName = ad.FullName;
            }
            catch (Exception e)
            {

            }
            return res;
        }
        public DispatchDetails GetDispatchDetailsByPoId(int id)
        {
            return context.dispatchDetails.Where(a => a.IsDeleted == false && a.PoDetailId == id).FirstOrDefault();
        }
        public ProcessResponse UpdateDeliveryDetails(DispatchDetails request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                DispatchDetails dd = context.dispatchDetails.Where(a => a.IsDeleted == false && a.DisPatchId == request.DisPatchId).FirstOrDefault();
                DispatchDetails old = dd;
                dd.DeliveredOn = request.DeliveredOn;
                context.Entry(old).CurrentValues.SetValues(dd);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }catch(Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Failed";
            }
            return pr;
        }
        public ProcessResponse DeliverPODetail(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                PODetails podOld = context.pODetails.Where(a => a.IsDeleted == false && a.PODetailId == id && a.CurrentStatus == "Dispatched").FirstOrDefault();
                PODetails podNew = podOld;
                podOld.CurrentStatus = "Delivered";
                podOld.PaymentStatus = "PaymentSuccess";
                context.Entry(podNew).CurrentValues.SetValues(podOld);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Successfully Delivered";
                POMaster pomNew = context.pOMasters.Where(a => a.IsDeleted == false && a.POID == podNew.POMasterId).FirstOrDefault();
                POMaster pomOld = pomNew;
                List<PODetails> allRtdDetails = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == podOld.POMasterId && a.CurrentStatus == "Ready To Dispatch").ToList();
                List<PODetails> allDisDetails = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == podOld.POMasterId && a.CurrentStatus == "Dispatched").ToList();
                if (allDisDetails.Count == 0 && allRtdDetails.Count == 0)
                {
                    pomNew.OrderStatus = "Delivered";
                    pomNew.CurrentStatus = "PaymentSuccess";
                    pr.statusCode = 2;
                }
                else if (allDisDetails.Count > 0 && allRtdDetails.Count == 0)
                {
                    pomNew.OrderStatus = "Partially Delivered";
                    if (pomNew.PaymentMethod == "COD")
                    {
                        podNew.CurrentStatus = "Partially Paid";
                    }
                    pr.statusCode = 3;
                    pr.currentId = pomNew.POID;
                }
                else if (allDisDetails.Count > 0 && allRtdDetails.Count > 0)
                {
                    pomNew.OrderStatus = "Partially Delivered and Dispatched";
                    if (pomNew.PaymentMethod == "COD")
                    {
                        podNew.CurrentStatus = "Partially Paid";
                    }
                    pr.statusCode = 3;
                    pr.currentId = pomNew.POID;
                }
                else
                {
                    pomNew.OrderStatus = "Ready to Dispatched and Partially Delivered";
                    if (pomNew.PaymentMethod == "COD")
                    {
                        podNew.CurrentStatus = "Partially Paid";
                    }
                    pr.statusCode = 2;
                }

                context.Entry(pomOld).CurrentValues.SetValues(pomNew);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Some Thing Went Wrong Please Try Again";
            }
            return pr;
        }
        public List<OrdersMastersDisplay> GetClosedOrderMasters()
        {
            List<OrdersMastersDisplay> result = new List<OrdersMastersDisplay>();
            try
            {
                result = (from a in context.pOMasters
                          join u in context.users on a.CustomerId equals u.UserId
                          where a.IsDeleted == false && (a.OrderStatus == "Delivered" || a.OrderStatus == "Partially Delivered" ||
                            a.OrderStatus == "Partially Delivered and Dispatched" || a.OrderStatus == "Ready to Dispatched and Partially Delivered")
                          select new OrdersMastersDisplay
                          {
                              POID = a.POID,
                              CustomerId = a.CustomerId,
                              CustomerName = u.Firstname,
                              CreatedOn = a.CreatedOn,
                              PaymentMethod = a.PaymentMethod,
                              CurrentStatus = a.CurrentStatus,
                              OrderAmount = a.OrderAmount
                          }).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public OrderDetailPage GetDetailsOfClosedOrder(int id)
        {
            OrderDetailPage res = new OrderDetailPage();
            res.odetails = new List<OrderDetailDisplay>();
            try
            {
                res.odetails = (from a in context.pODetails
                                  join p in context.productMains on a.ProductId equals p.ProductId
                                  join dd in context.dispatchDetails on a.PODetailId equals dd.PoDetailId
                                  where a.IsDeleted == false && a.CurrentStatus == "Delivered" && a.POMasterId == id && a.CancelStatus == false && a.RetunStatus == false
                                select new OrderDetailDisplay
                                  {
                                      PODetailId = a.PODetailId,
                                      POMasterId = a.POMasterId,
                                      ProductId = a.ProductId,
                                      NumberOfItems = a.NumberOfItems,
                                      OrderId = a.OrderId,
                                      Image = p.ProductMainImage1,
                                      ProductCode = p.ProductCode,
                                      DispatchedBy = dd.DispatchedBy,
                                      DispatchedOn = dd.DispatchedOn,
                                      DispatchNumber = dd.DispatchNumber,
                                      DeliveredOn=dd.DeliveredOn
                                  }).ToList();
                POMaster po = context.pOMasters.Where(b => b.IsDeleted == false && b.POID == id).FirstOrDefault();
                Address ad = context.addresses.Where(a => a.Id == po.addressId).FirstOrDefault();
                res.Address1 = ad.Address1;
                res.Address2 = ad.Address2;
                res.CityId = ad.CityId;
                res.CityName = context.cityMasters.Where(c => c.IsDeleted == false && c.Id == ad.CityId).Select(d => d.CityName).FirstOrDefault();
                res.StateName = context.stateMasters.Where(c => c.IsDeleted == false && c.Id == ad.StateId).Select(d => d.StateName).FirstOrDefault();
                res.CountryName = context.countryMasters.Where(c => c.IsDeleted == false && c.Id == ad.CountryId).Select(d => d.CountryName).FirstOrDefault();
                res.LocationStreet = ad.LocationStreet;
                res.LandMark = ad.LandMark;
                res.ZipCode = ad.ZipCode;
                res.FullName = ad.FullName;
            }
            catch (Exception e)
            {

            }
            return res;
        }
        public List<OrderDetailDisplay> GetCancledOrders()
        {
            List<OrderDetailDisplay> result = new List<OrderDetailDisplay>();
            try
            {
                result = (from a in context.pODetails
                          join p in context.productMains on a.ProductId equals p.ProductId
                          where a.IsDeleted == false && a.CurrentStatus == "cancelled" && a.CancelStatus==true &&a.RefundStatus==true
                          select new OrderDetailDisplay
                          {
                              PODetailId = a.PODetailId,
                              POMasterId = a.POMasterId,
                              ProductId = a.ProductId,
                              NumberOfItems = a.NumberOfItems,
                              OrderId = a.OrderId,
                              CurrentStatus=a.PaymentStatus,
                              Image = p.ProductMainImage1,
                              ProductCode = p.ProductCode,
                              PaymentStatus=a.PaymentStatus
                          }).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public RefundDetailsModel GetRefundDetails(int id)
        {
            RefundDetailsModel result = new RefundDetailsModel();
            try
            {
                result = (from pod in context.pODetails
                          join pom in context.pOMasters on pod.POMasterId equals pom.POID
                          join raz in context.razorPaymentResults on pom.POID equals raz.POID
                          where pod.IsDeleted==false && pod.PODetailId==id
                          select new RefundDetailsModel
                          {
                              orderedOn=pom.CreatedOn,
                              PaidAmount=pod.TotalPrice,
                              CancelationCharges=0,
                              TransId=raz.TransId,
                              order_id=raz.order_id,
                          }).FirstOrDefault(); 
            }catch(Exception e)
            {

            }
            return result;
        }
        public List<OrderDetailDisplay> GetCancledOrdersReturn()
        {
            List<OrderDetailDisplay> result = new List<OrderDetailDisplay>();
            try
            {
                result = (from a in context.pODetails
                          join p in context.productMains on a.ProductId equals p.ProductId
                          where a.IsDeleted == false && a.CurrentStatus == "cancelled" && a.CancelStatus == true && a.RefundStatus == false
                          select new OrderDetailDisplay
                          {
                              PODetailId = a.PODetailId,
                              POMasterId = a.POMasterId,
                              ProductId = a.ProductId,
                              NumberOfItems = a.NumberOfItems,
                              OrderId = a.OrderId,
                              CurrentStatus = a.PaymentStatus,
                              Image = p.ProductMainImage1,
                              ProductCode = p.ProductCode,
                              PaymentStatus = a.PaymentStatus
                          }).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public List<OrderDetailDisplay> GetOrdersToBereturned()
        {
            List<OrderDetailDisplay> result = new List<OrderDetailDisplay>();
            try
            {
                result = (from a in context.pODetails
                          join p in context.productMains on a.ProductId equals p.ProductId
                          where a.IsDeleted == false && a.PaymentStatus  == "To be refunded" && a.RetunStatus==false && a.RefundStatus==false && a.CurrentStatus == "Returned"
                          select new OrderDetailDisplay
                          {
                              PODetailId = a.PODetailId,
                              POMasterId = a.POMasterId,
                              ProductId = a.ProductId,
                              NumberOfItems = a.NumberOfItems,
                              OrderId = a.OrderId,
                              CurrentStatus = a.PaymentStatus,
                              Image = p.ProductMainImage1,
                              ProductCode = p.ProductCode,
                              PaymentStatus = a.PaymentStatus
                          }).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public ProcessResponse ReturnAOrder(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                PODetails podold = context.pODetails.Where(a => a.IsDeleted == false && a.PODetailId == id && a.PaymentStatus == "To be refunded" && a.CurrentStatus == "Returned").FirstOrDefault();
                PODetails podnew = podold;
                //podnew.CurrentStatus = "Returned";
                podnew.PaymentStatus = "Pending";
                podnew.RetunStatus = true;
                context.Entry(podold).CurrentValues.SetValues(podnew);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }catch(Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Please Try Again";
            }
            return pr;
        }
        public List<OrderDetailDisplay> GetOrdersGorReturned()
        {
            List<OrderDetailDisplay> result = new List<OrderDetailDisplay>();
            try
            {
                result = (from a in context.pODetails
                          join p in context.productMains on a.ProductId equals p.ProductId
                          where a.IsDeleted == false && a.CurrentStatus == "Returned" && a.PaymentStatus=="Refunded" && a.RetunStatus==true && a.RefundStatus==true
                          select new OrderDetailDisplay
                          {
                              PODetailId = a.PODetailId,
                              POMasterId = a.POMasterId,
                              ProductId = a.ProductId,
                              NumberOfItems = a.NumberOfItems,
                              OrderId = a.OrderId,
                              CurrentStatus = a.PaymentStatus,
                              Image = p.ProductMainImage1,
                              ProductCode = p.ProductCode,
                              PaymentStatus = a.PaymentStatus
                          }).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public List<OrderDetailDisplay> GetOrdersForAmountReturned()
        {
            List<OrderDetailDisplay> result = new List<OrderDetailDisplay>();
            try
            {
                result = (from a in context.pODetails
                          join p in context.productMains on a.ProductId equals p.ProductId
                          where a.IsDeleted == false && a.CurrentStatus == "Returned" && a.PaymentStatus== "Pending" && a.RetunStatus == true && a.RefundStatus == false
                          select new OrderDetailDisplay
                          {
                              PODetailId = a.PODetailId,
                              POMasterId = a.POMasterId,
                              ProductId = a.ProductId,
                              NumberOfItems = a.NumberOfItems,
                              OrderId = a.OrderId,
                              CurrentStatus = a.PaymentStatus,
                              Image = p.ProductMainImage1,
                              ProductCode = p.ProductCode,
                              PaymentStatus = a.PaymentStatus
                          }).ToList();
            }
            catch (Exception e)
            {

            }
            return result;
        }
        public Address GetDeliveryAddress(int id)
        {
            Address ad = new Address();
            try
            {
                ad = context.addresses.Where(a => a.Id == id).FirstOrDefault();
            }catch(Exception e)
            {

            }
            return ad;
        }
        public DetailSmsModel getDisPatchDetailsOfPODetail(int id)
        {
            DetailSmsModel r = new DetailSmsModel();
            try
            {
                r = (from Pd in context.pODetails
                     join dd in context.dispatchDetails on Pd.PODetailId equals dd.PoDetailId
                     join pm in context.pOMasters on Pd.POMasterId equals pm.POID
                     join u in context.users on pm.CustomerId equals u.UserId
                     select new DetailSmsModel
                     {
                         DispatchedBy=dd.DispatchedBy,
                         MobileNumber=u.MobileNumber,
                         OrderId=Pd.OrderId
                     }).FirstOrDefault();
            }
            catch(Exception e)
            {

            }
            return r;
        }        
    }
}
