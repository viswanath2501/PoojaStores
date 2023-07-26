using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Interfaces
{
    public interface IUsersMgmtRepo
    {
        ProcessResponse SaveUsers(Users request);
        List<UserMasterDisplay> GetAllUsers(int uid);
        UserMasterDisplay GetUserById(int id);
        Users GetProfileByUserId(int id);
        ProcessResponse UpdateUsers(Users request);
        LoginResponse LoginCheck(LoginRequest request);
        Users GetUserByEmail(string email);
        ProcessResponse InitiateResetPassword(string emailId);
        ProcessResponse CompletePasswordRequest(string emailOtp, int userid, string pword);
        string GetEmailOtp(int id);
        void LogError(Exception ex);

    }
}
