
namespace Restaurants.API.Middleware
{
    public class ExecutionTimeMiddleware(ILogger<ExecutionTimeMiddleware> logger) : IMiddleware
    {
            
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            long startMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            await next.Invoke(context);
            long endMillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            long millisTaken = endMillis - startMillis;
            if (millisTaken >= 4000)
            {
                logger.LogInformation("New {RequestType} Request ({RequestPath}) took {TimeTaken} ms", context.Request.Method, context.Request.Path, millisTaken);

            }

            
        }
    }
}
