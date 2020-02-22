using System;
using Virgo.Application.Interfaces;
using Virgo.Application.Models.Requests;
using Virgo.Application.Models.Responses;
using Virgo.DependencyInjection;
using Virgo.Domain.Interfaces;

namespace Virgo.Application.Services
{
    /// <summary>
    /// <see cref="ICustomService"/>服务实现类
    /// </summary>
    public class CustomService : ICustomService, ITransientDependency
    {
        /// <summary>
        /// <see cref="IRepository"/>仓储实例
        /// </summary>
        private readonly IRepository _repository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public CustomService(IRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CostomResponse Call(CustomRequest request)
        {
            System.Diagnostics.Debug.WriteLine("CustomService Calling");
            _repository.Call();
            return new CostomResponse()
            {
                Id = request.Id,
                Date = DateTime.Now
            };
        }
    }
}
