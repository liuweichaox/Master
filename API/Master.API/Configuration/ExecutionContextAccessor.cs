namespace Master.API.Configuration
{
    /// <summary>
    /// ExecutionContextAccessor
    /// </summary>
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// ExecutionContextAccessor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// CorrelationId
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        public Guid CorrelationId
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && IsAvailable && _httpContextAccessor.HttpContext.Request.Headers.Keys.Any(x => x == CorrelationMiddleware.CorrelationHeaderKey))
                {
                    return Guid.Parse(
                        _httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]);
                }
                throw new ApplicationException("Http context and correlation id is not available");
            }
        }

        public bool IsAvailable => _httpContextAccessor.HttpContext != null;
    }
}