using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using HCode.Domain;
using HCode.Application;

namespace Hcode.Api
{
    /// <summary>
    /// Middleware xử lý request, response
    /// </summary>
    /// Created by: nlnhat (12/07/2023)
    public class HandleMiddleware : IMiddleware
    {
        #region Fields
        /// <summary>
        /// Tài nguyên ngôn ngữ
        /// </summary>
        public IStringLocalizer<Resource> _resource;
        #endregion

        #region Constructors
        /// <summary>
        /// Inject dịch vụ logger
        /// </summary>
        public HandleMiddleware(IStringLocalizer<Resource> resource)
        {
            _resource = resource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Triển khai phương thức Invoke
        /// </summary>
        /// <param name="context">Đối tượng HttpContext</param>
        /// <param name="next">Đối tượng RequestDelegate</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                HandleBadRequest(context);
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }
        /// <summary>
        /// Xử lý bad request
        /// </summary>
        /// <param name="context">Đối tượng HttpContext</param>
        /// Created by: nlnhat (31/07/2023)
        private static void HandleBadRequest(HttpContext context)
        {
            var options = context.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>().Value;

            options.InvalidModelStateResponseFactory = (actionContext) =>
            {
                var errors = actionContext.ModelState.Values.SelectMany(error => error.Errors).ToList();
                var errorMsg = errors.First().ErrorMessage;
                var models = actionContext.ModelState;

                var key = string.Empty;
                dynamic value;  

                if (models != null)
                {
                    foreach (var item in models)
                    {
                        if (item.Value.Errors.Count > 0)
                        {
                            key = item.Key;
                            value = item.Value.Errors;
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            break;
                        }
                    }
                }

                return new BadRequestObjectResult(new ServerResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCode.InvalidData,
                    DevMsg = errorMsg,
                    UserMsg = errorMsg,
                    TraceId = context.TraceIdentifier,
                    MoreInfo = "",
                    Data = new
                    {
                        Key = key,
                        Value = actionContext.ModelState
                    }
                });
            };
        }
        /// <summary>
        /// Xử lý exception
        /// </summary>
        /// <param name="context">Đối tượng HttpContext</param>
        /// <param name="exception">Đối tượng Exception</param>
        /// Created by: nlnhat (31/06/2023)
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(
                text: new ServerResponse()
                {
                    Success = false,
                    ErrorCode = ErrorCode.ServerError,
                    DevMsg = exception.Message,
                    UserMsg = _resource["ServerError"],
                    TraceId = context.TraceIdentifier,
                    MoreInfo = exception.HelpLink,
                    ErrorKey = ErrorKey.Exception,
                    Data = new
                    {
                        exception.Source,
                        exception.Data,
                        exception.StackTrace,
                        TargetSite = exception.TargetSite?.ToString(),
                    }
                }.ToString() ?? ""
            );
        }
        #endregion
    }
}