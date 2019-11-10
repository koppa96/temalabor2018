using System;
using System.Threading.Tasks;
using Czeum.Core.Exceptions;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Czeum.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            if (e is NotFoundException)
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