using System.IO;
using System.Threading.Tasks;

namespace CoreBuilder.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(Stream stream, string contentType, string fileName);
        Task<Stream?> DownloadAsync(string filePath);
        Task DeleteAsync(string filePath);
    }
}
