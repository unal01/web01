using CoreBuilder.Data;
using CoreBuilder.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreBuilder.Services;

/// <summary>
/// Ö?retmen/E?itmen yönetim servisi
/// </summary>
public class TeacherService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TeacherService> _logger;

    public TeacherService(AppDbContext context, ILogger<TeacherService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Tenant'a ait aktif ö?retmenleri getir
    /// </summary>
    public async Task<List<Teacher>> GetActiveTeachersAsync(Guid tenantId)
    {
        return await _context.Teachers
            .Where(t => t.TenantId == tenantId && t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.FullName)
            .ToListAsync();
    }

    /// <summary>
    /// Tenant'a ait tüm ö?retmenleri getir (aktif + pasif)
    /// </summary>
    public async Task<List<Teacher>> GetAllTeachersAsync(Guid tenantId)
    {
        return await _context.Teachers
            .Where(t => t.TenantId == tenantId)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.FullName)
            .ToListAsync();
    }

    /// <summary>
    /// ID'ye göre ö?retmen getir
    /// </summary>
    public async Task<Teacher?> GetTeacherByIdAsync(Guid id)
    {
        return await _context.Teachers.FindAsync(id);
    }

    /// <summary>
    /// Uzmanl?k alan?na göre ö?retmenleri getir
    /// </summary>
    public async Task<List<Teacher>> GetTeachersBySpecialtyAsync(Guid tenantId, string specialty)
    {
        return await _context.Teachers
            .Where(t => t.TenantId == tenantId && t.IsActive && t.Specialty == specialty)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
    }

    /// <summary>
    /// Yeni ö?retmen ekle
    /// </summary>
    public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation(
            "Ö?retmen eklendi: {FullName} (TenantId: {TenantId})", 
            teacher.FullName, 
            teacher.TenantId);
        
        return teacher;
    }

    /// <summary>
    /// Ö?retmen bilgilerini güncelle
    /// </summary>
    public async Task<Teacher?> UpdateTeacherAsync(Teacher teacher)
    {
        var existing = await _context.Teachers.FindAsync(teacher.Id);
        if (existing == null) return null;

        existing.FullName = teacher.FullName;
        existing.Title = teacher.Title;
        existing.Department = teacher.Department;
        existing.Specialty = teacher.Specialty;
        existing.PhotoUrl = teacher.PhotoUrl;
        existing.Bio = teacher.Bio;
        existing.Experience = teacher.Experience;
        existing.Email = teacher.Email;
        existing.Phone = teacher.Phone;
        existing.IsActive = teacher.IsActive;
        existing.DisplayOrder = teacher.DisplayOrder;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        _logger.LogInformation(
            "Ö?retmen güncellendi: {FullName} (Id: {Id})", 
            teacher.FullName, 
            teacher.Id);
        
        return existing;
    }

    /// <summary>
    /// Ö?retmeni sil
    /// </summary>
    public async Task<bool> DeleteTeacherAsync(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return false;

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation(
            "Ö?retmen silindi: {FullName} (Id: {Id})", 
            teacher.FullName, 
            id);
        
        return true;
    }

    /// <summary>
    /// Ö?retmeni aktif/pasif yap
    /// </summary>
    public async Task<bool> ToggleTeacherStatusAsync(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return false;

        teacher.IsActive = !teacher.IsActive;
        teacher.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation(
            "Ö?retmen durumu de?i?tirildi: {FullName} -> {Status}", 
            teacher.FullName, 
            teacher.IsActive ? "Aktif" : "Pasif");
        
        return true;
    }

    /// <summary>
    /// Ö?retmen s?ras?n? de?i?tir
    /// </summary>
    public async Task<bool> ReorderTeachersAsync(List<Guid> teacherIds)
    {
        for (int i = 0; i < teacherIds.Count; i++)
        {
            var teacher = await _context.Teachers.FindAsync(teacherIds[i]);
            if (teacher != null)
            {
                teacher.DisplayOrder = i;
                teacher.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Ö?retmen s?ralamas? güncellendi: {Count} ö?retmen", teacherIds.Count);
        
        return true;
    }

    /// <summary>
    /// ?statistikler
    /// </summary>
    public async Task<TeacherStatistics> GetStatisticsAsync(Guid tenantId)
    {
        var teachers = await _context.Teachers
            .Where(t => t.TenantId == tenantId)
            .ToListAsync();

        return new TeacherStatistics
        {
            TotalCount = teachers.Count,
            ActiveCount = teachers.Count(t => t.IsActive),
            InactiveCount = teachers.Count(t => !t.IsActive),
            AverageExperience = teachers.Any() ? (int)teachers.Average(t => t.Experience) : 0,
            SpecialtyCounts = teachers
                .Where(t => !string.IsNullOrEmpty(t.Specialty))
                .GroupBy(t => t.Specialty)
                .ToDictionary(g => g.Key!, g => g.Count())
        };
    }
}

/// <summary>
/// Ö?retmen istatistikleri
/// </summary>
public class TeacherStatistics
{
    public int TotalCount { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public int AverageExperience { get; set; }
    public Dictionary<string, int> SpecialtyCounts { get; set; } = new();
}
