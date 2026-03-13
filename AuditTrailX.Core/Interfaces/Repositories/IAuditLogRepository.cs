using AuditTrailX.Core.Common;
using AuditTrailX.Core.DTOs.Request;
using AuditTrailX.Core.Entities;

namespace AuditTrailX.Core.Interfaces.Repositories;

public interface IAuditLogRepository
{
    // burada audit kaydını veritabanına ekleme işini repository katmanına tanımlıyorum
    Task<AuditLog> CreateAsync(AuditLog auditLog);

    // burada id ile tek kayıt getirme işini tanımlıyorum
    Task<AuditLog?> GetByIdAsync(Guid id);

    // burada filtreli ve sayfalı kayıt listeleme işini tanımlıyorum
    Task<PagedResult<AuditLog>> GetAllAsync(AuditLogQueryParameters parameters);

    // burada okunma ve şüpheli flaglerini güncelleme işini tanımlıyorum
    Task UpdateFlagsAsync(Guid id, bool? isRead, bool? isSuspicious);
}