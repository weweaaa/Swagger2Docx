using Swagger2Docx.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swagger2Docx.Endpoints;

var builder = WebApplication.CreateBuilder(args);

//讓 swagger doc 可以產生文件
builder.Services.AddScoped<SwaggerGenerator>();
builder.Services.AddScoped<DocHelper>();

#region Swagger Group
List<string> apiGroup = new List<string> { "Weatherforecast" };
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    apiGroup.ForEach(item =>
    {
        options.SwaggerDoc(item, new() { Title = item });
    });
    
    options.CustomSchemaIds(x => x.FullName);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableTryItOutByDefault();
        apiGroup.ForEach(item =>
        {
            options.SwaggerEndpoint($"/swagger/{item}/swagger.json", item);
        });
        
        #region 自訂樣式

        //css 注入
        options.InjectStylesheet("/css/swaggerdoc.css");
        options.InjectStylesheet("/css/app.min.css");

        //js 注入
        options.InjectJavascript("/js/jquery.js");
        options.InjectJavascript("/js/app.min.js");
        options.InjectJavascript("/js/swaggerdoc.js");

        #endregion
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 非正式環境支援 swagger doc 匯出
if (app.Environment.IsProduction() == false)
{
    app.MapSwaggerDocEndpointsAsync();
}

app.MapWeatherforecastEndpoints();

app.Run();