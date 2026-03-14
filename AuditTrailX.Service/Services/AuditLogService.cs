using AuditTrailX.Core.Common;
using AuditTrailX.Core.DTOs.Request;
using AuditTrailX.Core.DTOs.Response;
using AuditTrailX.Core.Entities;
using AuditTrailX.Core.Interfaces;
using AuditTrailX.Core.Interfaces.Repositories;

namespace AuditTrailX.Service.Services;

public class AuditLogService : IAuditLogService
{
    // repositoryyi interface üzerinden alıyorum
    private readonly IAuditLogRepository _repository;

    public AuditLogService(IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<AuditLogResponse> CreateAsync(
        CreateAuditLogRequest request,
        string ipAddress,
        string userAgent)
    {
        // request verisini entityye çeviriyorum
        // correlation id gelmezse kendim üretiyorum
        var log = new AuditLog
        {
            ActorId = request.ActorId,
            ActorName = request.ActorName,
            ActorType = request.ActorType,
            ActionType = request.ActionType,
            EntityName = request.EntityName,
            EntityId = request.EntityId,
            OldValues = request.OldValues,
            NewValues = request.NewValues,
            CorrelationId = request.CorrelationId ?? Guid.NewGuid().ToString(),
            IpAddress = ipAddress,
            UserAgent = userAgent,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(log);
        return AuditLogMapper.ToResponse(created);
    }

    public async Task<AuditLogResponse?> GetByIdAsync(Guid id)
    {
        // burada kayıt yoksa null dönüyorum
        var log = await _repository.GetByIdAsync(id);
        return log is null ? null : AuditLogMapper.ToResponse(log);
    }

    public async Task<PagedResult<AuditLogResponse>> GetAllAsync(AuditLogQueryParameters parameters)
    {
        // repositoryden gelen entity listesini response listesine çeviriyorum
        var pagedLogs = await _repository.GetAllAsync(parameters);

        return new PagedResult<AuditLogResponse>
        {
            Items = pagedLogs.Items.Select(AuditLogMapper.ToResponse).ToList(),
            TotalCount = pagedLogs.TotalCount,
            PageNumber = pagedLogs.PageNumber,
            PageSize = pagedLogs.PageSize
        };
    }

    public async Task UpdateFlagsAsync(Guid id, bool? isRead, bool? isSuspicious)
    {
        // flag güncelleme işini repositoryye devrediyorum
        await _repository.UpdateFlagsAsync(id, isRead, isSuspicious);
    }

    public async Task<IEnumerable<AuditLogResponse>> ExportAsync(AuditLogQueryParameters parameters)
    {
        // export için tek seferde olabildiğince fazla kayıt çekiyorum
        parameters.PageNumber = 1;
        parameters.PageSize = int.MaxValue;

        var result = await _repository.GetAllAsync(parameters);
        return result.Items.Select(AuditLogMapper.ToResponse);
    }
}