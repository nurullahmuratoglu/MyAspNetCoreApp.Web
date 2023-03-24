using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace MyAspNetCoreApp.Web.Filters
{
    public class LogFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("action metot çalışmadan önce");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("action metot çalıştıktan sonra");
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Debug.WriteLine("action metot sonuç üretilmeden önce");
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Debug.WriteLine("action metot sonuç üretildikten sonra");
        }
    }
}
