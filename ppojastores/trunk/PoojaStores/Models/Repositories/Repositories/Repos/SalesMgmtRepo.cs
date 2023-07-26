using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class SalesMgmtRepo : ISalesMgmtRepo
    {
        private readonly MyDbContext context;

        public SalesMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }

        public ProcessResponse SaveWishList(WishList request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.wishLists.Add(request);
                context.SaveChanges();
                response.currentId = request.WishListId;
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch(Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }
            return response;
        }

        public WishList GetWishListById(int id)
        {
            return context.wishLists.Where(a => a.IsDeleted == false && a.WishListId == id).FirstOrDefault();
        }

        public List<ProductDisplayModel> GetWishListProducts(int id)
        {
            List<WishList> wl= context.wishLists.Where(a => a.IsDeleted == false && a.UserId == id).ToList();
            List<ProductDisplayModel> pd = new List<ProductDisplayModel>();
            foreach (WishList w in wl) {
                ProductDisplayModel p = (from c in context.productMains
                                         join d in context.discountMasters on c.DiscountMasterId equals d.DiscountId
                      where c.IsDeleted == false && c.ProductId == w.ProductId
                      select new ProductDisplayModel
                      {
                          CartId=w.WishListId,
                          ProductMainImage1 = c.ProductMainImage1,
                          ProductId = c.ProductId,
                          ProductTitle = c.ProductTitle,
                          SellingPrice = (c.SellingPrice/100)*(100-d.DiscountPercentage),
                          QtyAvailable = c.QtyAvailable,

                      }).FirstOrDefault();
                p.SellingPrice = Math.Round((decimal)p.SellingPrice, 2);
                pd.Add(p);
            }
            return pd;
        }

        public ProcessResponse UpdateWishList(WishList request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                request.IsDeleted = false;
                response.currentId = request.WishListId;
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }

        public bool IsProductInWishList(int pid,int uid)
        {
            WishList w = context.wishLists.Where(a => a.IsDeleted == false && a.ProductId == pid && a.UserId == uid).FirstOrDefault();
            bool result = false;
            if(w!=null)
            {
                result = true;
            }
            return result;
        }

        public ProcessResponse SaveCart(Cart request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                request.addressId = 0;
                context.carts.Add(request);
                context.SaveChanges();
                request.IsDeleted = false;
                pr.statusCode = 1;
                pr.currentId = request.CartId;
                pr.statusMessage = "Success";
            }
            catch(Exception ex)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Failed";
                LogError(ex);
            }
            return pr;
        }
        
        public List<CartDetails> GetCartProducts(int id)
        {
            List<CartDetails> pm = new List<CartDetails>();
            try
            {
                Cart cr = context.carts.Where(a => a.IsDeleted == false && a.CartUserId == id && a.CurrentStatus=="Open").OrderByDescending(a => a.CartId).FirstOrDefault();
                pm = context.cartDetails.Where(a => a.IsDeleted == false && a.CartId == cr.CartId && a.CurrentStatus=="Open").ToList();
            }
            catch(Exception e) { }
            return pm;
        }

        public Cart GetCartById(int id)
        {
            return context.carts.Where(a => a.IsDeleted == false && a.CartUserId == id && a.CurrentStatus=="Open").FirstOrDefault();
        }

        //public Cart GetCartProductByUserId(int pid,int uid)
        //{
        //    return context.carts.Where(a => a.IsDeleted == false && a.ProductId == pid && a.CartUserId == uid).FirstOrDefault();
        //}

        public WishList GetProductsByProductId(int uid,int pid)
        {
            return context.wishLists.Where(a => a.IsDeleted == false && a.UserId == uid && a.ProductId == pid).FirstOrDefault();
        }

        public List<ProductDisplayModel> ProductSearch(int catId=0,int subcatId=0,string type="",int pagenumber=1,int pageSize=12)
        {
            List<ProductDisplayModel> response = new List<ProductDisplayModel>();

            if(catId>0)
            {
                if(subcatId>0)
                {
                    response = (from p in context.productMains
                               join ct in context.categoryMasters on p.CategoryId equals ct.CategoryId
                                where p.IsDeleted == false && p.CategoryId == catId && p.SubcategoryId == subcatId &&
                      (p.ProductTitle.StartsWith(type) || p.ProductDescription.Contains(type) || p.SQUID.Contains(type))
                       select new ProductDisplayModel
                       {
                        ProductTitle=p.ProductTitle,
                        ProductDescription=p.ProductDescription,
                        ProductMainImage1=p.ProductMainImage1,
                        ProductMainImage2=p.ProductMainImage2,
                        SellingPrice=p.SellingPrice,
                        Category_Name=ct.CategoryName
                    }).ToList();                   
                }
                else
                {
                    response = (from p in context.productMains
                                join ct in context.categoryMasters on p.CategoryId equals ct.CategoryId
                                where p.IsDeleted == false && p.CategoryId == catId && p.SubcategoryId == subcatId &&
                      (p.ProductTitle.StartsWith(type) || p.ProductDescription.Contains(type) || p.SQUID.Contains(type))
                                select new ProductDisplayModel
                                {
                                    ProductTitle = p.ProductTitle,
                                    ProductDescription = p.ProductDescription,
                                    ProductMainImage1 = p.ProductMainImage1,
                                    ProductMainImage2 = p.ProductMainImage2,
                                    SellingPrice = p.SellingPrice,
                                    Category_Name = ct.CategoryName
                                }).ToList();
                }
            }
            else
            {
                response = (from p in context.productMains
                            join ct in context.categoryMasters on p.CategoryId equals ct.CategoryId
                            where p.IsDeleted == false && p.CategoryId == catId && p.SubcategoryId == subcatId &&
                  (p.ProductTitle.StartsWith(type) || p.ProductDescription.Contains(type) || p.SQUID.Contains(type))
                            select new ProductDisplayModel
                            {
                                ProductTitle = p.ProductTitle,
                                ProductDescription = p.ProductDescription,
                                ProductMainImage1 = p.ProductMainImage1,
                                ProductMainImage2 = p.ProductMainImage2,
                                SellingPrice = p.SellingPrice,
                                Category_Name = ct.CategoryName
                            }).ToList();
            }
            return response;
        }
        

        public ProcessResponse UpdateCarts(Cart request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                pr.currentId = request.CartId;
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            catch(Exception ex)
            {
                pr.statusMessage = "Failed";
                pr.statusCode = 0;
                LogError(ex);
            }
            return pr;

        }

        public bool IsCart(int uid,int pid)
        {
            bool res = false;
            Cart c = context.carts.Where(a => a.IsDeleted == false && a.CartUserId == uid && a. CurrentStatus=="Open").FirstOrDefault();
            CartDetails cd = new CartDetails();
            if (c != null)
            {
                cd = context.cartDetails.Where(a => a.IsDeleted == false && a.CartId == c.CartId && a.ProductId == pid && a.CurrentStatus == "Open").FirstOrDefault();

                if (cd != null)
                {
                    res = true;
                }
            }
            return res;
        }

        public ProcessResponse SaveAddress(Address request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.addresses.Add(request);
                context.SaveChanges();
                response.currentId = request.Id;
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed";
                LogError(ex);
            }
            return response;
        }

        public List<AddressDisplayModel> GetAddress(int id)
        {
            List<AddressDisplayModel> ad = new List<AddressDisplayModel>();

            ad = (from a in context.addresses
                  join u in context.users on a.UserId equals u.UserId
                  join adt in context.addressTypes on a.AddressTypeId equals adt.Id
                  join c in context.countryMasters on a.CountryId equals c.Id
                  join s in context.stateMasters on a.StateId equals s.Id
                  join ct in context.cityMasters on a.CityId equals ct.Id
                  where a.IsDeleted==false&&a.UserId==id

                  select new AddressDisplayModel
                  {
                      Id=a.Id,
                      UserId=a.UserId,
                      LandMark=a.LandMark,
                      Address1=a.Address1,
                      Address2=a.Address2,
                      LocationStreet=a.LocationStreet,
                      CityName=ct.CityName,
                      StateName=s.StateName,
                      CountryName=c.CountryName,
                      FirstName=u.Firstname,
                      ZipCode=a.ZipCode,
                      FullName=a.FullName,
                      AddressTypeId=a.AddressTypeId,
                      AddressTypeName=adt.AddressTypeName
                      
                  }).OrderBy(b=>b.Id).ToList();
            return ad;
        }

        public Address GetAddressById(int id)
        {
            return context.addresses.Where(a => a.IsDeleted == false && a.Id == id).FirstOrDefault();
        }

        public ProcessResponse UpdateAddress(Address request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                Address a = GetAddressById(request.Id);
                context.Entry(a).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.Id;
                response.statusCode = 1;
                response.statusMessage = "Success";
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
                LogError(ex);
            }
            return response;
        }

        public ProcessResponse SaveCartDetails(CartDetails request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                context.cartDetails.Add(request);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            catch(Exception ex)
            {
                pr.statusCode = 0;
                LogError(ex);
            }
            return pr;
        }
        public List<CartDetails> GetCartDetails()
        {
            return context.cartDetails.Where(a => a.IsDeleted == false).ToList();
        }
        public CartDetails GetCartDetailsById(int id)
        {
            return context.cartDetails.Where(a => a.IsDeleted == false && a.CartDetailId == id).FirstOrDefault();
        }
        public List<CartDetails> GetCartDetailsByCartId(int id)
        {
            return context.cartDetails.Where(a => a.IsDeleted == false && a.CartId == id).ToList();
        }
        public CartDetails GetCartDetailsByProductId(int cid,int pid)
        {
            return context.cartDetails.Where(a => a.IsDeleted == false && a.CartId == cid && a.ProductId == pid).FirstOrDefault();
        }

        public List<OrderDisplayForCustomer> GetOredrs(int id)
        {
            List<OrderDisplayForCustomer> result = new List<OrderDisplayForCustomer>();
            try
            {
                List<Cart> c = context.carts.Where(a => a.IsDeleted == false && a.CartUserId == id && a.CurrentStatus== "Purchased").ToList();
                DateTime curDate = DateTime.Now;
                if (c != null)
                {
                    foreach (var o in c)
                    {
                        List<POMaster> pom = context.pOMasters.Where(a => a.IsDeleted == false && a.CartId == o.CartId && (a.CurrentStatus == "PaymentSuccess" || a.CurrentStatus == "UnPaid")).ToList();

                        if (pom != null)
                        {
                            foreach (var po in pom)
                            {
                                List<OrderDisplayForCustomer> abc = (from d in context.pODetails
                                                                     join p in context.productMains on d.ProductId equals p.ProductId
                                                                     where (d.IsDeleted == false && d.POMasterId == po.POID && d.CurrentStatus != "Delivered")
                                                                     select new OrderDisplayForCustomer
                                                                     {
                                                                         PODetailId = d.PODetailId,
                                                                         AddedOn = d.AddedOn,
                                                                         OrderId = d.OrderId,
                                                                         Image = p.ProductMainImage1,
                                                                         ProductCode = p.ProductColor,
                                                                         CurrentStatus = d.CurrentStatus,
                                                                         TotalPrice = d.TotalPrice,
                                                                         ProductId = d.ProductId,
                                                                         Discount = d.Discount,
                                                                         orderspan = null,
                                                                     }).ToList();
                                result.AddRange(abc);
                            }
                        }
                    }
                    foreach (var o in c)
                    {
                        List<POMaster> pom = context.pOMasters.Where(a => a.IsDeleted == false && a.CartId == o.CartId && (a.CurrentStatus == "PaymentSuccess" || a.CurrentStatus == "UnPaid")).ToList();
                        
                        if (pom != null)
                        {
                            foreach (var po in pom)
                            {
                                List<OrderDisplayForCustomer> abc =(from d in  context.pODetails
                                                               join p in context.productMains on d.ProductId equals p.ProductId
                                                               join dd in context.dispatchDetails on d.PODetailId equals dd.PoDetailId
                                                               where(d.IsDeleted == false && d.POMasterId == po.POID && d.CurrentStatus == "Delivered")
                                                               select new OrderDisplayForCustomer
                                                               { 
                                                                    PODetailId=d.PODetailId,
                                                                    AddedOn=d.AddedOn,
                                                                    OrderId=d.OrderId,
                                                                    Image=p.ProductMainImage1,
                                                                    ProductCode=p.ProductColor,
                                                                    CurrentStatus=d.CurrentStatus,
                                                                    TotalPrice=d.TotalPrice,
                                                                    ProductId=d.ProductId,
                                                                    Discount=d.Discount,
                                                                    //orderspan= curDate - dd.DeliveredOn,
                                                                   orderspan = (curDate-dd.DeliveredOn)
                                                               }).ToList();
                                result.AddRange(abc);
                            }
                        }
                    }
                    
                }
            }
            catch(Exception e) { }
           
            return result;
        }
        public CustOrderDetail GetDetailsOfOrder(int id)
        {
            CustOrderDetail res = new CustOrderDetail();

            try
            {
                DispatchDetails dds = context.dispatchDetails.Where(a => a.IsDeleted == false && a.PoDetailId == id).FirstOrDefault();
                if (dds == null)
                {
                    res = (from od in context.pODetails
                           join p in context.productMains on od.ProductId equals p.ProductId
                           join cat in context.categoryMasters on p.CategoryId equals cat.CategoryId
                           join om in context.pOMasters on od.POMasterId equals om.POID
                           join add in context.addresses on om.addressId equals add.Id
                           where od.IsDeleted == false && od.PODetailId == id
                           select new CustOrderDetail
                           {
                               PODetailId = od.PODetailId,
                               Image = p.ProductMainImage1,
                               OrderId = od.OrderId,
                               CategoryId=cat.CategoryId,
                               CategoryName = cat.CategoryName,
                               ProductId = (int)od.ProductId,
                               ProductName = p.ProductTitle,
                               ProductDescription = p.ProductDescription,
                               TotalValue = od.TotalPrice+od.GST,
                               UnitPrice=od.TotalPrice,
                               GST=od.GST,
                               Address1 = add.Address1,
                               Address2 = add.Address2,
                               LocationStreet = add.LocationStreet,
                               LandMark = add.LandMark,
                               CityName = context.cityMasters.Where(b => b.IsDeleted == false && b.Id == add.CityId).Select(c => c.CityName).FirstOrDefault(),
                               StateName = context.stateMasters.Where(b => b.IsDeleted == false && b.Id == add.StateId).Select(c => c.StateName).FirstOrDefault(),
                               CountryName = context.countryMasters.Where(b => b.IsDeleted == false && b.Id == add.CountryId).Select(c => c.CountryName).FirstOrDefault(),
                               ZipCode = add.ZipCode,
                               FullName = add.FullName,
                               PaymentStatus = od.PaymentStatus
                           }).FirstOrDefault();
                }
                else
                {
                    res = (from od in context.pODetails
                           join p in context.productMains on od.ProductId equals p.ProductId
                           join cat in context.categoryMasters on p.CategoryId equals cat.CategoryId
                           join om in context.pOMasters on od.POMasterId equals om.POID
                           join add in context.addresses on om.addressId equals add.Id
                           join dd in context.dispatchDetails on od.PODetailId equals dd.PoDetailId
                           where od.IsDeleted == false && od.PODetailId == id
                           select new CustOrderDetail
                           {
                               PODetailId = od.PODetailId,
                               Image = p.ProductMainImage1,
                               OrderId = od.OrderId,
                               CategoryName = cat.CategoryName,
                               ProductId = (int)od.ProductId,
                               ProductName = p.ProductTitle,
                               ProductDescription = p.ProductDescription,
                               TotalValue = od.TotalPrice+od.GST,
                               UnitPrice=od.TotalPrice,
                               GST=od.GST,
                               Address1 = add.Address1,
                               Address2 = add.Address2,
                               LocationStreet = add.LocationStreet,
                               LandMark = add.LandMark,
                               CityName = context.cityMasters.Where(b => b.IsDeleted == false && b.Id == add.CityId).Select(c => c.CityName).FirstOrDefault(),
                               StateName = context.stateMasters.Where(b => b.IsDeleted == false && b.Id == add.StateId).Select(c => c.StateName).FirstOrDefault(),
                               CountryName = context.countryMasters.Where(b => b.IsDeleted == false && b.Id == add.CountryId).Select(c => c.CountryName).FirstOrDefault(),
                               ZipCode = add.ZipCode,
                               FullName = add.FullName,
                               PaymentStatus = od.PaymentStatus,
                               InvoiceDocumentUrl=dd.InvoiceDocumentUrl
                           }).FirstOrDefault();
                }
            }catch(Exception e)
            {

            }

            return res;
        }
        public ProcessResponse CancelBookedOrder(int id)
        {
            ProcessResponse p = new ProcessResponse();
            try
            {
                PODetails pod = context.pODetails.Where(a => a.IsDeleted == false && a.PODetailId == id && a.CurrentStatus == "Ready To Dispatch").FirstOrDefault();
                PODetails old = pod;
                pod.CurrentStatus = "cancelled";
                pod.CancelStatus = true;
                pod.RefundStatus = false;
                if(pod.PaymentStatus== "PaymentSuccess")
                {
                    pod.PaymentStatus = "To be returned";
                }
                else
                {
                    pod.PaymentStatus = "Not Received";
                }
                context.Entry(old).CurrentValues.SetValues(pod);
                context.SaveChanges();
                POMaster master = context.pOMasters.Where(a => a.IsDeleted == false && a.POID == pod.POMasterId).FirstOrDefault();
                POMaster oldMaster = master;
                int rtd = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == master.POID && a.CurrentStatus == "Ready To Dispatch").Select(b => b.PODetailId).Count();
                int dis = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == master.POID && a.CurrentStatus == "Dispatched").Select(b => b.PODetailId).Count();
                int del = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == master.POID && a.CurrentStatus == "Delivered").Select(b => b.PODetailId).Count();
                if(rtd>0 && dis>0 && del > 0)
                {
                    master.OrderStatus = "Partially Delivered and Dispatched";
                }
                else if(rtd == 0 && dis > 0 && del > 0)
                {
                    master.OrderStatus = "Partially Delivered";
                }
                else if(rtd > 0 && dis == 0 && del > 0)
                {
                    master.OrderStatus = "Ready to Dispatched and Partially Delivered";
                }
                else if (rtd == 0 && dis == 0 && del > 0)
                {
                    master.OrderStatus = "Delivered";
                }
                else if (rtd > 0 && dis > 0 && del == 0)
                {
                    master.OrderStatus = "Partially Dispatched";
                }
                else if (rtd == 0 && dis > 0 && del == 0)
                {
                    master.OrderStatus = "Dispatched";
                }
                else if (rtd > 0 && dis == 0 && del == 0)
                {
                    master.OrderStatus = "Ready To Dispatch";
                }
                else if (rtd == 0 && dis == 0 && del == 0)
                {
                    master.OrderStatus = "cancelled";
                }
                context.Entry(oldMaster).CurrentValues.SetValues(master);
                context.SaveChanges();
                ProductMain pm = context.productMains.Where(a => a.IsDeleted == false && a.ProductId == pod.ProductId).FirstOrDefault();
                ProductMain pmnew = pm;
                pm.QtyAvailable += pod.NumberOfItems;
                context.Entry(pmnew).CurrentValues.SetValues(pm);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                p.statusCode = 0;
                p.statusMessage = "Plaease try Later";
            }
            return p;
        }
        public ProcessResponse ReturnOrder(int id)
        {
            ProcessResponse p = new ProcessResponse();
            try
            {
                PODetails pod = context.pODetails.Where(a => a.IsDeleted == false && a.PODetailId == id && a.CurrentStatus == "Delivered").FirstOrDefault();
                PODetails old = pod;
                pod.CurrentStatus = "Returned";
                pod.RefundStatus = false;
                //pod.RetunStatus = true;
                if (pod.PaymentStatus == "PaymentSuccess")
                {
                    pod.PaymentStatus = "To be refunded";
                }
                else
                {
                    pod.PaymentStatus = "Not Received";
                }
                context.Entry(old).CurrentValues.SetValues(pod);
                context.SaveChanges();
                POMaster master = context.pOMasters.Where(a => a.IsDeleted == false && a.POID == pod.POMasterId).FirstOrDefault();
                POMaster oldMaster = master;
                int rtd = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == master.POID && a.CurrentStatus == "Ready To Dispatch").Select(b => b.PODetailId).Count();
                int dis = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == master.POID && a.CurrentStatus == "Dispatched").Select(b => b.PODetailId).Count();
                int del = context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == master.POID && a.CurrentStatus == "Delivered").Select(b => b.PODetailId).Count();
                if (rtd > 0 && dis > 0 && del > 0)
                {
                    master.OrderStatus = "Partially Delivered and Dispatched";
                }
                else if (rtd == 0 && dis > 0 && del > 0)
                {
                    master.OrderStatus = "Partially Delivered";
                }
                else if (rtd > 0 && dis == 0 && del > 0)
                {
                    master.OrderStatus = "Ready to Dispatched and Partially Delivered";
                }
                else if (rtd == 0 && dis == 0 && del > 0)
                {
                    master.OrderStatus = "Delivered";
                }
                else if (rtd > 0 && dis > 0 && del == 0)
                {
                    master.OrderStatus = "Partially Dispatched";
                }
                else if (rtd == 0 && dis > 0 && del == 0)
                {
                    master.OrderStatus = "Dispatched";
                }
                else if (rtd > 0 && dis == 0 && del == 0)
                {
                    master.OrderStatus = "Ready To Dispatch";
                }
                else if (rtd == 0 && dis == 0 && del == 0)
                {
                    master.OrderStatus = "Returned";
                }
                context.Entry(oldMaster).CurrentValues.SetValues(master);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                p.statusCode = 0;
                p.statusMessage = "Plaease try Later";
            }
            return p;
        }
        public ProcessResponse UpdateCartDetails(CartDetails requst)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                CartDetails c = context.cartDetails.Where(a => a.IsDeleted == false && a.CartDetailId == requst.CartDetailId).FirstOrDefault();
                context.Entry(c).CurrentValues.SetValues(requst);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            catch(Exception ex)
            {
                pr.statusCode = 0;
                LogError(ex);
            }
            return pr;
        }
        public ProcessResponse SavePOMaster(POMaster request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                context.pOMasters.Add(request);
                context.SaveChanges();
                pr.currentId = request.POID;
                pr.statusCode = 1;
                pr.statusMessage = "Sucesses";
            }
            catch(Exception e) 
            {
                pr.statusMessage = e.Message;
                pr.statusCode = 0;
            }
            return pr;
        }
        public POMaster GetPOMasterByPOMId(int id)
        {
            return context.pOMasters.Where(a => a.IsDeleted == false && a.POID == id).FirstOrDefault();
        }
        public POMaster getPOMasterByCartId(int cartId)
        {
            return context.pOMasters.Where(a => a.IsDeleted == false && a.CartId == cartId).FirstOrDefault();
        }
        public ProcessResponse UpdatePOMaster(POMaster request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                POMaster po = context.pOMasters.Where(a => a.POID == request.POID && a.IsDeleted == false).FirstOrDefault();
                context.Entry(po).CurrentValues.SetValues(request);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Sucesses";
            }catch(Exception ex)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Failed";
                LogError(ex);
            }
            return pr;
        }
        public ProcessResponse SavePODETails(PODetails request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                context.pODetails.Add(request);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.currentId = request.PODetailId;
                pr.statusMessage = "Sucesses";
            }
            catch (Exception e)
            {
                pr.statusMessage = e.Message;
                pr.statusCode = 0;
            }
            return pr;
        }
        public PODetails GetPODetailsByPODetId(int id)
        {
            return context.pODetails.Where(a => a.IsDeleted == false && a.PODetailId == id).FirstOrDefault();
        }
        public PODetails getPODetailsByMasterId(int id)
        {
            return context.pODetails.Where(a => a.IsDeleted == false && a.POMasterId == id).FirstOrDefault();
        }
        public ProcessResponse UpdatePODetails(PODetails request)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                PODetails po = context.pODetails.Where(a => a.PODetailId == request.PODetailId && a.IsDeleted == false).FirstOrDefault();
                context.Entry(po).CurrentValues.SetValues(request);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Sucesses";
            }
            catch (Exception ex)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Failed";
                LogError(ex);
            }
            return pr;
        }
        public int GetPreviousDetailId()
        {
            int result = 0;
            try
            {
                PODetails pod = context.pODetails.OrderBy(a => a.PODetailId).Last();
                if (pod != null)
                {
                    result = pod.PODetailId;
                }
            }catch(Exception e)
            {

            }
            return result;
        }
        public AdminDashboard GetDashboardModel()
        {
            AdminDashboard ad = new AdminDashboard();
            try
            {
                DateTime dt = DateTime.Now;
                DateTime start = new DateTime(dt.Year, dt.Month, 1);
                DateTime end = start.AddMonths(1).AddDays(-1);
                UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Customer").FirstOrDefault();
                ad.NoOfCustomers = context.users.Where(a => a.IsDeleted == false && a.UserTypeId == utm.TypeId).Count();
                ad.NoOfOrdersToBeDisptched = context.pODetails.Where(a => a.IsDeleted == false && a.CurrentStatus == "Ready To Dispatch" && a.CancelStatus == false && a.RetunStatus == false).Count();
                ad.NoOfOrdersToGetReturn = context.pODetails.Where(a => a.IsDeleted == false && a.CurrentStatus == "cancelled" && a.CancelStatus == true && a.RefundStatus == false).Count();
                ad.ThisMonthSales = context.pODetails.Where(a => a.IsDeleted == false && a.CurrentStatus != "cancelled" && a.CurrentStatus != "Returned").Sum(b => b.TotalPrice);
            }catch(Exception e)
            {

            }
            return ad;
        }
        public Users GetUserByOrderId(int id)
        {
            Users u = new Users();
            try
            {
                PODetails pod = context.pODetails.Where(a => a.IsDeleted == false && a.PODetailId == id).FirstOrDefault();
                POMaster pom = context.pOMasters.Where(a => a.IsDeleted == false && a.POID == pod.POMasterId).FirstOrDefault();
                u = context.users.Where(a => a.IsDeleted == false && a.UserId == pom.CustomerId).FirstOrDefault();
            }catch(Exception e)
            {

            }
            return u;
        }
        public void LogError(Exception ex)
        {
            ApplicationErrorLog obj = new ApplicationErrorLog();
            obj.Error = ex.Message != null ? ex.Message : "";
            obj.ExceptionDateTime = DateTime.Now;
            obj.InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "";
            obj.Source = ex.Source;
            obj.Stacktrace = ex.StackTrace != null ? ex.StackTrace : "";
            context.applicationErrorLogs.Add(obj);
            context.SaveChanges();
        }
    }
}
