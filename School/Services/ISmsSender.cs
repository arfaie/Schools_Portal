using School.Models.Helpers.OptionEnums;
using System.Threading.Tasks;

namespace School.Services
{
	public interface ISmsSender
	{
		Task<bool> SendSmsAsync(string mobile, SmsTypes type, string token, string token2 = null);
	}
}