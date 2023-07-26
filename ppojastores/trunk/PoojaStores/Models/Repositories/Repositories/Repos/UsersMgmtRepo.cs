using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using PoojaStores.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class UsersMgmtRepo : IUsersMgmtRepo
    {
        private readonly MyDbContext context;

        public UsersMgmtRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SaveUsers(Users request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.PWord = PasswordEncryption.Encrypt(request.PWord);
                
                request.IsDeleted = false;
                context.users.Add(request);
                context.SaveChanges();
                response.currentId = request.UserId;
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
        public List<UserMasterDisplay> GetAllUsers(int uid)
        {
            List<UserMasterDisplay> response = new List<UserMasterDisplay>();
            response = (from u in context.users
                        where u.IsDeleted == false && u.UserId==uid
                        select new UserMasterDisplay
                        {
                            FirstName=u.Firstname,
                            EmailId=u.EmailId,
                            MobileNumber=u.MobileNumber
                        }).ToList();

            return response;
        }

        public UserMasterDisplay GetUserById(int id)
        {
            UserMasterDisplay response = new UserMasterDisplay();
            response = (from u in context.users
                        join ut in context.userTypeMasters on u.UserTypeId equals ut.TypeId
                        where (u.IsDeleted == false && u.UserId == id)
                        select new UserMasterDisplay()
                        {
                            FirstName = u.Firstname,
                            LastName=u.LastName,
                            EmailId = u.EmailId,
                            MobileNumber = u.MobileNumber,
                            UserTypeName=ut.TypeName,
                            UserTypeId=u.UserTypeId,
                            UserId=u.UserId,
                            ProfileImage=u.ProfileImage,
                            PWord=u.PWord
                        }).FirstOrDefault();

            return response;
        }
        public Users GetProfileByUserId(int id)
        {
            return context.users.Where(a => a.UserId == id).FirstOrDefault();
        }
        public ProcessResponse UpdateUsers(Users request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                context.Entry(request).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.UserId;
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
        public LoginResponse LoginCheck(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            try
            {


                var obj = (from um in context.users
                           join ut in context.userTypeMasters on um.UserTypeId equals ut.TypeId 
                           
                           where (um.EmailId == request.emailid || um.MobileNumber == request.emailid) &&
                           um.IsDeleted == false && um.CurrentStatus == "Active"
                           select new
                           {
                               um.EmailId,
                               um.UserId,
                               um.UserName,
                               ut.TypeName,
                               um.PWord,
                               um.Firstname

                           }).FirstOrDefault();

                if (obj == null)
                {
                    response.statusCode = 0;
                    response.statusMessage = "Emailid / Mobile number not registered";

                }
                else
                {
                    string pw = PasswordEncryption.Encrypt(request.pword);                   

                    if (!obj.PWord.Equals(pw))
                    {
                        response.statusCode = 0;
                        response.statusMessage = "Password mismatch";

                    }
                    else
                    {
                        response.statusCode = 1;
                        response.statusMessage = "Login success";
                        response.userId = obj.UserId;
                        response.userName = obj.Firstname;
                        response.userTypeName = obj.TypeName;
                        response.emailId = obj.EmailId;
                    }
                }

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
            Users res = new Users();
            try
            {
                res = context.users.Where(m => m.EmailId == email || m.MobileNumber == email || m.UserName == email).FirstOrDefault();
            }catch (Exception e)
            {

            }
            return res;
        }
        public ProcessResponse InitiateResetPassword(string emailId)
        {
            ProcessResponse ps = new ProcessResponse();
            var um = context.users.Where(a => (a.EmailId == emailId || a.MobileNumber==emailId) && a.IsDeleted == false ).FirstOrDefault();            
            if (um != null)
            {
                List<OTPTransactions> extraOts = context.oTPTransactions.Where(a => a.UserId == um.UserId && a.CurrentStatus.Equals("Draft")).ToList();

                foreach (OTPTransactions o in extraOts)
                {
                    o.UsedOn = DateTime.Now;
                    o.CurrentStatus = "Extras";
                    context.Entry(o).CurrentValues.SetValues(o);
                    context.SaveChanges();
                }

                OTPTransactions rp = new OTPTransactions();
                rp.CurrentStatus = "Draft";
                rp.CreatedOn = DateTime.Now;
                rp.EmailOTP = RandomGenerator.RandomNumber(1000, 9999).ToString();
                rp.MobileOTP = RandomGenerator.RandomNumber(1000, 9999).ToString();
                rp.IsDeleted = false;
                rp.UserId = um.UserId;
                context.oTPTransactions.Add(rp);
                context.SaveChanges(); ;
                ps.statusCode = 1;
                ps.currentId = um.UserId;
                ps.statusMessage = rp.MobileOTP;

            }
            else
            {
                ps.statusCode = 0;
                ps.statusMessage = "Email ID not registered or your account is de-activated.";
                ps.currentId = 0;
            }
            return ps;

        }
        public ProcessResponse CompletePasswordRequest(string emailOtp, int userid, string pword)
        {
            ProcessResponse ps = new ProcessResponse();
            OTPTransactions rs = new OTPTransactions();

            rs = context.oTPTransactions.Where(a => a.UserId == userid && (a.EmailOTP == emailOtp || a.MobileOTP == emailOtp) && a.CurrentStatus.Equals("Draft")).FirstOrDefault();
            if (rs != null)
            {
                rs.UsedOn = DateTime.Now;
                rs.CurrentStatus = "Used";
                context.Entry(rs).CurrentValues.SetValues(rs);
                context.SaveChanges();
                ps.statusCode = 1;
                ps.statusMessage = "Success";
                ps.currentId = Convert.ToInt32(rs.UserId);
                List<OTPTransactions> extraOts = context.oTPTransactions.Where(a => a.UserId == userid && a.CurrentStatus.Equals("Draft")).ToList();
                if (extraOts != null)
                {
                    foreach(OTPTransactions o in extraOts)
                    {
                        o.UsedOn = DateTime.Now;
                        o.CurrentStatus = "Extras";
                        context.Entry(o).CurrentValues.SetValues(o);
                        context.SaveChanges();
                    }
                }

                // update password
                Users um = new Users();
                um = context.users.Where(a => a.UserId == userid).FirstOrDefault();
                um.PWord = PasswordEncryption.Encrypt(pword);
                context.Entry(um).CurrentValues.SetValues(um);
                context.SaveChanges();
            }
            else
            {
                ps.statusCode = 0;
                ps.statusMessage = "Invalid OTP or OTP Expired";
            }
            return ps;
        }
        public string GetEmailOtp(int id)
        {
            string o = "";
            try
            {
                o = context.oTPTransactions.Where(a => a.IsDeleted == false && a.UserId == id && a.CurrentStatus == "Draft").Select(b => b.MobileOTP).FirstOrDefault();
            }catch(Exception e)
            {

            }
            return o;
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
