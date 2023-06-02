using Elsa.Models;
using System.Threading;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Mvc;
using WorkflowsPayments.Models;
using WorkflowsPayments.Services;
using Elsa.Activities.Workflows.Workflow;

namespace WorkflowsPayments.Controllers
{
    /// <summary>
    /// https://github.com/elsa-workflows/elsa-core/discussions/871
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DocController : ControllerBase
    {


        private readonly ILogger<SlowController> _logger;
        private readonly IWorkflowInstanceStore _store;
        private readonly IWorkflowDefinitionDispatcher _dispatcher;
        private readonly IDocumentStore _docs;
        private readonly ISystemClock _clock;

        public DocController(ILogger<SlowController> logger,
            IWorkflowInstanceStore store,
            IWorkflowDefinitionDispatcher workflow,
            IDocumentStore docs,
            ISystemClock clock)
        {
            _logger = logger;
            _store = store;
            _dispatcher = workflow;
            _docs = docs;
            _clock = clock;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var correlationId = Guid.NewGuid().ToString();
            var documentTypeId = Guid.NewGuid().ToString("N");
            var documentId = Guid.NewGuid().ToString("N");

            // Create a document record.
            var document = new Document
            {
                Id = documentId,
                Status = DocumentStatus.New,
                DocumentTypeId = documentTypeId,
                CreatedAt = _clock.UtcNow,
                FileName = $"{documentTypeId}.pdf"
            };

            await _docs.SaveAsync(document);

            var workflowBlueprintId = "db349538e46a439db8a3fd3279855e00";
            await _dispatcher.DispatchAsync(
                new ExecuteWorkflowDefinitionRequest(workflowBlueprintId, CorrelationId: document.Id, Input: new WorkflowInput(document.Id)), CancellationToken.None);


            return Ok(new
            {
                CorrelationId = correlationId
            });
        }
    }
}