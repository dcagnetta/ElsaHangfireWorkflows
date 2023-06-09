using Elsa.Http;
using Elsa.Workflows.Core.Contracts;
using Elsa.Workflows.Management.Contracts;
using Elsa.Workflows.Management.Filters;
using Elsa.Workflows.Runtime.Contracts;
using Elsa.Workflows.Runtime.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using WorkflowServerVersion3.Workflows;

namespace WorkflowServerVersion3.Controllers;

[ApiController]
[Route("run-workflow")]
public class RunWorkflowController : ControllerBase
{
    private readonly IWorkflowRunner _runner;
    private readonly IWorkflowInstanceStore _store;
    private readonly IWorkflowDispatcher _dispatcher;

    public RunWorkflowController(
        IWorkflowRunner runner,
        IWorkflowInstanceStore store,
        IWorkflowDispatcher dispatcher)
    {
        _runner = runner;
        _store = store;
        _dispatcher = dispatcher;
    }

    [HttpGet]
    public async Task Post()
    {
        await _runner.RunAsync(new WriteHttpResponse
        {
            Content = new("Hello ASP.NET world!")
        });
    }

    [HttpGet("run")]
    public async Task Run()
    {
        await _runner.RunAsync<BreakForEachWorkflow>();
        await _runner.RunAsync<ShoppingWorkflow>();
    }

    // http://localhost:5000/run-workflow/hangfire
    [HttpGet("hangfire")]
    public async Task<IActionResult> Hangfire()
    {
        var correlationId = Guid.NewGuid().ToString();

        await _dispatcher.DispatchAsync(
            new DispatchWorkflowDefinitionRequest
            {
                DefinitionId = nameof(ShoppingWorkflow),
                CorrelationId = correlationId
            });


        return Ok(new
        {
            CorrelationId = correlationId
        });
    }

    // http://localhost:5000/run-workflow/track
    [HttpGet("track/{correlationId}")]
    public async Task<IActionResult> Get(string correlationId)
    {
        var instance = await _store.FindAsync(new WorkflowInstanceFilter()
        {
            CorrelationId = correlationId
        });

        if(instance == null)
            return NotFound();

        return Ok(new
        {
            WorkflowInstanceId = instance.Id,
            instance.Status,
            instance.SubStatus,
            instance.CreatedAt,
            LastExecutedAt = instance.LastExecutedAt ?? DateTimeOffset.MinValue,
            //CurrentActivity = instance.CurrentActivity == null ? "" : instance.CurrentActivity.ActivityId
        });
    }
}