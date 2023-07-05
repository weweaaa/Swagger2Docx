using Swagger2Docx.Helpers;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swagger2Docx.Endpoints
{
    public static class SwaggerDocEndpoints
    {
        public static void MapSwaggerDocEndpointsAsync(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/swagger-doc").WithTags("SwaggerDoc").WithGroupName("Swagger Doc");
            group.MapGet("/ExportWord", [AllowAnonymous] async Task<IResult> (IWebHostEnvironment _hostingEnvironment, 
                SwaggerGenerator _swaggerGenerator,
                DocHelper _docHelper,
                string type, string version) => 
            {
                string contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //1. 根據指定版本獲取指定版本的json物件。
                var model = _swaggerGenerator.GetSwagger(version);
                //2. 根據範本引擎生成html
                var html = await HtmlHelper.GeneritorSwaggerHtmlAsync($"wwwroot/SwaggerDoc.cshtml", model);
                //3. 將html轉成word
                using var memoryStream = new MemoryStream();
                _docHelper.HtmlToWord(html, memoryStream);
                memoryStream.Position = 0;

                return Results.File(memoryStream.ToArray(), contenttype, $"SwaggerDoc_{DateTime.Now:yyyyMMddHHmmss}.docx"); 
            });

        }
    }
}