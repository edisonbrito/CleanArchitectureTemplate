using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Praxio.Tools.Arquitetura.Api.Response;
using Praxio.Tools.Arquitetura.Infra.CrossCutting.AutoMapper;
using Praxio.Tools.Arquitetura.Infra.CrossCutting.IoC;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace Praxio.Tools.Arquitetura.Api
{
    public class Startup
    {
        private const string Version = "1";
        private const string ProjectTitleName = "Praxio.Tools.Arquitetura.Api";
        private readonly IConfigurationRoot configurationRoot;

        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", true, true);

            configurationRoot = builder.Build();

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.AddAutoMapper(config =>
            {
                config.ForAllMaps((map, expression) =>
                {
                    foreach (var unmappedPropertyName in map.GetUnmappedPropertyNames())
                        expression.ForMember(unmappedPropertyName,
                            configurationExpression => configurationExpression.Ignore());
                });

                config.AddProfiles(typeof(ApplicationProfile).Assembly);
            });

            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            ConfigureSwagger(services);
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.DescribeAllEnumsAsStrings();
                s.DescribeStringEnumsInCamelCase();
                s.DescribeAllParametersInCamelCase();
                s.SwaggerDoc($"v{Version}", new Info
                {
                    Version = $"v{Version}",
                    Title = ProjectTitleName,
                    Description = "API Swagger surface",

                    TermsOfService = "TermsOfService",
                    Contact = new Contact
                    {
                        Name = "Contact_Name",
                        Email = "Contact_Email",
                        Url = "Url"
                    },
                });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{ProjectTitleName}.xml");
                s.IncludeXmlComments(filePath);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint($"/swagger/v{Version}/swagger.json", "Praxio API");
            });

            app.UseStaticFiles();            
            app.UseMvcWithDefaultRoute();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var connectionString = configurationRoot.GetConnectionString("DefaultConnection");

            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new InfraModule(connectionString));    

            builder.RegisterType<Presenter>();            
        }
    }
}