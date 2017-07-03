using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Bootstrap.Components.Models;
using Bootstrap.Components.Models.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Bootstrap.Components.Filters
{
    /// <summary>
    /// Use <see cref="Bootstrap.Components.Middlewares.RestfulApiSimpleExceptionHandlingMiddleware"/> with this filter.
    /// </summary>
    public class RestfulApiSimpleInvalidDataFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (context.ModelState.Any(t => t.Value.Errors.Any(a => a.Exception is JsonReaderException)))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest, "The payload is not invalid JSON");
                }
                throw new HttpResponseException(HttpStatusCode.BadRequest, string.Join("; ",
                    context.ModelState.Select(
                        t =>
                            $"{t.Key.Substring(0, 1)?.ToLower()}{t.Key.Substring(1)} {string.Join(", ", t.Value.Errors.Select(e => e.ErrorMessage))}"
                                .Trim())));
            }
        }
    }
}