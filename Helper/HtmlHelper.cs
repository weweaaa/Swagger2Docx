using Microsoft.OpenApi.Models;
using Razor.Templating.Core;

namespace Swagger2Docx.Helpers
{
    public static class HtmlHelper
    {
        /// <summary>
        /// 將model以Razor渲染成網頁字串
        /// </summary>
        /// <param name="templatePath">cshtml路徑</param>
        /// <param name="model">要渲染的swagger model</param>
        /// <returns></returns>
        public static async Task<string> GeneritorSwaggerHtmlAsync(string templatePath, OpenApiDocument model)
        {
            var html = await RazorTemplateEngine.RenderAsync(templatePath, model);
            return html;
        }
    }
}
