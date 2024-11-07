namespace Restaurants.API.Middlewares
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next.Invoke(context);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, ex.Message);
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsync("Something went wrong");
			}
        }
    }
}
