using Newtonsoft.Json;

namespace School.Helpers.ZarinPal
{
	public static class ZarinPalPayment
	{
		private const int AuthorityLength = 36;

		private const bool IsSandbox = true;

		public static PayResponse Request(string MerchantId,long amount, string description, string callbackUrl)
		{
			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			var httpCore = new HttpCore(Urls.GetPaymentRequestUrl(IsSandbox), "POST",
				JsonConvert.SerializeObject(new PayRequest(MerchantId, amount, description, callbackUrl)));
			var res = JsonConvert.DeserializeObject<PayResponse>(httpCore.GetResponse());
			res.Authority = res.Authority.TrimStart('0');
			return res;
		}

		public static PayVerifyResponse Verify(string MerchantId,long amount, string authority)
		{
			var z = "";
			var count = AuthorityLength - authority.Length;
			for (var i = 0; i < count; i++) z += "0";
			authority = z + authority;

			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			var httpCore = new HttpCore(Urls.GetVerificationUrl(IsSandbox), "POST",
				JsonConvert.SerializeObject(new PayVerify(MerchantId, amount, authority)));
			return JsonConvert.DeserializeObject<PayVerifyResponse>(httpCore.GetResponse());
		}

		public static string GetPaymentGatewayUrl(string authority)
		{
			//bool sandBoxMode = merchantID.Equals(TestMerchantID);
			return Urls.GetPaymentGatewayUrl(authority, IsSandbox);
		}
	}
}