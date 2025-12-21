namespace CoreBuilder.Models;

public class FontSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string PrimaryFont { get; set; } = "Inter, sans-serif";
    public string HeadingFont { get; set; } = "Inter, sans-serif";
    public string MonospaceFont { get; set; } = "Consolas, Monaco, 'Courier New', monospace";

    public int BaseFontSize { get; set; } = 14;
    public string HeadingFontWeight { get; set; } = "700";
    public string BodyFontWeight { get; set; } = "400";

    public decimal LineHeight { get; set; } = 1.5m;
    public string LetterSpacing { get; set; } = "normal";

    public bool UseGoogleFonts { get; set; } = true;
    public string? GoogleFontsUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public static class PopularFonts
{
    public static readonly Dictionary<string, string> Fonts = new()
    {
        { "Inter", "https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap" },
        { "Roboto", "https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" },
        { "Open Sans", "https://fonts.googleapis.com/css2?family=Open+Sans:wght@300;400;600;700&display=swap" },
        { "Lato", "https://fonts.googleapis.com/css2?family=Lato:wght@300;400;700&display=swap" },
        { "Montserrat", "https://fonts.googleapis.com/css2?family=Montserrat:wght@300;400;500;600;700&display=swap" },
        { "Poppins", "https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" },
        { "Nunito", "https://fonts.googleapis.com/css2?family=Nunito:wght@300;400;600;700;800&display=swap" },
        { "Rubik", "https://fonts.googleapis.com/css2?family=Rubik:wght@300;400;500;600;700&display=swap" },
        { "Noto Sans", "https://fonts.googleapis.com/css2?family=Noto+Sans:wght@300;400;600;700&display=swap" },
        { "Source Sans Pro", "https://fonts.googleapis.com/css2?family=Source+Sans+Pro:wght@300;400;600;700&display=swap" }
    };

    public static readonly Dictionary<string, string> Categories = new()
    {
        { "Inter", "Modern" },
        { "Roboto", "Modern" },
        { "Open Sans", "Profesyonel" },
        { "Lato", "Profesyonel" },
        { "Montserrat", "Modern" },
        { "Poppins", "Modern" },
        { "Nunito", "Yuvarlak" },
        { "Rubik", "Türkçe Uyumlu" },
        { "Noto Sans", "Türkçe Uyumlu" },
        { "Source Sans Pro", "Profesyonel" }
    };
}
