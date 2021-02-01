using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Students.Common.Exceptions;

namespace Students.Api.Middlewares
{
  public class ErrorHandlingMiddleware
  {
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      int statusCode;
      switch (exception)
      {
        case NotFoundException _:
          statusCode = (int)HttpStatusCode.NotFound;
          break;
        case InvalidArgumentException _:
          statusCode = (int)HttpStatusCode.BadRequest;
          break;
        case DuplicateGroupNameException _:
          statusCode = (int)HttpStatusCode.BadRequest;
          break;
        case InvalidOperationException _:
          statusCode = (int)HttpStatusCode.BadRequest;
          break;
        default:
          statusCode = (int)HttpStatusCode.InternalServerError;
          break;
      }

      var result = JsonConvert.SerializeObject(new { error = exception.Message });

      context.Response.ContentType = "application/json";
      context.Response.StatusCode = statusCode;
      return context.Response.WriteAsync(result);
    }
  }
}
