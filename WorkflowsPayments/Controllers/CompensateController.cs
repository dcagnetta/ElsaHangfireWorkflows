using Elsa.Persistence;
using Elsa.Services;
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
        
        private readonly ILogger<CompensateController> _logger;
        private readonly IWorkflowInstanceStore _store;
        private readonly IWorkflowDefinitionDispatcher _dispatcher;

        public CompensateController(
            ILogger<CompensateController> logger,
            IWorkflowInstanceStore store,
            IWorkflowDefinitionDispatcher workflow)
        {
            _logger = logger;
            _store = store;
            _dispatcher = workflow;
        }


        [HttpGet]
        public async Task<IActionResult> Compensate()
        {
            var correlationId = Guid.NewGuid().ToString();

            await _dispatcher.DispatchAsync(
                new ExecuteWorkflowDefinitionRequest(
                    WorkflowDefinitionId: nameof(CompensableWorkflow),
                    CorrelationId: correlationId));

            return Ok(new
            {
                CorrelationId = correlationId
            });
        }
    }
}