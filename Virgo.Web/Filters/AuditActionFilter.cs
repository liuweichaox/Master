using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Virgo.Extensions;

namespace Virgo.Web.Filters
{
    public class AuditActionFilter : IAsyncActionFilter
    {
        public AuditActionFilter()
        {
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            try
            {
                await next();
            }
            catch (Exception)
            {
                context.Result = new BadRequestResult();
            }
            stopwatch.Stop();
            var audit = new
            {
                Parameters = context.ActionArguments.Serialize(),
                Url = context.HttpContext.Request.GetAbsoluteUri(),
                Headers = context.HttpContext.Request.Headers.Serialize()
            };
            Debug.WriteLine($"审计日志：{Environment.NewLine}{audit.Serialize()}");
        }
    }
}
