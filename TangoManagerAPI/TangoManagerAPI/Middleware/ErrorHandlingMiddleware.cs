using System.Net;
using TangoManagerAPI.Entities.Exceptions;

namespace TangoManagerAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware 
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                HttpStatusCode httpStatusCode;
                switch (exception)
                {
                    case CardAlreadyExistsInPacketException ex:
                        httpStatusCode = HttpStatusCode.Conflict;
                        break;
                    case CardNotFoundInPacketException ex:
                        httpStatusCode = HttpStatusCode.NotFound;
                        break;
                    case EmptyPaquetException ex:
                        httpStatusCode = HttpStatusCode.InternalServerError;
                        break;
                    case EntityAlreadyExistsException ex:
                        httpStatusCode = HttpStatusCode.Conflict;
                        break;
                    case EntityDoNotExistException ex:
                        httpStatusCode = HttpStatusCode.NotFound;
                        break;
                    case QuizAlreadyFinishedException ex:
                        httpStatusCode = HttpStatusCode.InternalServerError;
                        break;
                    case QuizInvalidStateException ex:
                        httpStatusCode = HttpStatusCode.BadRequest;
                        break;
                    default:
                        httpStatusCode = HttpStatusCode.InternalServerError; 
                        break;

                }

                context.Response.StatusCode = (int)httpStatusCode;
                await context.Response.WriteAsync(exception.Message);
            }
        }
    }
}
