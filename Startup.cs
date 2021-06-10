using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Data.MongoDB>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // ler o swagger de um arquivo xml(utiliza biblioteca System.Reflection)
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); // caminho do diretorio que está o swagger
                c.IncludeXmlComments(xmlPath); //Inclui os comentarios no Swagger
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Curso");// qual arquivo o swagger tem que ler?
                c.RoutePrefix = string.Empty; //swagger rota padrao
            });
        }
    }
}
