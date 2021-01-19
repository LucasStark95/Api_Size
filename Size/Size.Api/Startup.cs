using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Size.Api.Helpers;
using Size.Core.Models;
using Size.Data.EFCore.Context;
using Size.Data.EFCore.Helpers;
using Size.Data.EFCore.Repositorios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace Size.Api
{
    /// <summary>
    /// Statup.
    /// Classe Responsavel pela inicialização da API.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configurações da aplicação.
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// Variável de ambiente.
        /// </summary>
        public IWebHostEnvironment CurrentEnvironment { get; }

        /// <summary>
        /// COnstrutor da Classe.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }
        /// <summary>
        /// Método de definições e configurações dos serviços.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services = ConfigureServer(services);
            services = ConfigureData(services);
            services = ConfigureAppSettings(services);
           
            services.AddHttpClient();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Documentação API",
                    Version = "v1",
                    Description = "API para prova de avaliação técnica.",
                    Contact = new OpenApiContact
                    {
                        Name = "Antonio Lucas de Almeida",
                        Url = new Uri("https://github.com/LucasStark95")
                    }

                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);


            });

            services.AddControllers()
                .AddNewtonsoftJson()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = _ =>
                    {
                        var result = new BadRequestObjectResult("Os valores informados não são válidos.");
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        result.StatusCode = (int)HttpStatusCode.PreconditionFailed;

                        return result;
                    };
                });
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["ApiSettings:Issuer"],
                    ValidAudience = Configuration["ApiSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ApiSettings:Secret"]))
                };
            });
        }

        /// <summary>
        /// Método de definições e configurações do APP.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("SiteCorsPolicy");
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Documentação Api");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IServiceCollection ConfigureServer(IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            return services;
        }

        private IServiceCollection ConfigureAppSettings(IServiceCollection services)
        {
            var appSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var appSettingsSection = appSettings.GetSection("ApiSettings");

            services.Configure<ApiSettings>(appSettingsSection);

            return services;
        }

        private IServiceCollection ConfigureData(IServiceCollection services)
        {
            if (CurrentEnvironment.IsEnvironment("Test"))
                services.AddDbContext<Contexto>(options =>
                    options.UseInMemoryDatabase(databaseName: "Size"));
            else
            {
                var connectionString = ConnectionHelper.GetConnectionString();
                services.AddDbContext<Contexto>(options => options.UseNpgsql(connectionString));
            }

            var dataServiceTypes = GetServicesTypes();
            foreach (var t in dataServiceTypes)
                services.AddScoped(t);

            return services;
        }

        private IEnumerable<Type> GetServicesTypes() => Assembly.GetAssembly(typeof(Contexto)).GetTypes()
                .Where(t => t != typeof(RepositorioBase<>) && InjectionHelper.IsSubclassOfRawGeneric(typeof(RepositorioBase<>), t));
    }
}
