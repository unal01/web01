using System;
using System.ComponentModel.DataAnnotations;

namespace CoreBuilder.Models
{
    public class SystemSettings
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // ===== GENEL AYARLAR =====
        public string? SiteName { get; set; }
        public string? SiteDescription { get; set; }
        public string? SiteLogoUrl { get; set; }
        public string? FaviconUrl { get; set; }
        public string DefaultLanguage { get; set; } = "tr-TR";
        public string TimeZone { get; set; } = "Europe/Istanbul";
        public string DateFormat { get; set; } = "dd/MM/yyyy";

        // ===== SEO AYARLARI =====
        public string? GoogleAnalyticsId { get; set; }
        public string? GoogleSearchConsoleCode { get; set; }
        public string? MetaKeywords { get; set; }
        public bool AutoGenerateSitemap { get; set; } = true;
        public string? FacebookAppId { get; set; }
        public string? TwitterHandle { get; set; }

        // ===== EMAIL AYARLARI =====
        public string? SmtpHost { get; set; }
        public int SmtpPort { get; set; } = 587;
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }
        public bool SmtpUseSsl { get; set; } = true;
        public string? EmailFromAddress { get; set; }
        public string? EmailFromName { get; set; }

        // ===== SOSYAL MEDYA =====
        public string? FacebookUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? YouTubeUrl { get; set; }
        public string? WhatsAppNumber { get; set; }

        // ===== GÜVENLİK AYARLARI =====
        public bool Enable2FA { get; set; } = false;
        public int SessionTimeoutMinutes { get; set; } = 60;
        public int PasswordMinLength { get; set; } = 8;
        public bool PasswordRequireUppercase { get; set; } = true;
        public bool PasswordRequireDigit { get; set; } = true;
        public bool PasswordRequireSpecialChar { get; set; } = true;
        public bool ForceHttps { get; set; } = true;
        public int MaxLoginAttempts { get; set; } = 5;

        // ===== PERFORMANS & CACHE =====
        public int CacheDurationMinutes { get; set; } = 60;
        public bool EnableLazyLoading { get; set; } = true;
        public int ImageCompressionQuality { get; set; } = 85;
        public string? CdnUrl { get; set; }

        // ===== BAKIM MODU =====
        public bool MaintenanceMode { get; set; } = false;
        public string? MaintenanceMessage { get; set; }

        // ===== BİLDİRİM AYARLARI =====
        public bool EmailNotificationsEnabled { get; set; } = true;
        public bool PushNotificationsEnabled { get; set; } = false;
        public string? WebhookUrl { get; set; }

        // ===== API AYARLARI =====
        public string? ApiKey { get; set; }
        public int ApiRateLimitPerMinute { get; set; } = 100;

        // ===== GELİŞMİŞ =====
        public string? CustomCss { get; set; }
        public string? CustomJavascript { get; set; }
        public string? HeaderScripts { get; set; }
        public string? FooterScripts { get; set; }
        public bool DebugMode { get; set; } = false;

        // ===== YEDEKLEME =====
        public bool AutoBackupEnabled { get; set; } = true;
        public int BackupIntervalDays { get; set; } = 7;
        public string? BackupStoragePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
