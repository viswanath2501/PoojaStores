using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class CustomerMgmtRepo : ICustomerMgmtRepo
    {
        private readonly MyDbContext context;
        public CustomerMgmtRepo(MyDbContext _con)
        {
            context = _con;
        }
        public List<Users> GetUsers(string ser="",int pageNumber=1,int pagiSize = 10)
        {
            List<Users> res = new List<Users>();
            int skipsize = 0;
            if (string.IsNullOrEmpty(ser))
            {
                ser = "";
            }
            if (pageNumber > 1)
            {
                skipsize = (pageNumber - 1) * pagiSize;
            }
            try
            {
                UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Customer").FirstOrDefault();
                res = context.users.Where(a => a.IsDeleted == false && a.UserTypeId == utm.TypeId &&
                        (a.LastName.Contains(ser) || a.Firstname.Contains(ser) || a.MobileNumber.Contains(ser) || a.EmailId.Contains(ser))
                        ).Skip(skipsize).Take(pagiSize).ToList();
            }
            catch (Exception e)
            {
                
            }
            return res;
        } 
        public int GetUserCount(string ser = "")
        {
            int result = 0;
            if (string.IsNullOrEmpty(ser))
            {
                ser = "";
            }
            try
            {
                UserTypeMaster utm = context.userTypeMasters.Where(a => a.IsDeleted == false && a.TypeName == "Customer").FirstOrDefault();
                result = context.users.Where(a => (a.LastName.Contains(ser) || a.Firstname.Contains(ser) || a.MobileNumber.Contains(ser) || a.EmailId.Contains(ser)) && a.UserTypeId == utm.TypeId && a.IsDeleted==false).Select(b => b.UserId).Count();
            }catch(Exception e)
            {

            }
            return result;
        }
        public List<OrdersMastersDisplay> GetAllCustomerOrders(int id)
        {
            List<OrdersMastersDisplay> result = new List<OrdersMastersDisplay>();
            try
            {
                result = (from a in context.pOMasters
                          where a.IsDeleted == false && a.CustomerId==id
                          select new OrdersMastersDisplay
                          {
                              POID = a.POID,
                              CustomerId = a.CustomerId,                              
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
        public List<OrderDetailDisplay> GetDetailsOfCustomerOrder(int id)
        {
            List<OrderDetailDisplay> result = new List<OrderDetailDisplay>();
            try
            {
                result = (from a in context.pODetails
                          join p in context.productMains on a.ProductId equals p.ProductId
                          where a.IsDeleted == false && a.POMasterId==id
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
        public Users GetUserById(int id)
        {
            Users u = new Users();
            try
            {
                u = context.users.Where(a => a.IsDeleted == false && a.UserId == id).FirstOrDefault();
            }catch(Exception e)
            {

            }
            return u;
        }
        public ProcessResponse DeleteUser(int id)
        {
            ProcessResponse pr = new ProcessResponse();
            try{
                Users u = context.users.Where(a => a.IsDeleted == false && a.UserId == id).FirstOrDefault();
                Users n = u;
                u.IsDeleted = true;
                context.Entry(n).CurrentValues.SetValues(u);
                context.SaveChanges();
                pr.statusCode = 1;
                pr.statusMessage = "Success";
            }
            catch (Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Please try later";
            }
            return pr;
        }
        public ProcessResponse SaveContactUs(ContactUs req)
        {
            ProcessResponse pr = new ProcessResponse();
            try
            {
                context.contactUs.Add(req);
                context.SaveChanges();
                pr.statusMessage = "Success";
                pr.statusCode = 1;
                pr.currentId = req.id;
            }catch(Exception e)
            {
                pr.statusCode = 0;
                pr.statusMessage = "Please Try Again";
            }
            return pr;
        }
        public List<ContactUs> GetAllContactUs()
        {
            return context.contactUs.Where(a => a.IsDeleted == false).ToList();
        }
    }
}
