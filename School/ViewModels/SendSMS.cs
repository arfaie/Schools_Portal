using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using School.ViewModels;
using School.Helpers.OptionEnums;


namespace Ecommerce.Helpers
{
    public class SendSMS
    {
        private string Baseurl = "https://api.kavenegar.com/";

        public async Task<bool> SendAsync(short Type, string Mobile, string token, string token2 = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = null;

                if (Type == (int)SmsTypes.Register)
                {
                    Res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + Mobile + "&token=" + token + "&token2=" + "Carbiotic.ir" + "&template=CarbioticRegister");
                }
                if (Type == (int)SmsTypes.ResendUserPass)
                {
                    Res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + Mobile + "&token=" + token + "&token2=" + token2 + "&template=CarbioticReset");
                }
                if (Res.IsSuccessStatusCode && Res != null)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    try
                    {
                        SMSViewModel.RootObject datalist = JsonConvert.DeserializeObject<SMSViewModel.RootObject>(Response);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}