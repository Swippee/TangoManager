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
                    case EntityDoesNotExistException ex:
                        httpStatusCode = HttpStatusCode.NotFound;
                        break;
                    case QuizAlreadyFinishedException ex:
                        httpStatusCode = HttpStatusCode.InternalServerError;
                        break;
                    case QuizInvalidStateException ex:
                        httpStatusCode = HttpStatusCode.BadRequest;
                        break;
                    case InvalidPacketTokenHeaderException ex:
                        httpStatusCode = HttpStatusCode.BadRequest;
                        break;
                    case InvalidPacketTokenException ex:
                        httpStatusCode = HttpStatusCode.Unauthorized;
                        break;
                    default:
                        httpStatusCode = HttpStatusCode.InternalServerError;
                        break;

                }

                context.Response.StatusCode = (int)httpStatusCode;
                await context.Response.WriteAsJsonAsync(new
                {
                    messageFull = exception.ToString(),
                    messageShort = exception.Message
                });
                await context.Response.CompleteAsync();
            }
        }
    }
}
