using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Elsa.Workflows.Core.Middleware.Workflows;

string dbConnectionString = "Data source=.;Initial Catalog=Elsa3;Integrated Security=True;Encrypt=False;";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Add Elsa services.
builder.Services.AddElsa(elsa =>
{
    elsa.AddActivitiesFrom<Program>();
    elsa.AddWorkflowsFrom<Program>();

    // Configure management feature to use EF Core.
    elsa.UseWorkflowManagement(
        management =>
            management.UseEntityFrameworkCore(ef =>
                ef.UseSqlServer(dbConnectionString)));

    elsa.UseWorkflowRuntime(runtime =>
    {
        //  Register the workflow with the runtime
        // runtime.AddWorkflow<HelloWorldHttpWorkflow>();

        runtime.UseDefaultRuntime(dr =>
            dr.UseEntityFrameworkCore(ef => 
                ef.UseSqlServer(dbConnectionString)));
        
        // Use Hangfire to schedule background activities.
        runtime.UseHangfireBackgroundActivityScheduler();

        // Capture execution log records.
        runtime.UseExecutionLogRecords(e =>
            e.UseEntityFrameworkCore(ef =>
                ef.UseSqlServer(dbConnectionString)));

        // Capture workflow state.
        runtime.UseAsyncWorkflowStateExporter();
    });

    elsa.UseWorkflows(workflows =>
    {
        // Configure workflow execution pipeline to handle workflow contexts.
        workflows.WithWorkflowExecutionPipeline(pipeline => pipeline
            .Reset()
            .UsePersistentVariables()
            .UseBookmarkPersistence()
            .UseWorkflowExecutionLogPersistence()
            .UseWorkflowStatePersistence()
            .UseWorkflowContexts()
            .UseDefaultActivityScheduler()
        );

        // Configure activity execution pipeline to handle workflow contexts.
        workflows.WithActivityExecutionPipeline(pipeline => pipeline
            .Reset()
            .UseWorkflowContexts()
            .UseBackgroundActivityInvoker()
        );
    });

    // Expose API endpoints.
    elsa.UseWorkflowsApi();

    // Use Hangfire.
    elsa.UseHangfire(hangfire =>
    {
        hangfire.UseSqlServerStorage(options => { options.NameOrConnectionString = dbConnectionString; });
    });

    // Use hangfire for scheduling timer events.
    elsa.UseScheduling(scheduling => scheduling.UseHangfireScheduler());

    elsa.UseWorkflowContexts();

    // Add services for HTTP activities and workflow middleware.
    elsa.UseHttp();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();
app.UseAuthorization();
app.UseWorkflows(); // Exposing workflows as endpoints

// Run the web app.
app.Run();
