﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bootstrap.Components.Extensions;
using Bootstrap.Components.Models;
using Bootstrap.Components.Models.Constants;
using Bootstrap.Components.Models.ResponseModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bootstrap.Components.Middlewares
{
    public class SimpleExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly EventId _eventId = new EventId(0, nameof(SimpleExceptionHandlingMiddleware));
        private readonly string _ajaxResponse;

        public SimpleExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<SimpleExceptionHandlingMiddleware>();
            _ajaxResponse = JsonConvert.SerializeObject(new BaseResponse((int)ResponseCode.InternalError,
                    ResponseCode.InternalError.ToString()),
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(_eventId, e, context.BuildRequestInformation());
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                    context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                    context.Response.Headers[HeaderNames.Expires] = "-1";
                    context.Response.Headers.Remove(HeaderNames.ETag);
                    //Default behavior
                    if (context.Request.AcceptJson())
                    {
                        await context.Response.WriteAsync(_ajaxResponse, Encoding.UTF8);
                    }
                    else
                    {
                        context.Response.Redirect("/");
                    }
                }
            }
        }
    }
    public static class ExceptionHandlingMiddlewareServiceCollectionExtensions
    {
        public static IApplicationBuilder UseSimpleExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SimpleExceptionHandlingMiddleware>();
        }
    }
}