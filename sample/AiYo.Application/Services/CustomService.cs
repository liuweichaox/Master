using System;
using Virgo.Application.Interfaces;
using Virgo.Application.Models.Requests;
using Virgo.Application.Models.Responses;
using Virgo.DependencyInjection;
using Virgo.Domain.Interfaces;
using Virgo.Domain.Models;
using Virgo.Extensions;

namespace Virgo.Application.Services
{
    /// <summary>
    /// <see cref="ICustomService"/>服务实现类
    /// </summary>
    public class CustomService : ICustomService, ITransientDependency
    {
        /// <summary>
        /// <see cref="ICustomRepository"/>仓储实例
        /// </summary>
        private readonly ICustomRepository _customRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customRepository"></param>
        public CustomService(ICustomRepository customRepository)
        {
            _customRepository = customRepository;
        }
        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CostomResponse Call(CustomRequest request)
        {
            System.Diagnostics.Debug.WriteLine("CustomService Calling");
            var customEntity = request.MapTo<CustomRequest,CustomEntity>();
            _customRepository.Call(customEntity);
            return new CostomResponse()
            {
                Id = request.Id,
                Date = DateTime.Now
            };
        }
    }
}
