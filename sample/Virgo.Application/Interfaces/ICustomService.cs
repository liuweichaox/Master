using Virgo.Application.Models.Requests;
using Virgo.Application.Models.Responses;

namespace Virgo.Application.Interfaces
{
    /// <summary>
    /// 自定义服务接口
    /// </summary>
    public interface ICustomService
    {
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <returns></returns>
        CostomResponse Call(CustomRequest request);
    }
}
