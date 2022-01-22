using GringottsBank.Entities.DTO.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GringottsBank.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(ApiResponse<NoContent>.Fail(500, ex.Message));
            }
        }
        public bool TryFindInnerException<T>(Exception top, out T foundException) where T : Exception
        {
            if (top == null)
            {
                foundException = null;
                return false;
            }

            Console.WriteLine(top.GetType());
            if (typeof(T) == top.GetType())
            {
                foundException = (T)top;
                return true;
            }

            return TryFindInnerException<T>(top.InnerException, out foundException);
        }
    }
}
