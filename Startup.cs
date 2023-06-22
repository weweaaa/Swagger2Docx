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
            services.AddScoped<SwaggerGenerator>(); //�`�JSwaggerGenerator,�᭱�i�H�����ϥγo�Ӥ�k
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",   //���� 
                    Title = "Swagger2Docx",  //���D
                    Description = $"���ئW�� Http API v1",    //�y�z 
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//������ε{���Ҧb�ؿ��]����A�����u�@�ؿ��v�T�A��ĳ�ĥΦ���k������|�^
                                                                                        //var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "docTemp.xml"); //�o�ӴN�O���t�m��xml�ɮצW
                                                                 // c.IncludeXmlComments(xmlPath);//�q�{���ĤG�ӰѼƬOfalse,���k���`��
                c.IncludeXmlComments(xmlPath, true); // �o�ӬOcontroller���`��



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

                #region Swagger �u�b�}�o���`���ϥ�
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    //c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Swagger2Docx v1");
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger2Docx v1");
                    //c.RoutePrefix = string.Empty;     //�p�G�O���� �X�ݸ��|�N�� �ڥ\���ܼƦW��/index.html,�`�Nlocalhost:8001/swagger�O�X�ݤ��쪺
                                                      //���|�t�m�A�]�m���šA��ܪ����b�ڥ\���ܼƦW�١]localhost:8001�^�X�ݸ���
                                                      // c.RoutePrefix = "swagger"; // �p�G�A�Q���@�Ӹ��|�A�����g�W�r�Y�i�A��p�����gc.RoutePrefix = "swagger"; �h�X�ݸ��|�� �ڥ\���ܼƦW��/swagger/index.html

                    c.DocumentTitle = "���ئW�� �u�W���ɽո�";
                    #region �ۭq�˦�

                    //css �`�J
                    c.InjectStylesheet("/css/swaggerdoc.css");
                    c.InjectStylesheet("/css/app.min.css");
                    //js �`�J
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
