﻿using School.Models.Helpers.OptionEnums;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using School.Helpers.OptionEnums;

namespace School.Helpers
{
	public class SendSms
	{
		private string _baseurl = "https://api.kavenegar.com/";

		public async Task<bool> SendAsync(short type, string mobile, string token, string token2 = null)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_baseurl);

				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage res = null;

				if (type == (int)SmsTypes.Register)
				{
					res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&%token10=" + token2 + "&template=SchoolPortalRegister");
				}
				if (type == (int)SmsTypes.StudentRegister)
				{
					res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&token=" + token + "&token2=" + token2 + "&template=CarbioticReset");
				}
                if (type == (int)SmsTypes.DoneOrder)
                {
                    res = await client.GetAsync("v1/6E65695778564344644231365673544D73794E324C7377634149324F7A5270324F6346774D6753666363633D/verify/lookup.json?receptor=" + mobile + "&%token10=" + token + "&token =" + token2 + "&template=DoneOrder");
                }
                if (res != null && res.IsSuccessStatusCode)
				{
					var result = res.Content.ReadAsStringAsync().Result;

					try
					{
						//var datalist = JsonConvert.DeserializeObject<SmsViewModel.RootObject>(result);
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
}