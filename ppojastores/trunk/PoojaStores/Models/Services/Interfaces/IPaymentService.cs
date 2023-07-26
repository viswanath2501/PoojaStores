using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
   public interface IPaymentService
    {
        ProcessResponse SavePoFollowUp(PoFollowUp request);
        List<PoFollowUp> GetAllPoFollowUp();
        PoFollowUp GetFollowUpById(int id);

        ProcessResponse SaveRazorresult(RazorPaymentResult request);
        List<RazorPaymentResult> GetRazorResult();
        RazorPaymentResult GetRazorResultById(int id);

    }
}
