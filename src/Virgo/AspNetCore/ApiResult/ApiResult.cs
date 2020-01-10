using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.AspNetCore
{
    /// <summary>
    /// Api返回类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        public ApiResult(T data, ApiStatus status, string message)
        {
            Data = data;
            Code = status;
            Message = message;
        }
        public T Data { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public ApiStatus Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }

    public class ApiResult : ApiResult<string>
    {
        public ApiResult(string data, ApiStatus status, string message) : base(data, status, message)
        {
        }
    }
}
