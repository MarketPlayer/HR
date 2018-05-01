using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ReviewMe.Exceptions
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // TODO: логировать исключение...

            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(actionExecutedContext.Exception.Message)
            };

            base.OnException(actionExecutedContext);
        }
    }
}