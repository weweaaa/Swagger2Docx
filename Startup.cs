using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swagger2Docx.Helper;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger2Docx
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SwaggerGenerator>(); //注入SwaggerGenerator,後面可以直接使用這個方法
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",   //版本 
                    Title = "Swagger2Docx",  //標題
                    Description = $"項目名稱 Http API v1",    //描述 
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//獲取應用程式所在目錄（絕對，不受工作目錄影響，建議採用此方法獲取路徑）
                                                                                        //var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "docTemp.xml"); //這個就是剛剛配置的xml檔案名
                                                                 // c.IncludeXmlComments(xmlPath);//默認的第二個參數是false,對方法的注釋
                c.IncludeXmlComments(xmlPath, true); // 這個是controller的注釋



            });
            services.AddScoped<SpireDocHelper>();
            services.AddControllers();


            //services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger2Docx", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                #region Swagger 只在開發環節中使用
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    //c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Swagger2Docx v1");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger2Docx v1");
                    //c.RoutePrefix = string.Empty;     //如果是為空 訪問路徑就為 根功能變數名稱/index.html,注意localhost:8001/swagger是訪問不到的
                                                      //路徑配置，設置為空，表示直接在根功能變數名稱（localhost:8001）訪問該檔
                                                      // c.RoutePrefix = "swagger"; // 如果你想換一個路徑，直接寫名字即可，比如直接寫c.RoutePrefix = "swagger"; 則訪問路徑為 根功能變數名稱/swagger/index.html

                    c.DocumentTitle = "項目名稱 線上文檔調試";
                    #region 自訂樣式

                    //css 注入
                    c.InjectStylesheet("/css/swaggerdoc.css");
                    c.InjectStylesheet("/css/app.min.css");
                    //js 注入
                    c.InjectJavascript("/js/jquery.js");
                    c.InjectJavascript("/js/swaggerdoc.js");
                    c.InjectJavascript("/js/app.min.js");

                    #endregion

                });
                #endregion

            }
            app.UseRouting();

            //app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger2Docx v1"));
            //}

            //app.UseRouting();

            ////app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
