using Restaurants.Domain.Exceptions;

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
			catch(NotFoundException notFound)
			{
				logger.LogWarning(notFound.Message);

				context.Response.StatusCode = StatusCodes.Status404NotFound;
				await context.Response.WriteAsync(notFound.Message);
			}
            catch (FavoriteNotFoundException notFound)
            {
                logger.LogWarning(notFound.Message);

                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(notFound.Message);
            }
            catch (FavoriteAlreadyExistsException alreadyExistsException)
            {
                logger.LogWarning(alreadyExistsException.Message);

                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                await context.Response.WriteAsync(alreadyExistsException.Message);
            }
            catch (ForbidException)
			{
				context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access forbidden");
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
