using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace WorkflowsPayments.Controllers
{
    /// <summary>
    /// https://github.com/elsa-workflows/elsa-core/pull/2940
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CompensateController : ControllerBase
    {
        

        private readonly IWorkflowDefinitionDispatcher _dispatcher;
        private readonly IBuildsAndStartsWorkflow _starter;

        public CompensateController(
            IWorkflowDefinitionDispatcher workflow,
            IBuildsAndStartsWorkflow starter)
        {
            _dispatcher = workflow;
            _starter = starter;
        }


        [HttpGet]
        public async Task<IActionResult> Compensate()
        {
            var correlationId = Guid.NewGuid().ToString();

            //var result = await _starter.BuildAndStartWorkflowAsync<CompensableWorkflow>();

            //await Task.Delay(5000);

            //var result1 = await _starter.BuildAndStartWorkflowAsync<FaultingWorkflow>();

            await _dispatcher.DispatchAsync(
                new ExecuteWorkflowDefinitionRequest(
                    WorkflowDefinitionId: nameof(CompensableWorkflow),
                    CorrelationId: correlationId));

            return Ok(new
            {
                CorrelationId = correlationId,
                //Result = result
            });
        }
    }
}