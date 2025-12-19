using Microsoft.AspNetCore.Components.Forms;
using CoreBuilder.Data;
using CoreBuilder.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOPath = System.IO.Path; // Alias to avoid conflict with HotChocolate.Path
using SystemIO = System.IO;

namespace CoreBuilder.Services
{
    public class ImageService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImageService> _logger;

        // İzin verilen resim formatları
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public ImageService(AppDbContext context, IWebHostEnvironment env, ILogger<ImageService> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task<string> UploadImageAsync(IBrowserFile file, Guid tenantId)
        {
            try
            {
                // 1. Validasyonlar
                ValidateFile(file);

                // 2. Klasör yolunu hazırla (wwwroot/uploads)
                var uploadsFolder = IOPath.Combine(_env.WebRootPath, "uploads");
                if (!SystemIO.Directory.Exists(uploadsFolder))
                {
                    SystemIO.Directory.CreateDirectory(uploadsFolder);
                    _logger.LogInformation("Uploads klasörü oluşturuldu: {Path}", uploadsFolder);
                }

                // 3. Benzersiz dosya adı oluştur
                var extension = IOPath.GetExtension(file.Name).ToLowerInvariant();
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = IOPath.Combine(uploadsFolder, fileName);

                // 4. Dosyayı kaydet
                _logger.LogInformation("Dosya kaydediliyor: {FileName} (Tenant: {TenantId})", file.Name, tenantId);
                
                await using var stream = file.OpenReadStream(maxAllowedSize: MaxFileSize);
                await using var fileStream = new SystemIO.FileStream(filePath, SystemIO.FileMode.Create);
                await stream.CopyToAsync(fileStream);

                // 5. Veritabanına kaydet
                var relativePath = $"/uploads/{fileName}";
                var imageEntry = new ImageEntry 
                { 
                    FileName = file.Name, 
                    FilePath = relativePath, 
                    TenantId = tenantId,
                    UploadDate = DateTime.Now
                };
                
                _context.Images.Add(imageEntry);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Dosya başarıyla yüklendi: {Path}", relativePath);
                return relativePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dosya yükleme hatası: {FileName}, Tenant: {TenantId}", file?.Name, tenantId);
                throw new InvalidOperationException($"Dosya yüklenirken bir hata oluştu: {ex.Message}", ex);
            }
        }

        public List<ImageEntry> GetImagesByTenant(Guid tenantId)
        {
            try
            {
                var images = _context.Images
                    .Where(i => i.TenantId == tenantId)
                    .OrderByDescending(i => i.UploadDate)
                    .ToList();

                _logger.LogInformation("Tenant {TenantId} için {Count} resim getirildi", tenantId, images.Count);
                return images;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Resimler getirilirken hata oluştu. Tenant: {TenantId}", tenantId);
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(Guid imageId, Guid tenantId)
        {
            try
            {
                var image = await _context.Images
                    .Where(i => i.Id == imageId && i.TenantId == tenantId)
                    .FirstOrDefaultAsync();

                if (image == null)
                {
                    _logger.LogWarning("Silinecek resim bulunamadı: {ImageId}", imageId);
                    return false;
                }

                // Fiziksel dosyayı sil
                var physicalPath = IOPath.Combine(_env.WebRootPath, image.FilePath.TrimStart('/'));
                if (SystemIO.File.Exists(physicalPath))
                {
                    SystemIO.File.Delete(physicalPath);
                    _logger.LogInformation("Fiziksel dosya silindi: {Path}", physicalPath);
                }

                // Veritabanından sil
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Resim başarıyla silindi: {ImageId}", imageId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Resim silinirken hata oluştu: {ImageId}", imageId);
                throw;
            }
        }

        private void ValidateFile(IBrowserFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file), "Dosya seçilmedi.");

            // Dosya boyutu kontrolü
            if (file.Size > MaxFileSize)
                throw new InvalidOperationException($"Dosya boyutu {MaxFileSize / (1024 * 1024)}MB'dan büyük olamaz.");

            // Dosya uzantısı kontrolü
            var extension = IOPath.GetExtension(file.Name).ToLowerInvariant();
            if (Array.IndexOf(AllowedExtensions, extension) == -1)
                throw new InvalidOperationException($"Sadece şu formatlar desteklenir: {string.Join(", ", AllowedExtensions)}");

            // Dosya adı güvenlik kontrolü
            if (file.Name.IndexOfAny(IOPath.GetInvalidFileNameChars()) >= 0)
                throw new InvalidOperationException("Dosya adı geçersiz karakterler içeriyor.");
        }
    }
}
