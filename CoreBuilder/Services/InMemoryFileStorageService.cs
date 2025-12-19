using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace CoreBuilder.Services
{
    public class InMemoryFileStorageService : IFileStorageService
    {
        private readonly ConcurrentDictionary<string, byte[]> _store = new();

        public Task DeleteAsync(string filePath)
        {
            _store.TryRemove(filePath, out _);
            return Task.CompletedTask;
        }

        public Task<Stream?> DownloadAsync(string filePath)
        {
            if (_store.TryGetValue(filePath, out var data))
            {
                return Task.FromResult<Stream?>(new MemoryStream(data));
            }
            return Task.FromResult<Stream?>(null);
        }

        public Task<string> UploadAsync(Stream stream, string contentType, string fileName)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var id = Guid.NewGuid().ToString() + "_" + fileName;
            _store[id] = ms.ToArray();
            return Task.FromResult(id);
        }
    }
}
