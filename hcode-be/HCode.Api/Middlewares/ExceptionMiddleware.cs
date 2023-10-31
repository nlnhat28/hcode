using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Hcode.Api.Domain;

namespace Hcode.Api.Api
{
    /// <summary>
    /// Middleware xử lý ngoại lệ
    /// </summary>
    /// Created by: nlnhat (12/07/2023)
    public class ExceptionMiddleware : IMiddleware
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
        /// <param name="logger">Đối tượng logger</param>
        public ExceptionMiddleware(IStringLocalizer<Resource> resource)
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

                return new BadRequestObjectResult(new MISAException()
                {
                    ErrorCode = MISAErrorCode.InvalidData,
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
            switch (exception)
            {
                // Ngoại lệ không tìm thấy
                case NotFoundException:
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsync(text: new MISAException()
                        {
                            ErrorCode = ((NotFoundException)exception).ErrorCode,
                            DevMsg = ((NotFoundException)exception).Message ?? _resource["NotFound"],
                            UserMsg = ((NotFoundException)exception).UserMsg ?? _resource["NotFound"],
                            TraceId = context.TraceIdentifier,
                            MoreInfo = exception.HelpLink,
                            Data = ((NotFoundException)exception).Data
                        }.ToString() ?? ""); ;
                        break;
                    };
                // Ngoại lệ dữ liệu không hợp lệ
                case ValidateException:
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync(text: new MISAException()
                        {
                            ErrorCode = ((ValidateException)exception).ErrorCode,
                            DevMsg = ((ValidateException)exception).Message ?? _resource["InvalidData"],
                            UserMsg = ((ValidateException)exception).UserMsg ?? _resource["InvalidData"],
                            TraceId = context.TraceIdentifier,
                            MoreInfo = exception.HelpLink,
                            Data = ((ValidateException)exception).Data
                        }.ToString() ?? "");
                        break;
                    }
                // Ngoại lệ xung đột dữ liệu
                case ConflictException:
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        await context.Response.WriteAsync(text: new MISAException()
                        {
                            ErrorCode = ((ConflictException)exception).ErrorCode,
                            DevMsg = ((ConflictException)exception).Message ?? _resource["ConflictData"],
                            UserMsg = ((ConflictException)exception).UserMsg ?? _resource["ConflictData"],
                            TraceId = context.TraceIdentifier,
                            MoreInfo = exception.HelpLink,
                            Data = ((ConflictException)exception).Data
                        }.ToString() ?? ""); ;
                        break;
                    };

                // Ngoại lệ không hoàn thành tác vụ
                case IncompleteException:
                    {
                        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                        await context.Response.WriteAsync(text: new MISAException()
                        {
                            ErrorCode = ((IncompleteException)exception).ErrorCode,
                            DevMsg = ((IncompleteException)exception).DevMsg ?? _resource["IncompleteTask"],
                            UserMsg = ((IncompleteException)exception).UserMsg ?? _resource["IncompleteTask"],
                            TraceId = context.TraceIdentifier,
                            MoreInfo = exception.HelpLink,
                            Data = ((IncompleteException)exception).Data
                        }.ToString() ?? ""); ;
                        break;
                    };
                // Mặc định
                default:
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync(
                            text: new MISAException()
                            {
                                ErrorCode = MISAErrorCode.ServerError,
                                DevMsg = exception.Message,
                                UserMsg = _resource["ServerError"],
                                TraceId = context.TraceIdentifier,
                                MoreInfo = exception.HelpLink,
                                Data = { }
                            }.ToString() ?? ""
                        );
                    };
                    break;
            }
        }
        #endregion
    }
}