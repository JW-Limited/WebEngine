using LILO_WebEngine.Core.Contracts;
using LILO_WebEngine.Service;
using System;
using System.Net;
using System.Text;
using System.Linq;

namespace LILO_WebEngine.Core.Pages
{
    public class ErrorHtmlDynamic : IErrorPageGenerator
    {
        private readonly string defaultTitle = "Server Error";
        private readonly string defaultBackgroundColor = "#f8f9fa";
        private readonly string defaultFontFamily = "'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif";

        private readonly Dictionary<HttpStatusCode, string> errorMessages = new Dictionary<HttpStatusCode, string>
        {
            { HttpStatusCode.NotFound, "The resource you are looking for might have been removed, had its name changed, or is temporarily unavailable." },
            { HttpStatusCode.InternalServerError, "An internal server error occurred." },
            { HttpStatusCode.BadRequest, "Bad Request: The server could not understand the request." },
            { HttpStatusCode.Unauthorized, "Unauthorized: Access to the requested resource is not authorized." },
            { HttpStatusCode.Forbidden, "Forbidden: Access to the requested resource is forbidden." },
            { HttpStatusCode.MethodNotAllowed, "Method Not Allowed: The requested HTTP method is not allowed for the resource." },
            { HttpStatusCode.NotAcceptable, "Not Acceptable: The server cannot produce a response matching the list of acceptable values defined in the request's headers." },
            { HttpStatusCode.ProxyAuthenticationRequired, "Proxy Authentication Required: The client must first authenticate itself with the proxy." },
            { HttpStatusCode.RequestTimeout, "Request Timeout: The server timed out waiting for the request." },
            { HttpStatusCode.Conflict, "Conflict: The request could not be completed due to a conflict with the current state of the target resource." },
            { HttpStatusCode.Gone, "Gone: The requested resource is no longer available at the server." },
            { HttpStatusCode.ServiceUnavailable, "Service Unavailable: The server is currently unable to handle the request due to maintenance or overloading." },
            { HttpStatusCode.GatewayTimeout, "Gateway Timeout: The server, while acting as a gateway or proxy, did not receive a timely response from an upstream server." }
        };

        private Dictionary<HttpStatusCode, string> errorIcons = new Dictionary<HttpStatusCode, string>
        {
            { HttpStatusCode.NotFound, "search" },
            { HttpStatusCode.InternalServerError, "server" },
            { HttpStatusCode.BadRequest, "x-octagon" },
            { HttpStatusCode.Unauthorized, "lock" },
            { HttpStatusCode.Forbidden, "shield-x" },
            { HttpStatusCode.MethodNotAllowed, "slash" },
            { HttpStatusCode.RequestTimeout, "clock" },
            { HttpStatusCode.Conflict, "git-merge" },
            { HttpStatusCode.Gone, "file-minus" },
            { HttpStatusCode.ServiceUnavailable, "alert-triangle" },
            { HttpStatusCode.GatewayTimeout, "globe" }
        };

        public string GenerateErrorPage(HttpStatusCode statusCode, string additionalErrorMessage = null, string backgroundColor = null, string fontFamily = null)
        {
            var sb = new StringBuilder();
            string statusCodeText = ((int)statusCode).ToString();
            string statusMessage = Enum.GetName(typeof(HttpStatusCode), statusCode) ?? "Unknown Error";
            string iconName = errorIcons.ContainsKey(statusCode) ? errorIcons[statusCode] : "alert-circle";
            
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"UTF-8\">");
            sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine($"<title>{statusCodeText} | {statusMessage}</title>");
            sb.AppendLine("<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css\" rel=\"stylesheet\">");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/feather-icons/dist/feather.min.css\">");
            sb.AppendLine("<style>");
            sb.AppendLine($"body {{ font-family: {fontFamily ?? defaultFontFamily}; background-color: {backgroundColor ?? defaultBackgroundColor}; color: #212529; margin: 0; padding: 0; min-height: 100vh; display: flex; flex-direction: column; }}");
            sb.AppendLine("* { box-sizing: border-box; }");
            sb.AppendLine(".header { position: relative; padding: 0.75rem 1rem; background: linear-gradient(135deg, #0d6efd, #0a58ca); color: white; box-shadow: 0 2px 10px rgba(0,0,0,0.1); z-index: 100; }");
            sb.AppendLine(".header-content { display: flex; justify-content: space-between; align-items: center; max-width: 1200px; margin: 0 auto; }");
            sb.AppendLine(".brand { font-weight: 600; font-size: 1rem; }");
            sb.AppendLine(".version { font-size: 0.75rem; opacity: 0.85; }");
            sb.AppendLine(".main-content { flex: 1; display: flex; align-items: center; justify-content: center; padding: 1rem; }");
            sb.AppendLine(".error-container { width: 100%; max-width: 680px; background-color: white; border-radius: 0.5rem; box-shadow: 0 5px 25px rgba(0,0,0,0.08); overflow: hidden; transition: all 0.3s ease; margin: 0 1rem; }");
            sb.AppendLine(".error-container:hover { transform: translateY(-5px); box-shadow: 0 8px 30px rgba(0,0,0,0.12); }");
            sb.AppendLine(".error-header { display: flex; align-items: center; padding: 1.5rem; border-bottom: 1px solid rgba(0,0,0,0.05); }");
            sb.AppendLine(".status-code { font-size: 1.75rem; font-weight: 700; margin-right: 1rem; color: #dc3545; }");
            sb.AppendLine(".status-text { font-size: 1.25rem; font-weight: 500; color: #343a40; }");
            sb.AppendLine(".error-body { padding: 1.5rem; }");
            sb.AppendLine(".error-message { font-size: 1rem; line-height: 1.6; color: #495057; margin-bottom: 1.5rem; }");
            sb.AppendLine(".additional-info { background-color: #f8f9fa; padding: 1rem; border-radius: 0.25rem; font-size: 0.875rem; color: #6c757d; margin-top: 1rem; border-left: 3px solid #dee2e6; white-space: pre-wrap; word-break: break-word; }");
            sb.AppendLine(".icon-container { display: flex; justify-content: center; margin-bottom: 1.5rem; }");
            sb.AppendLine(".error-icon { width: 80px; height: 80px; color: #dc3545; stroke-width: 1; }");
            sb.AppendLine(".error-icon.subtle { color: #6c757d; opacity: 0.7; }");
            sb.AppendLine(".footer { text-align: center; padding: 1rem; font-size: 0.75rem; color: #6c757d; opacity: 0.8; }");
            sb.AppendLine(".back-link { text-decoration: none; color: #0d6efd; display: inline-flex; align-items: center; font-weight: 500; margin-top: 0.5rem; transition: color 0.15s ease; }");
            sb.AppendLine(".back-link:hover { color: #0a58ca; }");
            sb.AppendLine(".back-link i { margin-right: 0.25rem; }");
            sb.AppendLine(".error-code-badge { position: absolute; top: 0; right: 0; font-size: 8rem; font-weight: 800; color: rgba(0,0,0,0.03); line-height: 1; z-index: 0; overflow: hidden; user-select: none; }");
            sb.AppendLine(".error-actions { margin-top: 1.5rem; display: flex; gap: 0.5rem; }");
            sb.AppendLine(".btn { display: inline-flex; align-items: center; padding: 0.5rem 1rem; border-radius: 0.25rem; text-decoration: none; font-weight: 500; transition: all 0.2s ease; }");
            sb.AppendLine(".btn-primary { background-color: #0d6efd; color: white; }");
            sb.AppendLine(".btn-primary:hover { background-color: #0a58ca; }");
            sb.AppendLine(".btn-outline { border: 1px solid #dee2e6; color: #495057; }");
            sb.AppendLine(".btn-outline:hover { background-color: #f8f9fa; }");
            sb.AppendLine(".btn i { margin-right: 0.25rem; }");
            sb.AppendLine("@keyframes fadeIn { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }");
            sb.AppendLine(".fade-in { animation: fadeIn 0.5s ease forwards; }");
            sb.AppendLine("@media (max-width: 576px) { .error-container { margin: 0 0.5rem; } .error-header { flex-direction: column; align-items: flex-start; } .status-code { margin-bottom: 0.5rem; margin-right: 0; } }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            
            sb.AppendLine("<header class=\"header\">");
            sb.AppendLine("  <div class=\"header-content\">");
            sb.AppendLine("    <div class=\"brand\">LILO-WebEngine</div>");
            sb.AppendLine($"    <div class=\"version\">{new LocalServerEventArgs().SemanticVersion}</div>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</header>");
            
            sb.AppendLine("<main class=\"main-content\">");
            sb.AppendLine("  <div class=\"error-container fade-in\">");
            sb.AppendLine("    <div class=\"error-code-badge\">" + statusCodeText + "</div>");
            sb.AppendLine("    <div class=\"error-header\">");
            sb.AppendLine($"      <div class=\"status-code\">{statusCodeText}</div>");
            sb.AppendLine($"      <div class=\"status-text\">{statusMessage}</div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"error-body\">");
            sb.AppendLine("      <div class=\"icon-container\">");
            sb.AppendLine($"        <i data-feather=\"{iconName}\" class=\"error-icon\"></i>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"error-message\">");
            sb.AppendLine(errorMessages.ContainsKey(statusCode) ? errorMessages[statusCode] : "An unexpected error occurred.");
            sb.AppendLine("      </div>");
            
            if (!string.IsNullOrEmpty(additionalErrorMessage))
            {
                sb.AppendLine("      <div class=\"additional-info\">");
                sb.AppendLine(additionalErrorMessage);
                sb.AppendLine("      </div>");
            }
            
            sb.AppendLine("      <div class=\"error-actions\">");
            sb.AppendLine("        <a href=\"javascript:history.back()\" class=\"btn btn-primary\"><i data-feather=\"arrow-left\" width=\"16\" height=\"16\"></i> Go Back</a>");
            sb.AppendLine("        <a href=\"/\" class=\"btn btn-outline\"><i data-feather=\"home\" width=\"16\" height=\"16\"></i> Home</a>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</main>");
            
            sb.AppendLine("<footer class=\"footer\">");
            sb.AppendLine("  <div>&copy; " + DateTime.Now.Year + " LILO-WebEngine | All rights reserved</div>");
            sb.AppendLine("</footer>");
            
            sb.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/feather-icons/dist/feather.min.js\"></script>");
            sb.AppendLine("<script>");
            sb.AppendLine("  document.addEventListener('DOMContentLoaded', function() {");
            sb.AppendLine("    feather.replace();");
            sb.AppendLine("  });");
            sb.AppendLine("</script>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public string GenerateErrorPage(HttpStatusCode statusCode, Exception ex = null, string backgroundColor = null, string fontFamily = null)
        {
            string additionalErrorMessage = null;
            
            if (ex != null)
            {
                StringBuilder exDetails = new StringBuilder();
                exDetails.AppendLine($"Error Details: {ex.Message}");
                
                if (ex.InnerException != null)
                    exDetails.AppendLine($"Inner Exception: {ex.InnerException.Message}");
                
                if (!string.IsNullOrEmpty(ex.StackTrace))
                    exDetails.AppendLine($"Stack Trace: {ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None).FirstOrDefault()}...");
                
                if (!string.IsNullOrEmpty(ex.Source))
                    exDetails.AppendLine($"Source: {ex.Source}");
                
                additionalErrorMessage = exDetails.ToString();
            }

            return GenerateErrorPage(statusCode, additionalErrorMessage, backgroundColor, fontFamily);
        }
    }
}