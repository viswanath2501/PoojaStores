using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Repos
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly MyDbContext context;

        public PaymentRepo(MyDbContext _context)
        {
            this.context = _context;
        }

        public ProcessResponse SavePoFollow(PoFollowUp request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.isDeleted = false;
                context.poFollowUps.Add(request);
                context.SaveChanges();
                response.currentId = request.FollowUpId;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";
               
            }
            return response;
        }
        public List<PoFollowUp> GetAllPoFollowUp()
        {
            List<PoFollowUp> response = new List<PoFollowUp>();
            try
            {
                response = context.poFollowUps.Where(a => a.isDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public PoFollowUp GetPoFollowUpById(int id)
        {
            PoFollowUp response = new PoFollowUp();
            try
            {
                response = context.poFollowUps.Where(a => a.isDeleted == false && a.FollowUpId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdatePoFolloeUp(PoFollowUp request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                PoFollowUp po = context.poFollowUps.Where(a => a.FollowUpId == request.FollowUpId).FirstOrDefault();
                context.Entry(po).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.FollowUpId;
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

        public ProcessResponse SaveRazorPayment(RazorPaymentResult request)
        {

            ProcessResponse response = new ProcessResponse();
            try
            {
                request.IsDeleted = false;
                context.razorPaymentResults.Add(request);
                context.SaveChanges();
                response.currentId = request.TRID;
                response.statusCode = 1;
                response.statusMessage = "Success";

            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.statusMessage = "failed";

            }
            return response;
        }
        public List<RazorPaymentResult> GetAllRazorPayResult()
        {
            List<RazorPaymentResult> response = new List<RazorPaymentResult>();
            try
            {
                response = context.razorPaymentResults.Where(a => a.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public RazorPaymentResult GetRazorResultById(int id)
        {
            RazorPaymentResult response = new RazorPaymentResult();
            try
            {
                response = context.razorPaymentResults.Where(a => a.IsDeleted == false && a.TRID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return response;
        }
        public ProcessResponse UpdateRazorResult(RazorPaymentResult request)
        {
            ProcessResponse response = new ProcessResponse();
            try
            {
                RazorPaymentResult rp = context.razorPaymentResults.Where(a => a.TRID == request.TRID).FirstOrDefault();
                context.Entry(rp).CurrentValues.SetValues(request);
                context.SaveChanges();
                response.currentId = request.TRID;
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
