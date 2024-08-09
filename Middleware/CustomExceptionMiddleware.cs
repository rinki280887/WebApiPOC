using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApiPOC.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            this.next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Calling next middleware in pipeline
                await next(context).ConfigureAwait(false);
            }
            catch (Exception exceptionObj)
            {
                await HandleExceptionAsync(context, exceptionObj).ConfigureAwait(false);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Logging the exception first
            _logger.LogError(exception.Message);

            // TODO: Handle more specific type exceptions

            // TODO: Save exception in DB or TEXT file based on configuration

            // Generic Handling
            string result = new { Message = exception.Message, StatusCode = (int)HttpStatusCode.InternalServerError }.ToString();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}
