using Elsa.Activities.Email.Options;
using Elsa.Activities.Http.Options;
using Elsa.Persistence.EntityFramework.SqlServer;
using Elsa.Server.Hangfire.Extensions;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder.Extensions;

namespace WorkflowsPayments
{
    public class Startup
    {
        string dbConnectionString = "Data source=.;Initial Catalog=Elsa;Integrated Security=True;Encrypt=False;";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Razor Pages (for UI).
            // services.AddRazorPages();

            // Hangfire (for background tasks).
            AddHangfire(services, dbConnectionString);

            // Elsa (workflows engine).
            AddWorkflowServices(services, dbConnectionString);

            /*services
                .AddElsa(options => options
                    .AddHttpActivities()
                    .AddWorkflow<HelloWorldWorkflow>());*/

            // Allow arbitrary client browser apps to access the API for demo purposes only.
            // In a production environment, make sure to allow only origins you trust.
            // services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("Content-Disposition")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }

            app
                .UseStaticFiles()
                .UseCors()
                .UseRouting()
                .UseHttpActivities() // Install middleware for triggering HTTP Endpoint activities. 
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapRazorPages();
                    endpoints.MapControllers(); // Elsa API Endpoints are implemented as ASP.NET API controllers.
                });*/


            app
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers(); // Elsa API Endpoints are implemented as ASP.NET API controllers.
                });

            app.UseHttpActivities();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                IsReadOnlyFunc = (DashboardContext context) => true
            });
        }

        private void AddHangfire(IServiceCollection services, string dbConnectionString)
        {
            services
                .AddHangfire(config => config
                    // Use same SqlServer  database as Elsa for storing jobs. 
                    .UseSqlServerStorage(dbConnectionString, new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true,
                        PrepareSchemaIfNecessary = true,
                        SchemaName = "HangfireElsa"
                    })
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings())
                .AddHangfireServer((sp, options) =>
                {
                    // Bind settings from configuration.
                    Configuration.GetSection("Hangfire").Bind(options);
                    // Configure queues for Elsa workflow dispatchers.
                    options.ConfigureForElsaDispatchers(sp);
                });
        }

        private void AddWorkflowServices(IServiceCollection services, string dbConnectionString)
        {
            services.AddWorkflowServices(dbContext => dbContext.UseSqlServer(dbConnectionString));

            // Configure SMTP.
            //services.Configure<SmtpOptions>(options => Configuration.GetSection("Elsa:Smtp").Bind(options));

            // Configure HTTP activities.
            //services.Configure<HttpActivityOptions>(options => Configuration.GetSection("Elsa:Server").Bind(options));

            // Elsa API (to allow Elsa Dashboard to connect for checking workflow instances).
            services.AddElsaApiEndpoints();
        }
    }
}
