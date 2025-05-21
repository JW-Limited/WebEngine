using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using LILO_WebEngine.Core.Contracts;

namespace LILO_WebEngine.Core.Handler
{
    public class ErrorHandler : ICoreTaskHandler
    {
        private readonly IErrorPageGenerator errorPageGenerator;

        public ErrorHandler(IErrorPageGenerator errorPageGenerator)
        {
            this.errorPageGenerator = errorPageGenerator;
        }

        public async Task Handle(HttpListenerContext context, HttpStatusCode statusCode, Exception ex = null)
        {
            var response = context.Response;
            response.StatusCode = (int)statusCode;

            response.ContentType = "text/html";
            var errorMessage = errorPageGenerator.GenerateErrorPage(statusCode, ex);
            var errorBytes = Encoding.UTF8.GetBytes(errorMessage);
            response.ContentLength64 = errorBytes.Length;
            await response.OutputStream.WriteAsync(errorBytes, 0, errorBytes.Length);
        }
    }
}
