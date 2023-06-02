
using Elsa;
using Elsa.Persistence;
using Elsa.Services;
using Microsoft.AspNetCore.Mvc;

namespace WorkflowsPayments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlowController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<SlowController> _logger;
        private readonly IWorkflowInstanceStore _store;
        private readonly IWorkflowDefinitionDispatcher _dispatcher;

        public SlowController(ILogger<SlowController> logger,
            IWorkflowInstanceStore store,
            IWorkflowDefinitionDispatcher workflow)
        {
            _logger = logger;
            _store = store;
            _dispatcher = workflow;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var correlationId = Guid.NewGuid().ToString();

            await _dispatcher.DispatchAsync(
                new ExecuteWorkflowDefinitionRequest(
                    WorkflowDefinitionId: nameof(VerySlowWorkflow),
                    CorrelationId: correlationId));

            return Ok(new
            {
                CorrelationId = correlationId
            });
        }

        [HttpGet("{correlationId}")]
        public async Task<IActionResult> Get(string correlationId)
        {
            var instance = await _store.FindByCorrelationIdAsync(correlationId);

            if(instance == null)
                return NotFound();

            return Ok(new
            {
                WorkflowInstanceId = instance.Id,
                instance.WorkflowStatus,
                CreatedAt = instance.CreatedAt.ToDateTimeOffset(),
                LastExecutedAt = instance.LastExecutedAt?.ToDateTimeOffset() ?? DateTimeOffset.MinValue,
                CurrentActivity = instance.CurrentActivity == null ? "" : instance.CurrentActivity.ActivityId
            });
        }
    }
}