using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
    public interface IUsersMgmtService
    {
        ProcessResponse SaveUsers(Users request);
        List<UserMasterDisplay> GetAllUsers(int uid);
        UserMasterDisplay GetUserById(int id);
        Users GetProfileByUserId(int id);
        LoginResponse LoginCheck(LoginRequest request);
        Users GetUserByEmail(string email);
        ProcessResponse InitiateResetPassword(string emailId);
        ProcessResponse CompletePasswordRequest(string key, int userid, string pword);
    }
}
