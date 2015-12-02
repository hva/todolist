using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace TodoList.Api
{
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