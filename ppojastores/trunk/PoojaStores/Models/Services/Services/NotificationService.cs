using Microsoft.Extensions.Configuration;
using PoojaStores.Models.ModelClasses;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Services.Interfaces;
using PoojaStores.Models.Utilities;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PoojaStores.Models.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly MyDbContext context;
        private readonly IConfiguration _config;

        public NotificationService(IConfiguration config, MyDbContext context)
        {
            _config = config;
            this.context = context;
        }

        public ProcessResponse SendProductDeleteEmail(string moduleName, string productDetails,
            string deletedBy, DateTime deletedon)
        {
            ProcessResponse ps = new ProcessResponse();
            try
            {
                EmailTemplates emailTemplate = new EmailTemplates();
                emailTemplate = GetEmailTemplateByModule(moduleName);
                if (emailTemplate != null)
                {
                    string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");
                    string adminname1 = _config.GetValue<string>("EmailConfig:adminPersonName");
                    string adminemail = _config.GetValue<string>("EmailConfig:adminEmail");
                    string emailText = emailTemplate.Subject;
                    string rd = deletedon.ToString("D");
                    emailText = emailText.Replace("##ADMINNAME##", adminname1);
                    emailText = emailText.Replace("##STAFFNAME##", deletedBy);
                    emailText = emailText.Replace("##PRODUCTDETAILS##", productDetails);
                    emailText = emailText.Replace("##DDATE##", rd);
                    bool res = PushEmail(emailText, adminemail, emailTemplate.Subject, emailCC);
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            catch (Exception ex)
            {
                ps.statusMessage = ex.Message;
                ps.statusCode = 0;
            }
            return ps;
        }
        
        public bool PushEmail(string emailtext, string to, string subject, string cc = "")
        {
            bool res = false;
            try
            {
                string emailimagesurl = _config.GetValue<string>("EmailConfig:EMAILIMAGEURL");
                string smtpserver = _config.GetValue<string>("EmailConfig:smtpServer");
                string smtpUsername = _config.GetValue<string>("EmailConfig:smtpEmail");
                string smtpPassword = _config.GetValue<string>("EmailConfig:smtppassword");
                int smtpPort = _config.GetValue<int>("EmailConfig:portNumber");

                emailtext = emailtext.Replace("##EMAILIMAGES##", emailimagesurl);
                MailMessage msg = new MailMessage(smtpUsername, to, subject, emailtext);

                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                mail.From = new MailAddress(smtpUsername);
                mail.Subject = subject;
                mail.Body = emailtext;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = smtpserver;
                smtp.Port = smtpPort;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(smtpUsername, smtpPassword);
                smtp.EnableSsl = true;

                try
                {
                    smtp.Send(mail);
                    res = true;
                }
                catch (Exception ex)
                {
                    res = false;
                }
            }
            catch (Exception ex)
            {
                // LogException.Record(ex);
                res = false;
            }
            return res;
        }

        public bool SentTextSms(string phoneNumber, string message, string countrycode="91")
        {
            bool res = false;
            try
            {
                string baseurl = _config.GetValue<string>("SMSConfig:smsBaseUrl");
                string authkey = _config.GetValue<string>("SMSConfig:smsAuthKey");
                string senderId = _config.GetValue<string>("SMSConfig:smsSenderId");

                string url = string.Empty;
                if (countrycode != "91")
                {
                    url = baseurl + "auth=" + authkey + "&msisdn=" + phoneNumber + "&countrycode=" + countrycode + "&senderid=" + senderId + "&message=" + message;
                }
                else
                {
                    url = baseurl + "auth=" + authkey + "&msisdn=" + phoneNumber + "&senderid=" + senderId + "&message=" + message;
                }

                //WebClient client = new WebClient();
                //Stream data = client.OpenRead(baseurl);
                //StreamReader reader = new StreamReader(data);
                //string s = reader.ReadToEnd();
                //data.Close();
                //reader.Close();

                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);
                res = true;
            }
            catch (Exception ex)
            {
                // LogException.Record(ex);
                res = false;
            }

            return res;
        }

        public EmailTemplates GetEmailTemplateByModule(string modulename)
        {
            EmailTemplates response = new EmailTemplates();
            response = context.emailTemplates.Where(a => a.ModuleName == modulename).FirstOrDefault();
            return response;
        }
        public ProcessResponse SendLoginCredentials(string moduleName, string toEamil, string userName,string password)
        {
            ProcessResponse ps = new ProcessResponse();
            EmailTemplates template = new EmailTemplates();
            template = GetEmailTemplateByModule(moduleName);
            if (template != null)
            {
                string HostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
                string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");

                string emailText = template.EmailTemplate;
                emailText = emailText.Replace("##UserName##", userName);
                emailText = emailText.Replace("##PASSWORD##", " :  " + password);
                bool res = PushEmail(emailText, toEamil, template.Subject, emailCC);
                if (res == false)
                {
                    ps.statusMessage = "failed to send email";
                    ps.statusCode = 0;
                }
                else
                {
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            return ps;
        }
        public ProcessResponse SendOrderReceived(string pno, string OrderId)
        {
            ProcessResponse pr = new ProcessResponse();
            string msg = AppSettings.SMSTemplates.OrderReceipt;
            msg = msg.Replace("{#var#}", OrderId);
            bool res = SentTextSms(pno, msg);
            if (res == false)
            {
                pr.statusMessage = "failed to send SMS";
                pr.statusCode = 0;
            }
            else
            {
                pr.statusMessage = "email SMS";
                pr.statusCode = 1;
            }
            return pr;
        }
        public ProcessResponse SendResetPasswordMobile(string pno, string OrderId,string userName)
        {
            ProcessResponse pr = new ProcessResponse();
            string msg = AppSettings.SMSTemplates.ForgetPaasswordOtp;
            msg = msg.Replace("{#var#}", OrderId);
            //msg = msg.Replace("{#UserName#}", userName);
            bool res = SentTextSms(pno, msg);
            if (res == false)
            {
                pr.statusMessage = "failed to send SMS";
                pr.statusCode = 0;
            }
            else
            {
                pr.statusMessage = "email SMS";
                pr.statusCode = 1;
            }
            return pr;
        }
        public ProcessResponse SendDispatchOrderMobile(string pno, string OrderId, string dispatch)
        {
            ProcessResponse pr = new ProcessResponse();
            string msg = "Your order No."+ OrderId + " dispatched through "+dispatch+" .PPEXIM";
            bool res = SentTextSms(pno, msg);
            if (res == false)
            {
                pr.statusMessage = "failed to send SMS";
                pr.statusCode = 0;
            }
            else
            {
                pr.statusMessage = "email SMS";
                pr.statusCode = 1;
            }
            return pr;
        }
        public ProcessResponse SendDelivertoMobile(string pno, string OrderId)
        {
            ProcessResponse pr = new ProcessResponse();
            string msg = "Your order No."+OrderId+" delivered today. PPEXIM";

            bool res = SentTextSms(pno, msg);
            if (res == false)
            {
                pr.statusMessage = "failed to send SMS";
                pr.statusCode = 0;
            }
            else
            {
                pr.statusMessage = "email SMS";
                pr.statusCode = 1;
            }
            return pr;
        }
        public ProcessResponse SendCanceltoMobile(string pno, string OrderId)
        {
            ProcessResponse pr = new ProcessResponse();
            //string msg = "Your order No." + OrderId + " delivered today. PPEXIM";
            string msg = "Your order No." + OrderId + " canceled. PPEXIM";

            bool res = SentTextSms(pno, msg);
            if (res == false)
            {
                pr.statusMessage = "failed to send SMS";
                pr.statusCode = 0;
            }
            else
            {
                pr.statusMessage = "email SMS";
                pr.statusCode = 1;
            }
            return pr;
        }
        public ProcessResponse SendReturntoMobile(string pno, string OrderId)
        {
            ProcessResponse pr = new ProcessResponse();
            //string msg = "Your order No." + OrderId + " delivered today. PPEXIM";
            string msg = "Your order No." + OrderId + " returned today. PPEXIM";

            bool res = SentTextSms(pno, msg);
            if (res == false)
            {
                pr.statusMessage = "failed to send SMS";
                pr.statusCode = 0;
            }
            else
            {
                pr.statusMessage = "email SMS";
                pr.statusCode = 1;
            }
            return pr;
        }
        public ProcessResponse SendForgetPasswordRequest(string moduleName, string toEamil, string userName, string password)
        {
            ProcessResponse ps = new ProcessResponse();
            EmailTemplates template = new EmailTemplates();
            template = GetEmailTemplateByModule(moduleName);
            if (template != null)
            {
                string HostURL = _config.GetValue<string>("OtherConfig:WebHostURL");
                string emailCC = _config.GetValue<string>("OtherConfig:EmailCC");

                string emailText = template.EmailTemplate;
                emailText = emailText.Replace("##UserName##", userName);
                emailText = emailText.Replace("##OTP##", " :  " + password);
                bool res = PushEmail(emailText, toEamil, template.Subject, emailCC);
                if (res == false)
                {
                    ps.statusMessage = "failed to send email";
                    ps.statusCode = 0;
                }
                else
                {
                    ps.statusMessage = "email sent";
                    ps.statusCode = 1;
                }
            }
            return ps;
        }
    }
}
