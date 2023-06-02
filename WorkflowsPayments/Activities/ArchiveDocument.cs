using System.Threading.Tasks;
using DocumentManagement.Core.Services;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Providers.WorkflowStorage;
using Elsa.Services;
using Elsa.Services.Models;
using WorkflowsPayments.Controllers;
using WorkflowsPayments.Models;
using WorkflowsPayments.Services;

namespace DocumentManagement.Workflows.Activities
{
    [Activity(Category = "Document Management", Description = "Archives the specified document.")]
    public class ArchiveDocument : Activity
    {
        private readonly IDocumentStore _documentStore;
        private readonly ILogger<ArchiveDocument> _logger;

        public ArchiveDocument(IDocumentStore documentStore, ILogger<ArchiveDocument> logger)
        {
            _documentStore = documentStore;
            _logger = logger;
        }
        
        [ActivityInput(
            Label = "Document",
            Hint = "The document to archive",
            SupportedSyntaxes = new[] {SyntaxNames.JavaScript, SyntaxNames.Liquid},
            DefaultWorkflowStorageProvider = TransientWorkflowStorageProvider.ProviderName
        )]
        public Document Document { get; set; } = default!;

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            _logger.LogInformation("****** Archiving....");

            Document.Status = DocumentStatus.Archived;
            await _documentStore.SaveAsync(Document);
            await Task.Delay(2000);

            _logger.LogInformation("****** Done Archiving....");

            return Done();
        }
    }
}