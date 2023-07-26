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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo pRepo;

        public PaymentService(IPaymentRepo _pRepo)
        {
            pRepo = _pRepo;
        }

        public ProcessResponse SavePoFollowUp(PoFollowUp request)
        {
            ProcessResponse pr = new ProcessResponse();
            if(request.FollowUpId>0)
            {
              pr=  pRepo.UpdatePoFolloeUp(request);
            }
            else
            {
               pr= pRepo.SavePoFollow(request);
            }
            return pr;

        }
        public List<PoFollowUp> GetAllPoFollowUp()
        {
            return pRepo.GetAllPoFollowUp();
        }
        public PoFollowUp GetFollowUpById(int id)
        {
            return pRepo.GetPoFollowUpById(id);
        }

        public ProcessResponse SaveRazorresult(RazorPaymentResult request)
        {
            ProcessResponse pr = new ProcessResponse();
            if (request.TRID > 0)
            {
                pr = pRepo.UpdateRazorResult(request);
            }
            else
            {
                pr = pRepo.SaveRazorPayment(request);
            }
            return pr;

        }
        public List<RazorPaymentResult> GetRazorResult()
        {
            return pRepo.GetAllRazorPayResult();
        }
        public RazorPaymentResult GetRazorResultById(int id)
        {
            return pRepo.GetRazorResultById(id);
        }

    }
}
