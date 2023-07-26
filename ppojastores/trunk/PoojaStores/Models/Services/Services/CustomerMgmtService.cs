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
    public class CustomerMgmtService : ICustomerMgmtService
    {
        private readonly ICustomerMgmtRepo custRepo;
        public CustomerMgmtService(ICustomerMgmtRepo _cuRep)
        {
            custRepo = _cuRep;
        }

        public List<Users> GetUsers(string ser = "", int pageNumber = 1, int pagiSize = 10)
        {
            return custRepo.GetUsers(ser, pageNumber, pagiSize);
        }
        public int GetUserCount(string ser = "")
        {
            return custRepo.GetUserCount(ser);
        }
        public List<OrdersMastersDisplay> GetAllCustomerOrders(int id)
        {
            return custRepo.GetAllCustomerOrders(id);
        }
        public List<OrderDetailDisplay> GetDetailsOfCustomerOrder(int id)
        {
            return custRepo.GetDetailsOfCustomerOrder(id);
        }
        public Users GetUserById(int id)
        {
            return custRepo. GetUserById(id);
        }
        public ProcessResponse DeleteUser(int id)
        {
            return custRepo.DeleteUser(id);
        }
        public ProcessResponse SaveContactUs(ContactUs req)
        {
            return  custRepo.SaveContactUs(req);
        }
        public List<ContactUs> GetAllContactUs()
        {
            return custRepo.GetAllContactUs();
        }
    }
}
