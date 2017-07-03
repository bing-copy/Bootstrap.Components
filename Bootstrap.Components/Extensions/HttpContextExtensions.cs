﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Bootstrap.Components.Extensions
{
    public static class HttpContextItemExtensions
    {
        public const string HttpContextElaspsedTimeKey = "ElapsedTimeInfo";

        public static void AddElapsedTimeInfo(this IDictionary<object, object> items, object key, object info)
        {
            IList<KeyValuePair<object, object>> list;
            if (items.TryGetValue(HttpContextElaspsedTimeKey, out var value))
            {
                list = value as IList<KeyValuePair<object, object>>;
            }
            else
            {
                items[HttpContextElaspsedTimeKey] = list = new List<KeyValuePair<object, object>>();
            }
            list?.Add(new KeyValuePair<object, object>(key, info));
        }

        public static IList<KeyValuePair<object, object>> GetElaspedTimeInfo(this IDictionary<object, object> items)
        {
            return items.TryGetValue(HttpContextElaspsedTimeKey, out var value)
                ? value as IList<KeyValuePair<object, object>>
                : null;
        }

        public static string BuildRequestInformation(this HttpContext context)
        {
            return string.Join(Environment.NewLine,
                $"DisplayUrl: {context.Request.GetDisplayUrl()}",
                $"Connection: {context.TraceIdentifier}",
                $"ClientIp: {context.Request.GetClientIp()}");
        }
    }
}