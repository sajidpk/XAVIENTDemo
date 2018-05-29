using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XAVIENTDemo.Repository;

namespace XAVIENTDemo
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x =>
            {
                x.ReturnHttpNotAcceptable = true;
                x.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            // AWS Options
            var awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            var client = awsOptions.CreateServiceClient<IAmazonDynamoDB>();
            
            services.AddAWSService<Amazon.DynamoDBv2.IAmazonDynamoDB>();

            // Add S3 to the ASP.NET Core dependency injection framework. 
            services.AddAWSService<Amazon.S3.IAmazonS3>();

            services.AddSingleton<IPlayersRepository, PlayersRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env , IPlayersRepository playerRepository)
        {
            if  (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBulider =>
                {
                    appBulider.Run(async context =>
                  {
                      context.Response.StatusCode = 500;
                      await context.Response.WriteAsync("Please try again");

                  });

                });
            }

            app.UseMvc();

            playerRepository.CreateTableAsync(false);
        }
    }
}
