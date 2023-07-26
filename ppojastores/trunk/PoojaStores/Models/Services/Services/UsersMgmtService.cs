using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using PoojaStores.Models.Services.Interfaces;
using PoojaStores.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services
{
    public class UsersMgmtService : IUsersMgmtService
    {
        private readonly IUsersMgmtRepo uRepo;
        private readonly INotificationService nService;
        public UsersMgmtService(IUsersMgmtRepo _uRepo,INotificationService _nService)
        {
            uRepo = _uRepo;
            nService = _nService;
        }
        public ProcessResponse SaveUsers(Users request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                if (request.UserId > 0)
                {
                    response = uRepo.UpdateUsers(request);
                }
                else
                {
                    request.IsDeleted = false;
                    response = uRepo.SaveUsers(request);
                    if (response.statusCode == 1)
                    {
                        string password = PasswordEncryption.Decrypt(request.PWord);
                        ProcessResponse p = nService.SendLoginCredentials(AppSettings.EmailTemplates.RegistrationDetails, request.EmailId, request.Firstname, password);
                    }
                }
            }
            catch (Exception ex)
            {
                uRepo.LogError(ex);
            }

            return response;
        }
        public List<UserMasterDisplay> GetAllUsers(int uid)
        {
            return uRepo.GetAllUsers(uid);
        }
        public UserMasterDisplay GetUserById(int id)
        {
            return uRepo.GetUserById(id);
        }
        public Users GetProfileByUserId(int id)
        {
            return uRepo.GetProfileByUserId(id);
        }
        public LoginResponse LoginCheck(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            try
            {

                response = uRepo.LoginCheck(request);
                return response;
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "Failed to login";
                return response;
            }
        }
        public Users GetUserByEmail(string email)
        {
            return uRepo.GetUserByEmail(email);
        }
        public ProcessResponse InitiateResetPassword(string emailId)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                ps = uRepo.InitiateResetPassword(emailId);
                if (ps.statusCode == 1)
                {
                    // send Message
                    var udata = uRepo.GetUserByEmail(emailId);
                    string emailotp = uRepo.GetEmailOtp(udata.UserId);
                    var email = nService.SendResetPasswordMobile(udata.MobileNumber, emailotp, (udata.Firstname + udata.LastName));
                    //var email = nService.SendForgetPasswordRequest(AppSettings.EmailTemplates.OtpForForgetPassword, emailId, (udata.Firstname + udata.LastName), emailotp);
                }
            }
            catch (Exception ex)
            {
                ps.statusCode = 0;
                ps.statusMessage = "failed";
            }
            return ps;
        }
        public ProcessResponse CompletePasswordRequest(string key, int userid, string pword)
        {
            ProcessResponse ps = new ProcessResponse();

            ps = uRepo.CompletePasswordRequest(key, userid, pword);

            return ps;
        }
    }
}

    

