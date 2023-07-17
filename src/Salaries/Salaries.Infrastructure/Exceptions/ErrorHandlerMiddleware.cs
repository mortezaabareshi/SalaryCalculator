using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Open.Serialization.Json;

namespace Salaries.Infrastructure.Exceptions;

internal sealed class ErrorHandlerMiddleware : IMiddleware
{
  private readonly IExceptionToResponseMapper _exceptionToResponseMapper;
  private readonly IJsonSerializer _jsonSerializer;
  private readonly ILogger<ErrorHandlerMiddleware> _logger;

  public ErrorHandlerMiddleware(
    IExceptionToResponseMapper exceptionToResponseMapper,
    IJsonSerializer jsonSerializer,
    ILogger<ErrorHandlerMiddleware> logger)
  {
    _exceptionToResponseMapper = exceptionToResponseMapper;
    _jsonSerializer = jsonSerializer;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    try
    {
      await next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, ex.Message);
      await HandleErrorAsync(context, ex);
    }
  }

  private async Task HandleErrorAsync(HttpContext context, Exception exception)
  {
    ExceptionResponse exceptionResponse = _exceptionToResponseMapper.Map(exception);
    context.Response.StatusCode = exceptionResponse != null ? (int) exceptionResponse.StatusCode : 400;
    if (exceptionResponse?.Response == null)
    {
      await context.Response.WriteAsync(string.Empty);
    }
    else
    {
      context.Response.ContentType = "application/json";
      await _jsonSerializer.SerializeAsync(context.Response.Body, exceptionResponse.Response);
    }
  }
}