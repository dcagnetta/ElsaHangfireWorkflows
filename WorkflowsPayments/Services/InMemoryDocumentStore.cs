using System.Collections.Concurrent;
using WorkflowsPayments.Models;

namespace WorkflowsPayments.Services
{
    public class InMemoryDocumentStore : IDocumentStore
    {
        private ConcurrentBag<Document> _items = new();

        public Task SaveAsync(Document entity, CancellationToken cancellationToken = default)
        {
             _items.Add(entity);
             
             return Task.CompletedTask;
        }

        public Task<Document?> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            var doc = _items.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(doc);
        }
    }
}
