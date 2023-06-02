using Storage.Net.Blobs;

namespace DocumentManagement.Core.Services
{
    public class FileStorage : IFileStorage
    {
        private readonly IBlobStorage _blobStorage;
        

        public Task WriteAsync(Stream data, string fileName, CancellationToken cancellationToken = default) =>
            Console.Out.WriteLineAsync("FileStorage - WriteAsync");

        public Task ReadAsync(string fileName, CancellationToken cancellationToken = default) =>
         Console.Out.WriteLineAsync("FileStorage - ReadAsync");
    }
}