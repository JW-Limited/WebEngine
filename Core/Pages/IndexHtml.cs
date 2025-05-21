using LILO_WebEngine.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LILO_WebEngine.Core.Pages
{
    internal class IndexHtml : ILILOHtmlGenerator
    {
        private static object _lock = new object();
        private static IndexHtml _instance;

        public static IndexHtml Instance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new IndexHtml();
                }

                return _instance;
            }
        }

        private IndexHtml()
        {

        }

        public string GetSizeString(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size} {sizes[order]}";
        }

        public static void SetInstance(object instance)
        {
            _instance = (IndexHtml)instance;
        }

        public string GetFileIcon(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".pdf": return "file-pdf";
                case ".doc":
                case ".docx": return "file-word";
                case ".xls":
                case ".xlsx": return "file-excel";
                case ".ppt":
                case ".pptx": return "file-ppt";
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp": return "file-image";
                case ".mp3":
                case ".wav":
                case ".ogg": return "file-music";
                case ".mp4":
                case ".avi":
                case ".mov": return "file-play";
                case ".zip":
                case ".rar":
                case ".7z": return "file-zip";
                case ".txt": return "file-text";
                case ".cs": return "code-square";
                case ".html":
                case ".htm": return "code";
                case ".css": return "filetype-css";
                case ".js": return "filetype-js";
                default: return "file-earmark";
            }
        }

        public string MicrosoftWordWrapper(string filePath)
        {
            var sb = new StringBuilder();
            var info = new FileInfo(filePath);
            string[] fileContent = File.ReadAllLines(filePath);
            string fileExtension = Path.GetExtension(filePath).ToLower();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"UTF-8\">");
            sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine("<title>Advanced Text Viewer</title>");
            sb.AppendLine("<link rel=\"icon\" type=\"image/png\" sizes=\"32x32\" href=\"/images/favlogo.png\">");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"/css/word-wrapper.css\">");
            sb.AppendLine("<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css\" rel=\"stylesheet\">");
            sb.AppendLine("<link href=\"https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css\" rel=\"stylesheet\">");
            sb.AppendLine("<link href=\"https://cdn.quilljs.com/1.3.6/quill.snow.css\" rel=\"stylesheet\">");
            sb.AppendLine("<link href=\"https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap\" rel=\"stylesheet\">");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/themes/prism-tomorrow.min.css\">");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: 'Inter', sans-serif; margin: 0; padding: 0; background-color: #f8f9fa; color: #212529; }");
            sb.AppendLine("* { box-sizing: border-box; }");
            sb.AppendLine(".app-container { display: flex; flex-direction: column; height: 100vh; }");
            sb.AppendLine(".header { background: linear-gradient(135deg, #0d6efd, #0a58ca); color: white; padding: 1rem; box-shadow: 0 2px 10px rgba(0,0,0,0.1); display: flex; justify-content: space-between; align-items: center; }");
            sb.AppendLine(".header h1 { margin: 0; font-size: 1.25rem; font-weight: 600; display: flex; align-items: center; }");
            sb.AppendLine(".app-name-subtitle { font-size: 0.75rem; opacity: 0.8; margin-left: 0.5rem; font-weight: normal; }");
            sb.AppendLine(".toolbar { background-color: white; padding: 0.75rem; border-bottom: 1px solid rgba(0,0,0,0.1); display: flex; gap: 0.5rem; }");
            sb.AppendLine(".toolbar button { display: inline-flex; align-items: center; gap: 0.5rem; border: none; background-color: #f1f3f5; color: #495057; padding: 0.5rem 0.75rem; border-radius: 0.25rem; cursor: pointer; font-size: 0.875rem; font-weight: 500; transition: all 0.2s ease; }");
            sb.AppendLine(".toolbar button:hover { background-color: #e9ecef; }");
            sb.AppendLine(".toolbar button i { font-size: 1rem; }");
            sb.AppendLine(".editor-main { display: flex; height: calc(100vh - 115px); }");
            sb.AppendLine(".editor-container { flex: 1; position: relative; background-color: white; border-right: 1px solid rgba(0,0,0,0.1); }");
            sb.AppendLine(".sidebar { width: 300px; background-color: white; padding: 1rem; overflow-y: auto; display: flex; flex-direction: column; }");
            sb.AppendLine(".file-info { margin-bottom: 1.5rem; }");
            sb.AppendLine(".file-info-header { display: flex; align-items: center; margin-bottom: 1rem; }");
            sb.AppendLine(".file-icon { font-size: 2.5rem; margin-right: 0.75rem; color: #0d6efd; }");
            sb.AppendLine(".file-title { font-size: 1.125rem; font-weight: 600; color: #212529; margin: 0 0 0.25rem 0; }");
            sb.AppendLine(".file-path { font-size: 0.75rem; color: #6c757d; margin: 0; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }");
            sb.AppendLine(".file-details { margin-top: 1rem; }");
            sb.AppendLine(".detail-item { display: flex; margin-bottom: 0.5rem; align-items: center; }");
            sb.AppendLine(".detail-label { font-size: 0.75rem; color: #6c757d; width: 80px; }");
            sb.AppendLine(".detail-value { font-size: 0.875rem; color: #212529; flex: 1; font-weight: 500; }");
            sb.AppendLine(".search-container { margin-top: 1rem; }");
            sb.AppendLine(".search-input { display: flex; }");
            sb.AppendLine(".search-input input { flex: 1; padding: 0.5rem; border: 1px solid #dee2e6; border-radius: 0.25rem 0 0 0.25rem; font-size: 0.875rem; }");
            sb.AppendLine(".search-input button { background-color: #0d6efd; border: none; color: white; padding: 0.5rem 0.75rem; border-radius: 0 0.25rem 0.25rem 0; cursor: pointer; }");
            sb.AppendLine(".search-results { margin-top: 0.5rem; max-height: 200px; overflow-y: auto; }");
            sb.AppendLine(".search-result { padding: 0.5rem; font-size: 0.875rem; cursor: pointer; border-radius: 0.25rem; }");
            sb.AppendLine(".search-result:hover { background-color: #f1f3f5; }");
            sb.AppendLine(".search-result.active { background-color: #e9ecef; }");
            sb.AppendLine(".action-buttons { margin-top: auto; display: flex; flex-direction: column; gap: 0.5rem; }");
            sb.AppendLine(".btn { display: inline-flex; align-items: center; justify-content: center; gap: 0.5rem; border: none; padding: 0.5rem 0.75rem; border-radius: 0.25rem; cursor: pointer; font-size: 0.875rem; font-weight: 500; transition: all 0.2s ease; width: 100%; }");
            sb.AppendLine(".btn-primary { background-color: #0d6efd; color: white; }");
            sb.AppendLine(".btn-primary:hover { background-color: #0a58ca; }");
            sb.AppendLine(".btn-outline { background-color: transparent; border: 1px solid #dee2e6; color: #495057; }");
            sb.AppendLine(".btn-outline:hover { background-color: #f8f9fa; }");
            sb.AppendLine(".editor { height: 100%; }");
            sb.AppendLine(".ql-container { height: calc(100% - 42px); border: none !important; font-size: 0.9375rem; }");
            sb.AppendLine(".ql-toolbar { border-top: none !important; border-left: none !important; border-right: none !important; }");
            sb.AppendLine(".code-container { height: 100%; overflow: auto; position: relative; }");
            sb.AppendLine(".code-with-line-numbers { display: flex; min-height: 100%; }");
            sb.AppendLine(".line-numbers { padding: 1rem 0.5rem; text-align: right; background-color: #2d2d2d; color: #606366; user-select: none; width: 3.5rem; flex-shrink: 0; font-family: monospace; font-size: 0.875rem; border-right: 1px solid #444; }");
            sb.AppendLine(".code-content { padding: 1rem; flex: 1; overflow-x: auto; font-family: monospace; font-size: 0.875rem; background-color: #2d2d2d; color: #f8f8f2; }");
            sb.AppendLine("code[class*=\"language-\"] { font-family: monospace; }");
            sb.AppendLine("@media (max-width: 768px) { .editor-main { flex-direction: column; } .sidebar { width: 100%; height: auto; max-height: 300px; } }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            
            sb.AppendLine("<div class=\"app-container\">");
            sb.AppendLine("  <header class=\"header\">");
            sb.AppendLine("    <h1>Advanced Text Viewer<span class=\"app-name-subtitle\">Beta</span></h1>");
            sb.AppendLine("    <div>");
            sb.AppendLine("      <span id=\"status-indicator\" class=\"status-indicator\">Ready</span>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </header>");
            
            sb.AppendLine("  <div class=\"toolbar\">");
            sb.AppendLine("    <button id=\"save-button\"><i class=\"bi bi-save\"></i> Save</button>");
            sb.AppendLine("    <button id=\"copy-button\"><i class=\"bi bi-clipboard\"></i> Copy</button>");
            sb.AppendLine("    <button id=\"download-button\"><i class=\"bi bi-download\"></i> Download</button>");
            sb.AppendLine("    <button id=\"print-button\"><i class=\"bi bi-printer\"></i> Print</button>");
            sb.AppendLine("  </div>");
            
            sb.AppendLine("  <div class=\"editor-main\">");
            
            // Editor section based on file type
            sb.AppendLine("    <div class=\"editor-container\">");
            if (fileExtension == ".cs" || fileExtension == ".js" || fileExtension == ".html" || fileExtension == ".css" || fileExtension == ".txt" || fileExtension == ".xml" || fileExtension == ".json")
            {
                sb.AppendLine("      <div class=\"code-container\">");
                sb.AppendLine("        <div class=\"code-with-line-numbers\">");
                sb.AppendLine("          <div class=\"line-numbers\" id=\"line-numbers\"></div>");
                sb.AppendLine($"          <pre class=\"code-content\"><code class=\"language-{GetLanguageFromExtension(fileExtension)}\" id=\"code-content\"></code></pre>");
                sb.AppendLine("        </div>");
                sb.AppendLine("      </div>");
            }
            else
            {
                sb.AppendLine("      <div id=\"editor\"></div>");
            }
            sb.AppendLine("    </div>");
            
            sb.AppendLine("    <div class=\"sidebar\">");
            sb.AppendLine("      <div class=\"file-info\">");
            sb.AppendLine("        <div class=\"file-info-header\">");
            sb.AppendLine($"          <i class=\"bi bi-{GetFileIcon(info.Name)} file-icon\"></i>");
            sb.AppendLine("          <div>");
            sb.AppendLine($"            <h2 class=\"file-title\">{info.Name}</h2>");
            sb.AppendLine($"            <p class=\"file-path\">{info.FullName}</p>");
            sb.AppendLine("          </div>");
            sb.AppendLine("        </div>");
            
            sb.AppendLine("        <div class=\"file-details\">");
            sb.AppendLine("          <div class=\"detail-item\">");
            sb.AppendLine("            <span class=\"detail-label\">Type:</span>");
            sb.AppendLine($"            <span class=\"detail-value\">{info.Extension.TrimStart('.').ToUpper()} File</span>");
            sb.AppendLine("          </div>");
            sb.AppendLine("          <div class=\"detail-item\">");
            sb.AppendLine("            <span class=\"detail-label\">Size:</span>");
            sb.AppendLine($"            <span class=\"detail-value\">{GetSizeString(info.Length)}</span>");
            sb.AppendLine("          </div>");
            sb.AppendLine("          <div class=\"detail-item\">");
            sb.AppendLine("            <span class=\"detail-label\">Created:</span>");
            sb.AppendLine($"            <span class=\"detail-value\">{info.CreationTime.ToString("yyyy-MM-dd HH:mm")}</span>");
            sb.AppendLine("          </div>");
            sb.AppendLine("          <div class=\"detail-item\">");
            sb.AppendLine("            <span class=\"detail-label\">Modified:</span>");
            sb.AppendLine($"            <span class=\"detail-value\">{info.LastWriteTime.ToString("yyyy-MM-dd HH:mm")}</span>");
            sb.AppendLine("          </div>");
            sb.AppendLine("          <div class=\"detail-item\">");
            sb.AppendLine("            <span class=\"detail-label\">Lines:</span>");
            sb.AppendLine($"            <span class=\"detail-value\">{fileContent.Length}</span>");
            sb.AppendLine("          </div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");
            
            sb.AppendLine("      <div class=\"search-container\">");
            sb.AppendLine("        <div class=\"search-input\">");
            sb.AppendLine("          <input type=\"text\" id=\"search-input\" placeholder=\"Search text...\">");
            sb.AppendLine("          <button id=\"search-button\"><i class=\"bi bi-search\"></i></button>");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class=\"search-results\" id=\"search-results\"></div>");
            sb.AppendLine("      </div>");
            
            sb.AppendLine("      <div class=\"action-buttons\">");
            sb.AppendLine("        <button class=\"btn btn-primary\" id=\"open-editor-button\"><i class=\"bi bi-pencil-square\"></i> Open in Text Editor</button>");
            sb.AppendLine("        <button class=\"btn btn-outline\" id=\"back-button\"><i class=\"bi bi-arrow-left\"></i> Back to Files</button>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");
            
            sb.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js\"></script>");
            sb.AppendLine("<script src=\"https://cdn.quilljs.com/1.3.6/quill.min.js\"></script>");
            sb.AppendLine("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/prism.min.js\"></script>");
            sb.AppendLine("<script src=\"https://cdnjs.cloudflare.com/ajax/libs/prism/1.29.0/plugins/autoloader/prism-autoloader.min.js\"></script>");
            
            sb.AppendLine("<script>");
            sb.AppendLine("document.addEventListener('DOMContentLoaded', function() {");
            
            if (fileExtension == ".cs" || fileExtension == ".js" || fileExtension == ".html" || fileExtension == ".css" || fileExtension == ".txt" || fileExtension == ".xml" || fileExtension == ".json")
            {
                sb.AppendLine("  // Code file handling");
                sb.AppendLine("  const codeContent = document.getElementById('code-content');");
                sb.AppendLine("  const lineNumbers = document.getElementById('line-numbers');");
                sb.AppendLine($"  const fileContent = {Newtonsoft.Json.JsonConvert.SerializeObject(string.Join("\n", fileContent))};");
                sb.AppendLine("  codeContent.textContent = fileContent;");
                sb.AppendLine("  ");
                sb.AppendLine("  // Generate line numbers");
                sb.AppendLine("  const lines = fileContent.split('\\n');");
                sb.AppendLine("  lines.forEach((line, index) => {");
                sb.AppendLine("    const lineNumber = document.createElement('div');");
                sb.AppendLine("    lineNumber.textContent = index + 1;");
                sb.AppendLine("    lineNumbers.appendChild(lineNumber);");
                sb.AppendLine("  });");
                sb.AppendLine("  ");
                sb.AppendLine("  // Highlight syntax");
                sb.AppendLine("  Prism.highlightElement(codeContent);");
            }
            else
            {
                sb.AppendLine("  // Rich text handling with Quill");
                sb.AppendLine("  const quill = new Quill('#editor', {");
                sb.AppendLine("    theme: 'snow',");
                sb.AppendLine("    modules: {");
                sb.AppendLine("      toolbar: [");
                sb.AppendLine("        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],");
                sb.AppendLine("        ['bold', 'italic', 'underline', 'strike'],");
                sb.AppendLine("        [{ 'list': 'ordered'}, { 'list': 'bullet' }],");
                sb.AppendLine("        [{ 'color': [] }, { 'background': [] }],");
                sb.AppendLine("        ['clean']");
                sb.AppendLine("      ]");
                sb.AppendLine("    }");
                sb.AppendLine("  });");
                sb.AppendLine("  ");
                sb.AppendLine($"  const fileContent = {Newtonsoft.Json.JsonConvert.SerializeObject(string.Join("\n", fileContent))};");
                sb.AppendLine("  quill.clipboard.dangerouslyPasteHTML(fileContent);");
                sb.AppendLine("  quill.history.clear();");
            }
            
            sb.AppendLine("  // Search functionality");
            sb.AppendLine("  const searchInput = document.getElementById('search-input');");
            sb.AppendLine("  const searchButton = document.getElementById('search-button');");
            sb.AppendLine("  const searchResults = document.getElementById('search-results');");
            sb.AppendLine("  ");
            sb.AppendLine("  function performSearch() {");
            sb.AppendLine("    const query = searchInput.value.toLowerCase();");
            sb.AppendLine("    if (!query) {");
            sb.AppendLine("      searchResults.innerHTML = '';");
            sb.AppendLine("      return;");
            sb.AppendLine("    }");
            sb.AppendLine("    ");
            sb.AppendLine("    const lines = fileContent.split('\\n');");
            sb.AppendLine("    const matches = [];");
            sb.AppendLine("    ");
            sb.AppendLine("    lines.forEach((line, index) => {");
            sb.AppendLine("      if (line.toLowerCase().includes(query)) {");
            sb.AppendLine("        matches.push({ line: index + 1, text: line, index });");
            sb.AppendLine("      }");
            sb.AppendLine("    });");
            sb.AppendLine("    ");
            sb.AppendLine("    searchResults.innerHTML = '';");
            sb.AppendLine("    if (matches.length === 0) {");
            sb.AppendLine("      searchResults.innerHTML = '<div class=\"p-2 text-muted\">No results found</div>';");
            sb.AppendLine("      return;");
            sb.AppendLine("    }");
            sb.AppendLine("    ");
            sb.AppendLine("    matches.forEach(match => {");
            sb.AppendLine("      const resultItem = document.createElement('div');");
            sb.AppendLine("      resultItem.className = 'search-result';");
            sb.AppendLine("      resultItem.textContent = `Line ${match.line}: ${match.text.substring(0, 40)}${match.text.length > 40 ? '...' : ''}`;");
            sb.AppendLine("      resultItem.addEventListener('click', () => highlightLine(match.index));");
            sb.AppendLine("      searchResults.appendChild(resultItem);");
            sb.AppendLine("    });");
            sb.AppendLine("  }");
            sb.AppendLine("  ");
            sb.AppendLine("  function highlightLine(lineIndex) {");
            if (fileExtension == ".cs" || fileExtension == ".js" || fileExtension == ".html" || fileExtension == ".css" || fileExtension == ".txt" || fileExtension == ".xml" || fileExtension == ".json")
            {
                sb.AppendLine("    const lineElements = lineNumbers.querySelectorAll('div');");
                sb.AppendLine("    lineElements.forEach(el => el.classList.remove('highlighted'));");
                sb.AppendLine("    lineElements[lineIndex].classList.add('highlighted');");
                sb.AppendLine("    lineElements[lineIndex].scrollIntoView({ behavior: 'smooth', block: 'center' });");
            }
            else
            {
                sb.AppendLine("    const lines = fileContent.split('\\n');");
                sb.AppendLine("    let pos = 0;");
                sb.AppendLine("    for (let i = 0; i < lineIndex; i++) {");
                sb.AppendLine("      pos += lines[i].length + 1;");
                sb.AppendLine("    }");
                sb.AppendLine("    quill.setSelection(pos, lines[lineIndex].length);");
            }
            sb.AppendLine("  }");
            sb.AppendLine("  ");
            sb.AppendLine("  searchButton.addEventListener('click', performSearch);");
            sb.AppendLine("  searchInput.addEventListener('keyup', function(event) {");
            sb.AppendLine("    if (event.key === 'Enter') {");
            sb.AppendLine("      performSearch();");
            sb.AppendLine("    }");
            sb.AppendLine("  });");
            
            sb.AppendLine("  // Button actions");
            sb.AppendLine("  document.getElementById('back-button').addEventListener('click', function() {");
            sb.AppendLine("    window.location.href = window.location.href.substring(0, window.location.href.lastIndexOf('/'));");
            sb.AppendLine("  });");
            
            sb.AppendLine("  document.getElementById('open-editor-button').addEventListener('click', function() {");
            sb.AppendLine("    // Implementation for opening in external editor");
            sb.AppendLine("    console.log('Opening in external editor');");
            sb.AppendLine("  });");
            
            sb.AppendLine("  document.getElementById('copy-button').addEventListener('click', function() {");
            sb.AppendLine("    navigator.clipboard.writeText(fileContent)");
            sb.AppendLine("      .then(() => {");
            sb.AppendLine("        const statusIndicator = document.getElementById('status-indicator');");
            sb.AppendLine("        statusIndicator.textContent = 'Copied to clipboard!';");
            sb.AppendLine("        setTimeout(() => { statusIndicator.textContent = 'Ready'; }, 2000);");
            sb.AppendLine("      });");
            sb.AppendLine("  });");
            
            sb.AppendLine("  document.getElementById('download-button').addEventListener('click', function() {");
            sb.AppendLine($"    const a = document.createElement('a');");
            sb.AppendLine($"    const blob = new Blob([fileContent], {{type: 'text/plain'}});");
            sb.AppendLine($"    a.href = URL.createObjectURL(blob);");
            sb.AppendLine($"    a.download = '{info.Name}';");
            sb.AppendLine($"    a.click();");
            sb.AppendLine("  });");
            
            sb.AppendLine("});");
            sb.AppendLine("</script>");
            
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private string GetLanguageFromExtension(string extension)
        {
            switch (extension)
            {
                case ".cs": return "csharp";
                case ".js": return "javascript";
                case ".html": return "html";
                case ".css": return "css";
                case ".xml": return "xml";
                case ".json": return "json";
                default: return "plaintext";
            }
        }

        public string Generate(string reqDirectory, string serverName)
        {
            var sb = new StringBuilder();
            var currentDirectory = new DirectoryInfo(reqDirectory);
            string cleanDirPath = reqDirectory.Replace("C:\\LILO\\", "");

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"en\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"UTF-8\">");
            sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine($"<title>Index of {cleanDirPath}</title>");
            sb.AppendLine("<link rel=\"icon\" type=\"image/png\" sizes=\"32x32\" href=\"/images/favlogo.png\">");
            sb.AppendLine("<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css\" rel=\"stylesheet\">");
            sb.AppendLine("<link href=\"https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css\" rel=\"stylesheet\">");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"/css/styles.css\">");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"/css/winui-style.css\">");
            sb.AppendLine("<link href=\"https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap\" rel=\"stylesheet\">");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: 'Inter', sans-serif; background-color: #f8f9fa; margin: 0; padding: 0; color: #212529; }");
            sb.AppendLine("* { box-sizing: border-box; }");
            sb.AppendLine(".app-container { display: flex; flex-direction: column; min-height: 100vh; }");
            sb.AppendLine(".header { background: linear-gradient(135deg, #0d6efd, #0a58ca); color: white; padding: 1rem; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }");
            sb.AppendLine(".header-content { display: flex; justify-content: space-between; align-items: center; max-width: 1200px; margin: 0 auto; }");
            sb.AppendLine(".app-title { margin: 0; font-size: 1.25rem; font-weight: 600; }");
            sb.AppendLine(".app-name-subtitle { font-size: 0.75rem; opacity: 0.8; margin-left: 0.5rem; font-weight: normal; }");
            sb.AppendLine(".header-right { display: flex; align-items: center; gap: 1rem; }");
            sb.AppendLine(".search-bar { position: relative; display: flex; align-items: center; }");
            sb.AppendLine(".search-bar input { padding: 0.5rem 1rem 0.5rem 2.25rem; border: none; border-radius: 4px; width: 250px; font-size: 0.875rem; }");
            sb.AppendLine(".search-bar i { position: absolute; left: 0.75rem; color: #6c757d; }");
            sb.AppendLine(".btn-group { display: flex; gap: 0.25rem; }");
            sb.AppendLine(".btn { display: inline-flex; align-items: center; justify-content: center; padding: 0.5rem; border-radius: 0.25rem; border: none; cursor: pointer; transition: all 0.2s ease; }");
            sb.AppendLine(".btn-primary { background-color: #0d6efd; color: white; }");
            sb.AppendLine(".btn-primary:hover { background-color: #0a58ca; }");
            sb.AppendLine(".btn-secondary { background-color: rgba(255,255,255,0.2); color: white; }");
            sb.AppendLine(".btn-secondary:hover { background-color: rgba(255,255,255,0.3); }");
            sb.AppendLine(".btn i { font-size: 1rem; }");
            sb.AppendLine(".main-content { flex: 1; padding: 1.5rem; max-width: 1200px; margin: 0 auto; width: 100%; }");
            sb.AppendLine(".breadcrumb-section { background-color: white; border-radius: 0.5rem; box-shadow: 0 2px 8px rgba(0,0,0,0.05); padding: 0.75rem 1rem; margin-bottom: 1rem; }");
            sb.AppendLine(".breadcrumb { display: flex; flex-wrap: wrap; gap: 0.5rem; list-style: none; margin: 0; padding: 0; font-size: 0.875rem; }");
            sb.AppendLine(".breadcrumb-item { display: flex; align-items: center; }");
            sb.AppendLine(".breadcrumb-item:not(:last-child)::after { content: '/'; margin-left: 0.5rem; color: #6c757d; }");
            sb.AppendLine(".breadcrumb-item a { color: #0d6efd; text-decoration: none; transition: color 0.15s ease; }");
            sb.AppendLine(".breadcrumb-item a:hover { color: #0a58ca; text-decoration: underline; }");
            sb.AppendLine(".card { background-color: white; border-radius: 0.5rem; box-shadow: 0 2px 8px rgba(0,0,0,0.05); overflow: hidden; }");
            sb.AppendLine(".card-header { padding: 1rem; border-bottom: 1px solid rgba(0,0,0,0.05); display: flex; justify-content: space-between; align-items: center; }");
            sb.AppendLine(".view-options { display: flex; gap: 0.5rem; }");
            sb.AppendLine(".view-btn { background-color: transparent; border: 1px solid #dee2e6; color: #6c757d; padding: 0.25rem 0.5rem; border-radius: 0.25rem; cursor: pointer; transition: all 0.15s ease; }");
            sb.AppendLine(".view-btn.active { background-color: #f1f3f5; border-color: #ced4da; color: #212529; }");
            sb.AppendLine(".view-btn:hover { background-color: #f8f9fa; }");
            sb.AppendLine(".table { width: 100%; border-collapse: collapse; }");
            sb.AppendLine(".table th { padding: 0.75rem 1rem; text-align: left; font-weight: 500; color: #495057; border-bottom: 1px solid #e9ecef; font-size: 0.875rem; }");
            sb.AppendLine(".table td { padding: 0.75rem 1rem; border-bottom: 1px solid #e9ecef; font-size: 0.875rem; vertical-align: middle; }");
            sb.AppendLine(".table tr:last-child td { border-bottom: none; }");
            sb.AppendLine(".table tr:hover td { background-color: #f8f9fa; }");
            sb.AppendLine(".file-name { display: flex; align-items: center; gap: 0.5rem; }");
            sb.AppendLine(".file-icon { font-size: 1.25rem; display: flex; align-items: center; justify-content: center; width: 1.75rem; }");
            sb.AppendLine(".folder-icon { color: #ffc107; }");
            sb.AppendLine(".file-link { color: #212529; text-decoration: none; display: flex; align-items: center; }");
            sb.AppendLine(".file-link:hover { color: #0d6efd; }");
            sb.AppendLine(".file-ext { margin-left: 0.25rem; font-size: 0.75rem; color: #6c757d; }");
            sb.AppendLine(".file-size { color: #6c757d; }");
            sb.AppendLine(".file-date { color: #6c757d; }");
            sb.AppendLine(".file-actions { display: flex; gap: 0.25rem; }");
            sb.AppendLine(".file-action-btn { background-color: transparent; border: none; color: #6c757d; padding: 0.25rem; border-radius: 0.25rem; cursor: pointer; transition: all 0.15s ease; }");
            sb.AppendLine(".file-action-btn:hover { background-color: #f1f3f5; color: #0d6efd; }");
            sb.AppendLine(".no-items { padding: 2rem; text-align: center; color: #6c757d; }");
            sb.AppendLine(".grid-view { display: grid; grid-template-columns: repeat(auto-fill, minmax(180px, 1fr)); gap: 1rem; padding: 1rem; }");
            sb.AppendLine(".grid-item { background-color: white; border-radius: 0.5rem; overflow: hidden; transition: all 0.2s ease; border: 1px solid #e9ecef; display: flex; flex-direction: column; height: 100%; }");
            sb.AppendLine(".grid-item:hover { transform: translateY(-3px); box-shadow: 0 5px 15px rgba(0,0,0,0.08); border-color: #dee2e6; }");
            sb.AppendLine(".grid-item-icon { display: flex; align-items: center; justify-content: center; padding: 1.5rem; font-size: 2rem; background-color: #f8f9fa; }");
            sb.AppendLine(".grid-item-content { padding: 0.75rem; display: flex; flex-direction: column; flex: 1; }");
            sb.AppendLine(".grid-item-name { font-weight: 500; margin-bottom: 0.25rem; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }");
            sb.AppendLine(".grid-item-info { font-size: 0.75rem; color: #6c757d; margin-top: auto; }");
            sb.AppendLine(".empty-state { display: flex; flex-direction: column; align-items: center; justify-content: center; padding: 3rem 1rem; }");
            sb.AppendLine(".empty-state-icon { font-size: 3rem; color: #ced4da; margin-bottom: 1rem; }");
            sb.AppendLine(".empty-state-message { color: #6c757d; margin-bottom: 1.5rem; text-align: center; }");
            sb.AppendLine(".footer { background-color: white; padding: 1rem; text-align: center; font-size: 0.75rem; color: #6c757d; border-top: 1px solid #e9ecef; }");
            sb.AppendLine("@media (max-width: 768px) { .search-bar input { width: 180px; } .table th:nth-child(3), .table td:nth-child(3) { display: none; } }");
            sb.AppendLine("@media (max-width: 576px) { .search-bar { display: none; } .header-content { flex-direction: column; align-items: flex-start; gap: 0.5rem; } .header-right { width: 100%; justify-content: space-between; } }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            
            sb.AppendLine("<div class=\"app-container\">");
            
            // Header
            sb.AppendLine("  <header class=\"header\">");
            sb.AppendLine("    <div class=\"header-content\">");
            sb.AppendLine($"      <h1 class=\"app-title\">{serverName}<span class=\"app-name-subtitle\">({cleanDirPath})</span></h1>");
            sb.AppendLine("      <div class=\"header-right\">");
            sb.AppendLine("        <div class=\"search-bar\">");
            sb.AppendLine("          <i class=\"bi bi-search\"></i>");
            sb.AppendLine("          <input type=\"text\" id=\"searchInput\" placeholder=\"Search files...\">");
            sb.AppendLine("        </div>");
            sb.AppendLine("        <div class=\"btn-group\">");
            sb.AppendLine("          <button class=\"btn btn-secondary\" id=\"addFileButton\" title=\"Add File\"><i class=\"bi bi-plus-lg\"></i></button>");
            sb.AppendLine("          <button class=\"btn btn-secondary\" id=\"settingsButton\" title=\"Settings\"><i class=\"bi bi-gear\"></i></button>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </header>");
            
            // Main content
            sb.AppendLine("  <div class=\"main-content\">");
            
            // Breadcrumb
            sb.AppendLine("    <nav class=\"breadcrumb-section\">");
            sb.AppendLine("      <ol class=\"breadcrumb\">");
            sb.AppendLine("        <li class=\"breadcrumb-item\"><a href=\"/\"><i class=\"bi bi-house-door\"></i> Home</a></li>");
            
            var directories = currentDirectory.FullName.Split(Path.DirectorySeparatorChar);
            string path = string.Empty;
            foreach (var directory in directories)
            {
                if (!string.IsNullOrEmpty(directory))
                {
                    path += directory + Path.DirectorySeparatorChar;
                    if (directory.Equals("LILO", StringComparison.OrdinalIgnoreCase))
                        continue;
                    
                    sb.AppendLine($"        <li class=\"breadcrumb-item\"><a href=\"/{path}\">{directory}</a></li>");
                }
            }
            
            sb.AppendLine("      </ol>");
            sb.AppendLine("    </nav>");
            
            // Files list
            sb.AppendLine("    <div class=\"card\">");
            sb.AppendLine("      <div class=\"card-header\">");
            sb.AppendLine("        <div>Files and Folders</div>");
            sb.AppendLine("        <div class=\"view-options\">");
            sb.AppendLine("          <button class=\"view-btn active\" id=\"list-view-btn\"><i class=\"bi bi-list\"></i> List</button>");
            sb.AppendLine("          <button class=\"view-btn\" id=\"grid-view-btn\"><i class=\"bi bi-grid\"></i> Grid</button>");
            sb.AppendLine("        </div>");
            sb.AppendLine("      </div>");
            
            // List view (default)
            sb.AppendLine("      <div id=\"list-view\">");
            
            // Add table with files and directories
            sb.AppendLine("        <table class=\"table\">");
            sb.AppendLine("          <thead>");
            sb.AppendLine("            <tr>");
            sb.AppendLine("              <th>Name</th>");
            sb.AppendLine("              <th style=\"width: 100px;\">Size</th>");
            sb.AppendLine("              <th style=\"width: 180px;\">Last Modified</th>");
            sb.AppendLine("              <th style=\"width: 100px;\">Actions</th>");
            sb.AppendLine("            </tr>");
            sb.AppendLine("          </thead>");
            sb.AppendLine("          <tbody>");
            
            var parentDirectory = currentDirectory.Parent;
            
            if (parentDirectory != null)
            {
                sb.AppendLine("            <tr>");
                sb.AppendLine("              <td>");
                sb.AppendLine("                <div class=\"file-name\">");
                sb.AppendLine("                  <div class=\"file-icon\"><i class=\"bi bi-arrow-up\"></i></div>");
                sb.AppendLine("                  <a href=\"../\" class=\"file-link\">Parent Directory</a>");
                sb.AppendLine("                </div>");
                sb.AppendLine("              </td>");
                sb.AppendLine("              <td></td>");
                sb.AppendLine("              <td></td>");
                sb.AppendLine("              <td></td>");
                sb.AppendLine("            </tr>");
            }
            
            var directories_list = currentDirectory.GetDirectories();
            var files_list = currentDirectory.GetFiles();
            
            if (directories_list.Length == 0 && files_list.Length == 0)
            {
                sb.AppendLine("            <tr>");
                sb.AppendLine("              <td colspan=\"4\">");
                sb.AppendLine("                <div class=\"no-items\">");
                sb.AppendLine("                  <i class=\"bi bi-folder-x mb-3\" style=\"font-size: 2rem;\"></i>");
                sb.AppendLine("                  <p>This folder is empty</p>");
                sb.AppendLine("                </div>");
                sb.AppendLine("              </td>");
                sb.AppendLine("            </tr>");
            }
            else
            {
                foreach (var directory in directories_list)
                {
                    sb.AppendLine("            <tr>");
                    sb.AppendLine("              <td>");
                    sb.AppendLine("                <div class=\"file-name\">");
                    sb.AppendLine("                  <div class=\"file-icon folder-icon\"><i class=\"bi bi-folder\"></i></div>");
                    sb.AppendLine($"                  <a href=\"{directory.Name}/\" class=\"file-link\">{directory.Name}</a>");
                    sb.AppendLine("                </div>");
                    sb.AppendLine("              </td>");
                    sb.AppendLine("              <td><span class=\"file-size\">-</span></td>");
                    sb.AppendLine($"              <td><span class=\"file-date\">{directory.LastWriteTime.ToString("yyyy-MM-dd HH:mm")}</span></td>");
                    sb.AppendLine("              <td class=\"file-actions\">");
                    sb.AppendLine($"                <button class=\"file-action-btn\" title=\"Open folder\" onclick=\"window.location.href='{directory.Name}/'\"><i class=\"bi bi-folder2-open\"></i></button>");
                    sb.AppendLine("              </td>");
                    sb.AppendLine("            </tr>");
                }
                
                foreach (var file in files_list)
                {
                    string fileExtension = Path.GetExtension(file.Name).TrimStart('.').ToLower();
                    string fileIcon = GetFileIcon(file.Name);
                    
                    sb.AppendLine("            <tr>");
                    sb.AppendLine("              <td>");
                    sb.AppendLine("                <div class=\"file-name\">");
                    sb.AppendLine($"                  <div class=\"file-icon\"><i class=\"bi bi-{fileIcon}\"></i></div>");
                    sb.AppendLine($"                  <a href=\"{file.Name}\" class=\"file-link\">{Path.GetFileNameWithoutExtension(file.Name)}<span class=\"file-ext\">.{fileExtension}</span></a>");
                    sb.AppendLine("                </div>");
                    sb.AppendLine("              </td>");
                    sb.AppendLine($"              <td><span class=\"file-size\">{GetSizeString(file.Length)}</span></td>");
                    sb.AppendLine($"              <td><span class=\"file-date\">{file.LastWriteTime.ToString("yyyy-MM-dd HH:mm")}</span></td>");
                    sb.AppendLine("              <td class=\"file-actions\">");
                    sb.AppendLine($"                <a class=\"file-action-btn\" href=\"{file.Name}\" title=\"Open file\"><i class=\"bi bi-box-arrow-up-right\"></i></a>");
                    sb.AppendLine($"                <a class=\"file-action-btn\" href=\"{file.Name}\" download title=\"Download\"><i class=\"bi bi-download\"></i></a>");
                    sb.AppendLine("              </td>");
                    sb.AppendLine("            </tr>");
                }
            }
            
            sb.AppendLine("          </tbody>");
            sb.AppendLine("        </table>");
            sb.AppendLine("      </div>");
            
            // Grid view (initially hidden)
            sb.AppendLine("      <div id=\"grid-view\" style=\"display: none;\" class=\"grid-view\">");
            
            if (directories_list.Length == 0 && files_list.Length == 0)
            {
                sb.AppendLine("        <div class=\"empty-state\">");
                sb.AppendLine("          <i class=\"bi bi-folder-x empty-state-icon\"></i>");
                sb.AppendLine("          <p class=\"empty-state-message\">This folder is empty</p>");
                sb.AppendLine("        </div>");
            }
            else
            {
                if (parentDirectory != null)
                {
                    sb.AppendLine("        <a href=\"../\" class=\"grid-item\">");
                    sb.AppendLine("          <div class=\"grid-item-icon\">");
                    sb.AppendLine("            <i class=\"bi bi-arrow-up\"></i>");
                    sb.AppendLine("          </div>");
                    sb.AppendLine("          <div class=\"grid-item-content\">");
                    sb.AppendLine("            <div class=\"grid-item-name\">Parent Directory</div>");
                    sb.AppendLine("          </div>");
                    sb.AppendLine("        </a>");
                }
                
                foreach (var directory in directories_list)
                {
                    sb.AppendLine($"        <a href=\"{directory.Name}/\" class=\"grid-item\">");
                    sb.AppendLine("          <div class=\"grid-item-icon\">");
                    sb.AppendLine("            <i class=\"bi bi-folder folder-icon\"></i>");
                    sb.AppendLine("          </div>");
                    sb.AppendLine("          <div class=\"grid-item-content\">");
                    sb.AppendLine($"            <div class=\"grid-item-name\">{directory.Name}</div>");
                    sb.AppendLine($"            <div class=\"grid-item-info\">{directory.LastWriteTime.ToString("yyyy-MM-dd")}</div>");
                    sb.AppendLine("          </div>");
                    sb.AppendLine("        </a>");
                }
                
                foreach (var file in files_list)
                {
                    string fileIcon = GetFileIcon(file.Name);
                    
                    sb.AppendLine($"        <a href=\"{file.Name}\" class=\"grid-item\">");
                    sb.AppendLine("          <div class=\"grid-item-icon\">");
                    sb.AppendLine($"            <i class=\"bi bi-{fileIcon}\"></i>");
                    sb.AppendLine("          </div>");
                    sb.AppendLine("          <div class=\"grid-item-content\">");
                    sb.AppendLine($"            <div class=\"grid-item-name\">{file.Name}</div>");
                    sb.AppendLine($"            <div class=\"grid-item-info\">{GetSizeString(file.Length)} • {file.LastWriteTime.ToString("yyyy-MM-dd")}</div>");
                    sb.AppendLine("          </div>");
                    sb.AppendLine("        </a>");
                }
            }
            
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            
            // Footer
            sb.AppendLine("  <footer class=\"footer\">");
            sb.AppendLine($"    <div>&copy; {DateTime.Now.Year} LILO-WebEngine | All rights reserved</div>");
            sb.AppendLine("  </footer>");
            
            sb.AppendLine("</div>");
            
            // Scripts
            sb.AppendLine("<script>");
            sb.AppendLine("document.addEventListener('DOMContentLoaded', function() {");
            sb.AppendLine("  // View switching");
            sb.AppendLine("  const listViewBtn = document.getElementById('list-view-btn');");
            sb.AppendLine("  const gridViewBtn = document.getElementById('grid-view-btn');");
            sb.AppendLine("  const listView = document.getElementById('list-view');");
            sb.AppendLine("  const gridView = document.getElementById('grid-view');");
            sb.AppendLine("  ");
            sb.AppendLine("  listViewBtn.addEventListener('click', function() {");
            sb.AppendLine("    listView.style.display = 'block';");
            sb.AppendLine("    gridView.style.display = 'none';");
            sb.AppendLine("    listViewBtn.classList.add('active');");
            sb.AppendLine("    gridViewBtn.classList.remove('active');");
            sb.AppendLine("    localStorage.setItem('preferredView', 'list');");
            sb.AppendLine("  });");
            sb.AppendLine("  ");
            sb.AppendLine("  gridViewBtn.addEventListener('click', function() {");
            sb.AppendLine("    listView.style.display = 'none';");
            sb.AppendLine("    gridView.style.display = 'grid';");
            sb.AppendLine("    gridViewBtn.classList.add('active');");
            sb.AppendLine("    listViewBtn.classList.remove('active');");
            sb.AppendLine("    localStorage.setItem('preferredView', 'grid');");
            sb.AppendLine("  });");
            sb.AppendLine("  ");
            sb.AppendLine("  // Load user's preferred view from localStorage");
            sb.AppendLine("  const preferredView = localStorage.getItem('preferredView');");
            sb.AppendLine("  if (preferredView === 'grid') {");
            sb.AppendLine("    gridViewBtn.click();");
            sb.AppendLine("  }");
            sb.AppendLine("  ");
            sb.AppendLine("  // Search functionality");
            sb.AppendLine("  const searchInput = document.getElementById('searchInput');");
            sb.AppendLine("  searchInput.addEventListener('input', function() {");
            sb.AppendLine("    const query = this.value.toLowerCase();");
            sb.AppendLine("    const listItems = document.querySelectorAll('#list-view tbody tr');");
            sb.AppendLine("    const gridItems = document.querySelectorAll('#grid-view .grid-item');");
            sb.AppendLine("    ");
            sb.AppendLine("    // Filter list view");
            sb.AppendLine("    listItems.forEach(item => {");
            sb.AppendLine("      const fileName = item.querySelector('.file-link').textContent.toLowerCase();");
            sb.AppendLine("      if (fileName.includes(query) || fileName === 'parent directory') {");
            sb.AppendLine("        item.style.display = '';");
            sb.AppendLine("      } else {");
            sb.AppendLine("        item.style.display = 'none';");
            sb.AppendLine("      }");
            sb.AppendLine("    });");
            sb.AppendLine("    ");
            sb.AppendLine("    // Filter grid view");
            sb.AppendLine("    gridItems.forEach(item => {");
            sb.AppendLine("      const fileName = item.querySelector('.grid-item-name').textContent.toLowerCase();");
            sb.AppendLine("      if (fileName.includes(query) || fileName === 'parent directory') {");
            sb.AppendLine("        item.style.display = '';");
            sb.AppendLine("      } else {");
            sb.AppendLine("        item.style.display = 'none';");
            sb.AppendLine("      }");
            sb.AppendLine("    });");
            sb.AppendLine("  });");
            sb.AppendLine("  ");
            sb.AppendLine("  // Add File button functionality");
            sb.AppendLine("  document.getElementById('addFileButton').addEventListener('click', function() {");
            sb.AppendLine("    alert('File upload functionality will be implemented here');");
            sb.AppendLine("  });");
            sb.AppendLine("});");
            sb.AppendLine("</script>");
            
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            
            return sb.ToString();
        }

        public Task<string> GenerateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
