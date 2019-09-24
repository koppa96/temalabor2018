using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Czeum.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            if (e is ArgumentOutOfRangeException)
            {
                context.Response.StatusCode = 404;
                return context.Response.WriteJsonAsync(new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = e.Message
                });
            }

            if (e is ArgumentException || e is InvalidOperationException)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteJsonAsync(new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = e.Message
                });
            }

            if (e is UnauthorizedAccessException)
            {
                context.Response.StatusCode = 401;
                return context.Response.WriteJsonAsync(new ProblemDetails
                {
                    Status = 401,
                    Title = "Unauthorized",
                    Detail = e.Message
                });
            }

            context.Response.StatusCode = 500;
            return context.Response.WriteJsonAsync(new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = e.Message
            });
        }
    }
}