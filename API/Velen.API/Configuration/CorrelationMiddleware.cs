namespace Velen.API.Configuration
{
    internal class CorrelationMiddleware:IMiddleware
    {
        internal const string CorrelationHeaderKey = "CorrelationId";
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var correlationId = Guid.NewGuid();

            if (context.Request != null)
            {
                context.Request.Headers.Add(CorrelationHeaderKey, correlationId.ToString());
            }

            await next.Invoke(context);
        }
    }
}