using WorkflowsPayments.Models;

namespace WorkflowsPayments.Services
{
    public interface IDocumentService
    {
        Task<Document> SaveDocumentAsync(string fileName, Stream data, string documentTypeId, CancellationToken cancellationToken = default);
    }
}