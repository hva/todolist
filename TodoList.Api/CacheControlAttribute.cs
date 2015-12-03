using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace TodoList.Api
{
    // CacheControlAttribute is used to set cache HTTP headers
    // to methods of controller.
    public class CacheControlAttribute : ActionFilterAttribute
    {
        public int MaxAge { get; set; }
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            context.Response.Headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(MaxAge),
            };
        }
    }
}