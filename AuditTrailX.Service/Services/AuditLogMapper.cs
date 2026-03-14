using AuditTrailX.Core.DTOs.Response;
using AuditTrailX.Core.Entities;

namespace AuditTrailX.Service.Services;

public static class AuditLogMapper
{
    // burada entity yi response modeline çeviriyorum
    // enum alanlarını stringe çeviriyorum ki dışarıda daha okunabilir dursun
    public static AuditLogResponse ToResponse(AuditLog log) => new()
    {
        Id = log.Id,
        ActorId = log.ActorId,
        ActorName = log.ActorName,
        ActorType = log.ActorType.ToString(),
        ActionType = log.ActionType.ToString(),
        EntityName = log.EntityName,
        EntityId = log.EntityId,
        OldValues = log.OldValues,
        NewValues = log.NewValues,
        IpAddress = log.IpAddress,
        UserAgent = log.UserAgent,
        CorrelationId = log.CorrelationId,
        IsRead = log.IsRead,
        IsSuspicious = log.IsSuspicious,
        CreatedAt = log.CreatedAt
    };
}