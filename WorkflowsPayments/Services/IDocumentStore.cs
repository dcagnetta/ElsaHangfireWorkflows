using WorkflowsPayments.Models;

namespace WorkflowsPayments.Services
{
    public interface IDocumentStore
    {
        Task SaveAsync(Document entity, CancellationToken cancellationToken = default);
        Task<Document?> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}