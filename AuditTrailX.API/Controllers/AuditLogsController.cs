using AuditTrailX.Core.DTOs.Request;
using AuditTrailX.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditTrailX.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditLogsController : ControllerBase
{
    // burada servisi interface üzerinden alıyorum
    private readonly IAuditLogService _service;

    public AuditLogsController(IAuditLogService service) => _service = service;

    /// <summary>
    /// Yeni audit log kaydı oluşturur 
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuditLogRequest request)
    {
        // ip ve user agentı http context ten alıyorum
        // bu bilgilere sadece controller erişebilir servise parametre olarak geçiyorum
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = Request.Headers.UserAgent.ToString();

        var result = await _service.CreateAsync(request, ipAddress, userAgent);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Filtrelenebilir ve sayfalı audit log listesi döner
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AuditLogQueryParameters parameters)
    {
        var result = await _service.GetAllAsync(parameters);
        return Ok(result);
    }

    /// <summary>
    /// ID'ye göre tek audit log kaydı döner
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>
    /// IsRead ve IsSuspicious flaglerini günceller Audit log immutability korunur
    /// </summary>
    [HttpPatch("{id:guid}/flags")]
    public async Task<IActionResult> UpdateFlags(Guid id, [FromBody] UpdateFlagsRequest request)
    {
        await _service.UpdateFlagsAsync(id, request.IsRead, request.IsSuspicious);
        return NoContent();
    }

    /// <summary>
    /// Filtrelenmiş kayıtları JSON olarak export eder
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] AuditLogQueryParameters parameters)
    {
        var result = await _service.ExportAsync(parameters);
        return Ok(result);
    }
}

public record UpdateFlagsRequest(bool? IsRead, bool? IsSuspicious);