using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Swagger2Docx.Helper;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swagger2Docx.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SwaggerController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly SwaggerGenerator _swaggerGenerator;

        private readonly SpireDocHelper _spireDocHelper;

        public SwaggerController(IWebHostEnvironment hostingEnvironment, SpireDocHelper spireDocHelper, SwaggerGenerator swaggerGenerator)
        {
            _webHostEnvironment = hostingEnvironment;
            _spireDocHelper = spireDocHelper;
            _swaggerGenerator = swaggerGenerator;
        }
        /// <summary>
        /// 匯出文件
        /// </summary>
        /// <param name="type">檔案類型</param>
        /// <param name="version">版本號V1</param>
        /// <returns></returns>
        [HttpGet]
        public FileResult ExportWord(string type, string version)
        {
            string contenttype = string.Empty;

            var model = _swaggerGenerator.GetSwagger(version); //1. 根據指定版本獲取指定版本的json物件。

            var html = HtmlHelper.GeneritorSwaggerHtml($"{_webHostEnvironment.WebRootPath}\\SwaggerDoc.cshtml", model); //2. 根據範本引擎生成html

            var op = _spireDocHelper.SwaggerConversHtml(html, type, out contenttype); //3.將html匯出檔案類型

            return File(op, contenttype, $"XUnit.Core介面文檔{type}");
        }
    }
}
