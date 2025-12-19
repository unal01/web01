using CoreBuilder.Data;
using CoreBuilder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CoreBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public FormsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<IActionResult> GetFormsByTenant(Guid tenantId)
        {
            if (tenantId == Guid.Empty) return BadRequest("tenantId is required");
            _db.CurrentTenantId = tenantId;
            var forms = await _db.FormDefinitions.ToListAsync();
            return Ok(forms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForm(Guid id)
        {
            var form = await _db.FormDefinitions.FindAsync(id);
            if (form == null) return NotFound();
            return Ok(form);
        }

        [HttpPost]
        public async Task<IActionResult> CreateForm([FromBody] CreateFormRequest request)
        {
            if (request == null) return BadRequest();
            if (request.TenantId == Guid.Empty) return BadRequest("TenantId is required");

            var form = new FormDefinition
            {
                TenantId = request.TenantId,
                Name = request.Name,
                JsonDefinition = request.JsonDefinition
            };

            _db.FormDefinitions.Add(form);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForm(Guid id, [FromBody] UpdateFormRequest request)
        {
            var form = await _db.FormDefinitions.FindAsync(id);
            if (form == null) return NotFound();

            form.Name = request.Name ?? form.Name;
            form.JsonDefinition = request.JsonDefinition ?? form.JsonDefinition;

            _db.FormDefinitions.Update(form);
            await _db.SaveChangesAsync();

            return Ok(form);
        }
    }

    // DTO sınıfları - nullable uyarılarını düzelt
    public class CreateFormRequest
    {
        public Guid TenantId { get; set; }
        public required string Name { get; set; }
        public required string JsonDefinition { get; set; }
    }

    public class UpdateFormRequest
    {
        public string? Name { get; set; }
        public string? JsonDefinition { get; set; }
    }
}
