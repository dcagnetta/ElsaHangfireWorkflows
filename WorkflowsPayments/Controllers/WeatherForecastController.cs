
using Elsa.Services;
using Microsoft.AspNetCore.Mvc;

namespace WorkflowsPayments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWorkflowDefinitionDispatcher _dispatcher;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWorkflowDefinitionDispatcher workflow)
        {
            _logger = logger;
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
    }
}