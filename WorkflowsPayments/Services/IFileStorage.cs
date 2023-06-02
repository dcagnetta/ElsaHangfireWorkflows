namespace DocumentManagement.Core.Services
{
    public interface IFileStorage
    {
        Task WriteAsync(Stream data, string fileName, CancellationToken cancellationToken = default);
        Task ReadAsync(string fileName, CancellationToken cancellationToken = default);
    }
}