using System.Net;

namespace Salaries.Infrastructure.Exceptions;

public class ExceptionResponse
{
    public object Response { get; }

    public HttpStatusCode StatusCode { get; }

    public ExceptionResponse(object response, HttpStatusCode statusCode)
    {
        this.Response = response;
        this.StatusCode = statusCode;
    }
}