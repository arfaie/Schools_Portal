using School.Models.Helpers.OptionEnums;
using School.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using School.Helpers.OptionEnums;

namespace School.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        private const string BaseUrl = "https://api.kavenegar.com/";

        public async Task<bool> SendSmsAsync(string mobile, SmsTypes type, string token, string token2 = null,string token3=null)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = null;

            if (type == SmsTypes.Register)
            {
                response = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token10=" + token2 + "&template=SchoolPortalRegister");
            }
            else if (type == SmsTypes.ResetPass)
            {
                response = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&template=SahadResetPass");
            }
            else if (type == SmsTypes.SchoolPaymentDone)
            {
                response = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token10=" + token2 + "&token20=" + token3 + "&template=SchoolPaymentDone");
            }
            else if (type == SmsTypes.SchoolPreSubmitDone)
            {
                response = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token10=" + token2 + "&token20=" + token3 + "&template=SchoolPreSubmitDone");
            }
            else if (type == SmsTypes.SchoolRegisterDone)
            {
                response = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token10=" + token2 + "&token20=" + token3 + "&template=SchoolRegisterDone");
            }
            if (response != null && response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                try
                {
                    var datalist = JsonConvert.DeserializeObject<SMSViewModel.RootObject>(result);
                }
                catch (Exception e)
                {
                    throw;
                }
                return true;
            }

            return false;
        }
    }
}