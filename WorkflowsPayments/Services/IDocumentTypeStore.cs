using WorkflowsPayments.Models;

namespace WorkflowsPayments.Services;

public interface IDocumentTypeStore
{
    Task<IEnumerable<DocumentType>> ListAsync(CancellationToken cancellationToken = default);
    Task<DocumentType?> GetAsync(string id, CancellationToken cancellationToken = default);
}