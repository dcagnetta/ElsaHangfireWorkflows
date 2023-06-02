using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Providers.WorkflowStorage;
using Elsa.Services;
using Elsa.Services.Models;

namespace DocumentManagement.Workflows.Activities
{
    [Action(Category = "Document Management", Description = "Zips the specified file.")]
    public class ZipFile : Activity
    {
        private readonly ILogger<ZipFile> _logger;

        [ActivityInput(
            Hint = "The file stream to zip.",
            SupportedSyntaxes = new[] {SyntaxNames.JavaScript},
            DefaultWorkflowStorageProvider = TransientWorkflowStorageProvider.ProviderName,
            DisableWorkflowProviderSelection = true
        )]
        public Stream Stream { get; set; } = default!;

        [ActivityInput(
            Hint = "The file name to use for the zip entry.",
            SupportedSyntaxes = new[] {SyntaxNames.JavaScript}
        )]
        public string FileName { get; set; } = default!;

        [ActivityOutput(
            Hint = "The zipped file stream.",
            DefaultWorkflowStorageProvider = TransientWorkflowStorageProvider.ProviderName,
            DisableWorkflowProviderSelection = true
        )]
        public Stream Output { get; set; } = default!;

        public ZipFile(ILogger<ZipFile> logger)
        {
            _logger = logger;
        }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            /*var outputStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
            {
                var zipEntry = zipArchive.CreateEntry(FileName, CompressionLevel.Optimal);
                await using var zipStream = zipEntry.Open();
                await Stream.CopyToAsync(zipStream);
            }

            // Reset position.
            outputStream.Seek(0, SeekOrigin.Begin);
            Output = outputStream;*/

            _logger.LogInformation("****** Zipping....");

                await Task.Delay(5000);
            Output = new MemoryStream();

            _logger.LogInformation("****** Done Zipping....");

            return Done();
        }
    }
}