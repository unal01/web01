using CoreBuilder.Models;
using CoreBuilder.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CoreBuilder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitesController : ControllerBase
    {
        private readonly SiteService _siteService;

        public SitesController(SiteService siteService)
        {
            _siteService = siteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSites()
        {
            // Artık serviste GetSitesAsync var, hata vermez.
            var sites = await _siteService.GetSitesAsync();
            return Ok(sites);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSite(Guid id)
        {
            var site = await _siteService.GetSiteAsync(id);
            if (site == null) return NotFound();
            return Ok(site);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSite([FromBody] Tenant site)
        {
            await _siteService.CreateSiteAsync(site);
            return CreatedAtAction(nameof(GetSite), new { id = site.Id }, site);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSite(Guid id)
        {
            await _siteService.DeleteSiteAsync(id);
            return NoContent();
        }
    }
}
