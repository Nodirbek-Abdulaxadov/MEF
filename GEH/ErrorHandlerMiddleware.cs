using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Text.Json;
using ZteBillingGateway.Utility.Exceptions.models;
using ZteBillingGateway.Utility.Localize;

namespace ZteBillingGateway.Utility.Exceptions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAlertaGram _telegramAlert;
        private IStringLocalizer<Resource> _stringLocalizer;
        private HttpResponseException httpResponseException { get; set; }
        public ILogger<ErrorHandlerMiddleware> Logger { get; }

        public ErrorHandlerMiddleware(RequestDelegate next,
                                      ILogger<ErrorHandlerMiddleware> logger,
                                      IAlertaGram telegramAlert, IStringLocalizer<Resource> stringLocalizer)
        {
            _next = next;
            Logger = logger;
            _telegramAlert = telegramAlert;
            _stringLocalizer = stringLocalizer;
        }
        public async Task Invoke(HttpContext context)
        {
            var activity = Activity.Current;
            try
            {
                await _next(context);
            }
            catch (CustomServiceException ex)
            {
                string methodName = ex.MethodName!;
                var response = context.Response;
                response.ContentType = "application/json";
                switch (ex.Message)
                {
                    case "webhook-ident-failed":
                        httpResponseException = new HttpResponseException(status: 404, internalError: ex.InnerException,
                        message: _stringLocalizer[ex.Message], code: ex.Message, name: "Bad Request");
                        break;
                    default:
                        httpResponseException = new HttpResponseException(status: 400, internalError: ex.InnerException,
                        message: _stringLocalizer[ex.Message], code: ex.Message, name: "Bad Request");
                        break;

                }


                //await _telegramAlert.NotifyErrorAsync(this.httpResponseException.message!, methodName);
                var result = JsonSerializer.Serialize(this.httpResponseException);
                response.StatusCode = (int)this.httpResponseException.status!;
                activity.RecordException(ex);
                Logger.LogError(ex, ex.Message);
                await response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                string methodName = "UnhandledException";
                var response = context.Response;
                response.ContentType = "application/json";

                activity.RecordException(ex);
                httpResponseException = new HttpResponseException(status: 500, internalError: ex.InnerException == null ? null : ex.InnerException.ToString(),
                        message: ex.Message, code: ex.Message, name: "Internal Server Error");
                var result = JsonSerializer.Serialize(this.httpResponseException);
                response.StatusCode = 500;
                await _telegramAlert.NotifyAsync(result.ToString(), methodName);
                Logger.LogError(ex, ex.Message);
                await response.WriteAsync(result);
            }
        }

    }
}
