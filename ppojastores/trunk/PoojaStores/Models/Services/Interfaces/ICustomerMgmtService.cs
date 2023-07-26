using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
    public interface ICustomerMgmtService
    {
        List<Users> GetUsers(string ser = "", int pageNumber = 1, int pagiSize = 10);
        int GetUserCount(string ser = "");
        List<OrdersMastersDisplay> GetAllCustomerOrders(int id);
        List<OrderDetailDisplay> GetDetailsOfCustomerOrder(int id);
        Users GetUserById(int id);
        ProcessResponse DeleteUser(int id);
        ProcessResponse SaveContactUs(ContactUs req);
        List<ContactUs> GetAllContactUs();
    }
}
