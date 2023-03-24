using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyAspNetCoreApp.Web.Filters
{
    public class CashResourceFilter : Attribute, IResourceFilter
    {


        private static IActionResult _cache;
        public void OnResourceExecuted(ResourceExecutedContext context)//response üretildekten sonra çalışacak metot
        {
            _cache = context.Result;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)//ilk request geldiği anda çalışacak metot
        {
            if (_cache != null)
            { 
                context.Result = _cache;
            
            }
        }
    }
}
