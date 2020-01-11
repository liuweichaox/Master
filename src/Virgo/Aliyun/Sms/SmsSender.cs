using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System;
using System.Threading.Tasks;
using Virgo.DependencyInjection;

namespace Virgo.Aliyun.Sms
{
    /// <summary>
    /// <see cref="ISmsSender"/>实现类
    /// </summary>
    public class SmsSender : ISmsSender, ISingletonDependency
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SendSmsResponse> SendSmsAsync(SendSmsRequest request)
        {
            var verifiedCode = new Random().Next(100000, 999999).ToString();
            string accessKeyId = "";//你的accessKeyId，此处需要替换成开发者自己的AK(在阿里云访问控制台寻找)
            string accessKeySecret = "";//你的accessKeySecret，此处需要替换成开发者自己的AK(在阿里云访问控制台寻找)
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsResponse response = null;
            await Task.Run(() =>
            {
                try
                {
                    //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”
                    request.PhoneNumbers = "18771506573";
                    //必填:短信签名-可在短信控制台中找到
                    request.SignName = "凡迹网";
                    //必填:短信模板-可在短信控制台中找到，发送国际/港澳台消息时，请使用国际/港澳台短信模版
                    request.TemplateCode = "SMS_107090067";
                    //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                    request.TemplateParam = "{\"code\":\"dalao\"}";
                    //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                    request.OutId = "yourOutId";
                    //请求失败这里会抛ClientException异常
                    response = acsClient.GetAcsResponse(request);
                }
                catch (ServerException e)
                {
                    throw e;
                }
                catch (ClientException e)
                {
                    throw e;
                }
            });
            return response;
        }
    }
}
