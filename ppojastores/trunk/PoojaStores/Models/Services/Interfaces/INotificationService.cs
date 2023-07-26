using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Interfaces
{
    public interface INotificationService
    {
        ProcessResponse SendProductDeleteEmail(string moduleName, string productDetails,
            string deletedBy, DateTime deletedon);
        bool PushEmail(string emailtext, string to, string subject, string cc = "");
        bool SentTextSms(string phoneNumber, string message, string countrycode);
        EmailTemplates GetEmailTemplateByModule(string modulename);
        ProcessResponse SendLoginCredentials(string moduleName, string toEamil, string userName, string password);
        ProcessResponse SendOrderReceived(string pno, string OrderId);
        ProcessResponse SendResetPasswordMobile(string pno, string OrderId, string userName);
        ProcessResponse SendDispatchOrderMobile(string pno, string OrderId, string dispatch);
        ProcessResponse SendDelivertoMobile(string pno, string OrderId);
        ProcessResponse SendCanceltoMobile(string pno, string OrderId);
        ProcessResponse SendReturntoMobile(string pno, string OrderId);
        ProcessResponse SendForgetPasswordRequest(string moduleName, string toEamil, string userName, string password);
    }
}
