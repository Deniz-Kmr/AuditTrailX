using AuditTrailX.Core.Common;
using AuditTrailX.Core.DTOs.Request;
using AuditTrailX.Core.DTOs.Response;

namespace AuditTrailX.Core.Interfaces;

public interface IAuditLogService
{
    // burada yeni audit kaydı oluşturma işini service katmanına tanımlıyorum
    Task<AuditLogResponse> CreateAsync(CreateAuditLogRequest request, string ipAddress, string userAgent);

    // burada id ile tek audit kaydı getirme işini tanımlıyorum
    Task<AuditLogResponse?> GetByIdAsync(Guid id);

    // burada filtreli ve sayfalı audit kayıtlarını getirme işini tanımlıyorum
    Task<PagedResult<AuditLogResponse>> GetAllAsync(AuditLogQueryParameters parameters);

    // burada okunma ve şüpheli olma flaglerini güncelleme işini tanımlıyorum
    Task UpdateFlagsAsync(Guid id, bool? isRead, bool? isSuspicious);

    // burada dışarı aktarma için tüm kayıtları listeleme işini tanımlıyorum
    Task<IEnumerable<AuditLogResponse>> ExportAsync(AuditLogQueryParameters parameters);
}