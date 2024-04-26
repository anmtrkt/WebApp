namespace WebApp.Middlewares
{
    public class StartMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public StartMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Пришел запрос от " + context.Request);

            await _next(context);
        }
    }
}
