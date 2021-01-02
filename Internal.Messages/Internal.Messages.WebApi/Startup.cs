using System.IO;
using System.Reflection;
using Internal.Messages.Configuration;
using Internal.Messages.WebApi.Filters;
using Internal.Messages.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Internal.Messages.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds services for controllers to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
            // This method will not register services used for views or pages.
            services.AddControllers(setupAction =>
            {
                // Determines if a 406 response code (an unsupprted request response type) is returned
                // by the API when requested by the consumer.
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(setupAction =>
            {
                // For converting JSON values to Microsoft.AspNetCore.JsonPatch.JsonPatchDocument
                setupAction.SerializerSettings.ContractResolver =
                   new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlDataContractSerializerFormatters() // Adds the XML API response format, if requested. JSON is supported by default.
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = "modelvalidationproblem",
                        Title = "One or more model validation errors occurred.",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Detail = "See the errors property for details.",
                        Instance = context.HttpContext.Request.Path
                    };

                    problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

                    return new UnprocessableEntityObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AddResponseHeadersFilter>();

                // integrate xml comments
                options.IncludeXmlComments(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Internal.Messages.WebApi.xml"));
            });

            // For catching, logging and returning appropriate controller related errors
            services.AddScoped<ApiExceptionFilter>();

            // Register the shared dependencies in the Mapping project
            DependencyConfig.Register(services, Configuration, Environment, System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Allow cross origin resource sharing for testing all development requests.
                // This should not be done for a production build.
                app.UseCors(builder =>
                {
                    builder.WithOrigins("*");
                });
            }
            else
            {
                // In a non-development environment, this adds middleware to the pipeline that
                // will catch exceptions, log them, and re-execute the request in an
                // alternate pipeline. The request will not be re-executed if the response has
                // already started.
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("There was an unexpected error.");
                    });
                });
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = string.Empty;
            });

            // Add logging to the request pipeline
            LoggerConfig.Configure(loggerFactory);

            // Adds middleware for redirecting HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Marks the position in the middleware pipeline where a routing
            // decision is made (where an endpoint is selected)
            app.UseRouting();

            // Adds the Microsoft.AspNetCore.Authorization.AuthorizationMiddleware to the specified
            // Microsoft.AspNetCore.Builder.IApplicationBuilder, which enables authorization
            // capabilities.
            app.UseAuthorization();

            // Marks the position in the middleware pipeline where the selected
            // endpoint is executed
            app.UseEndpoints(endpoints =>
            {
                // Adds endpoints for our controller actions but no routes are specified
                // For a Web API, use attributes at controller and action level: [Route], [HttpGet], ...
                endpoints.MapControllers();
            });
        }
    }
}
