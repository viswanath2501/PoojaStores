using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Repositories.Repositories.Interfaces
{
    public interface IPaymentRepo
    {
        ProcessResponse SavePoFollow(PoFollowUp request);
        List<PoFollowUp> GetAllPoFollowUp();
        PoFollowUp GetPoFollowUpById(int id);
        ProcessResponse UpdatePoFolloeUp(PoFollowUp request);

        ProcessResponse SaveRazorPayment(RazorPaymentResult request);
        List<RazorPaymentResult> GetAllRazorPayResult();
        RazorPaymentResult GetRazorResultById(int id);
        ProcessResponse UpdateRazorResult(RazorPaymentResult request);
        void LogError(Exception ex);
    }
}
