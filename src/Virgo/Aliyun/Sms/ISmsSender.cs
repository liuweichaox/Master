using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System.Threading.Tasks;

namespace Virgo.Aliyun.Sms
{
    /// <summary>
    /// 发送短信的接口
    /// </summary>
    public interface ISmsSender
    {
        /// <summary>
        /// 短信发送服务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SendSmsResponse> SendSmsAsync(SendSmsRequest request);
    }
}
