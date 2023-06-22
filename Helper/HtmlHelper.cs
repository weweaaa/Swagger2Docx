using Microsoft.OpenApi.Models;
using RazorEngine;
using RazorEngine.Templating;

namespace Swagger2Docx.Helper
{
    public class HtmlHelper
    {
        /// <summary>
        /// 將數據遍利靜態頁面中
        /// </summary>
        /// <param name="templatePath">靜態頁面路徑</param>
        /// <param name="model">獲取到的文件數據</param>
        /// <returns></returns>
        public static string GeneritorSwaggerHtml(string templatePath, OpenApiDocument model)
        {
            var template = System.IO.File.ReadAllText(templatePath);
            var result = Engine.Razor.RunCompile(template, "i3yuan", typeof(OpenApiDocument), model);
            return result;
        }
    }

}
