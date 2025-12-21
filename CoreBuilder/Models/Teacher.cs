namespace CoreBuilder.Models;

/// <summary>
/// Ö?retmen/E?itmen modeli
/// </summary>
public class Teacher
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public required string FullName { get; set; }
    public string? Title { get; set; } // Prof. Dr., Doç. Dr., Dr., Ö?r. Gör., vb.
    public string? Department { get; set; } // Matematik, Türkçe, ?ngilizce, vb.
    public string? Specialty { get; set; } // KPSS, ALES, DGS, YDS, vb.
    public string? PhotoUrl { get; set; }
    public string? Bio { get; set; } // K?sa biyografi
    public int Experience { get; set; } // Y?l olarak deneyim
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation
    public Tenant? Tenant { get; set; }
}
