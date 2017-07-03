﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bootstrap.Components.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bootstrap.Components.Middlewares
{
    public class ElapsedTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly EventId _eventId = new EventId(0, nameof(ElapsedTimeMiddleware));

        public ElapsedTimeMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ElapsedTimeMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = Stopwatch.StartNew();
            await _next(context);
            stopWatch.Stop();
            var message = $"[Total] {stopWatch.ElapsedMilliseconds}ms, {stopWatch.ElapsedTicks}ticks";
            var elapsedTimeInfo = context.Items.GetElaspedTimeInfo();
            if (elapsedTimeInfo != null)
            {
                message += Environment.NewLine + string.Join(Environment.NewLine,
                               elapsedTimeInfo.Select(t => $"[{t.Key.ToString()}] {t.Value.ToString()}"));
            }
            _logger.LogTrace(_eventId, message);
        }
    }

    public static class ElapsedTimeMiddlewareServiceCollectionExtensions
    {
        public static IApplicationBuilder UseElapsedTime(IApplicationBuilder app)
        {
            return app.UseMiddleware<ElapsedTimeMiddleware>();
        }
    }
}