using AuditTrailX.Core.Enums;

namespace AuditTrailX.Core.DTOs.Request;

public class CreateAuditLogRequest
{
    // burada dışarıdan yeni audit kaydı oluşturmak için gerekli alanları taşıyorum
    public string ActorId { get; set; } = null!;
    public string ActorName { get; set; } = null!;
    public ActorType ActorType { get; set; }
    public ActionType ActionType { get; set; }
    public string EntityName { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? CorrelationId { get; set; }
}