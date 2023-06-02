using Storage.Net.Blobs;

namespace DocumentManagement.Core.Services
{
    public class FileStorage : IFileStorage
    {
        private readonly IBlobStorage _blobStorage;
        

        public Task WriteAsync(Stream data, string fileName, CancellationToken cancellationToken = default) =>
            Console.Out.WriteLineAsync("**** >>>  FileStorage - WriteAsync");

        public async Task ReadAsync(string fileName, CancellationToken cancellationToken = default)
        {
            await Task.Delay(5000);

            await Console.Out.WriteLineAsync("**** >>>  FileStorage - ReadAsync");
        }
    }
}