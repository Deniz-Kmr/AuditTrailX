namespace AuditTrailX.Core.DTOs.Response;

public class AuditLogResponse
{
    // burada audit kaydını dışarıya dönerken göstereceğim alanları tutuyorum
    public Guid Id { get; set; }
    public string ActorId { get; set; } = null!;
    public string ActorName { get; set; } = null!;
    public string ActorType { get; set; } = null!;
    public string ActionType { get; set; } = null!;
    public string EntityName { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? CorrelationId { get; set; }
    public bool IsRead { get; set; }
    public bool IsSuspicious { get; set; }
    public DateTime CreatedAt { get; set; }
}