using CoreBuilder.Models;
using CoreBuilder.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoreBuilder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly ThemeService _themeService;

        public ThemesController(ThemeService themeService)
        {
            _themeService = themeService;
        }

        // GET: api/Themes
        [HttpGet]
        public async Task<IActionResult> GetThemes()
        {
            var themes = await _themeService.GetThemesAsync();
            return Ok(themes);
        }

        // GET: api/Themes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTheme(Guid id)
        {
            var theme = await _themeService.GetThemeAsync(id);
            if (theme == null) return NotFound();
            return Ok(theme);
        }

        // POST: api/Themes
        [HttpPost]
        public async Task<IActionResult> CreateTheme([FromBody] Theme theme)
        {
            // EKSİK OLAN METOT BURADA ÇAĞRILIYORDU VE HATA VERİYORDU.
            // Şimdi sadece await ile çağırıyoruz, sonucu değişkene atamıyoruz.
            await _themeService.CreateThemeAsync(theme);

            // Başarılı oluşturma yanıtı
            return CreatedAtAction(nameof(GetTheme), new { id = theme.Id }, theme);
        }

        // DELETE: api/Themes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheme(Guid id)
        {
            // EKSİK OLAN METOT BURADA ÇAĞRILIYORDU VE HATA VERİYORDU.
            await _themeService.DeleteThemeAsync(id);

            return NoContent();
        }
    }
}
