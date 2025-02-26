using System.Net;

namespace ReceivablesFactoring.WebApi.Infrastructure;

public record ExceptionResponse (HttpStatusCode StatusCode, string Message, string[] Errors);
