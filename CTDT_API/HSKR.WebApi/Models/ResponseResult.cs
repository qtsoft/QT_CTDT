using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;

namespace CTDT.WebApi.Models
{
    public class ResponseResult : IHttpActionResult
    {

        private object _resultValue;
        private HttpActionContext _actionContext;

        public ResponseResult(object result, HttpActionContext context)
        {
            _resultValue = result;
            _actionContext = context;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            StringContent content = null;
            HttpStatusCode statusCode;
            var temp = "";
            if (_resultValue != null)
            {
                if (_resultValue.GetType() == typeof(bool) || _resultValue.GetType() == typeof(Boolean))
                {
                    temp = _resultValue.ToString().ToLower();
                    content = new StringContent(temp);
                }
                else if (_resultValue.GetType().IsPrimitive || _resultValue.GetType() == typeof(string))
                {
                    temp = _resultValue + "";
                    content = new StringContent(temp);
                }
                else
                {
                    temp = JsonConvert.SerializeObject(_resultValue);
                    content = new StringContent(temp);
                }
            }
             statusCode = _actionContext.Response != null ? _actionContext.Response.StatusCode : HttpStatusCode.OK;
            var response = new HttpResponseMessage()
            {
                Content = content,
                RequestMessage = _actionContext.Request,
                StatusCode = statusCode, 
            };
            return Task.FromResult(response);
        }
    }
}
