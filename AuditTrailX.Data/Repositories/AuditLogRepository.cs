using AuditTrailX.Core.Common;
using AuditTrailX.Core.DTOs.Request;
using AuditTrailX.Core.Entities;
using AuditTrailX.Core.Interfaces.Repositories;
using AuditTrailX.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AuditTrailX.Data.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    // burada dbcontexti constructor injection ile alıyorum
    private readonly AuditTrailXDbContext _context;

    public AuditLogRepository(AuditTrailXDbContext context)
    {
        _context = context;
    }

    public async Task<AuditLog> CreateAsync(AuditLog auditLog)
    {
        // yeni bir log ekliyorum ve kaydediyorum
        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
        return auditLog;
    }

    public async Task<AuditLog?> GetByIdAsync(Guid id)
    {
        // sadece okuma yaptığım için asnotracking kullanıyorum değişecek nesne yok performans sağlayacak bana 
        return await _context.AuditLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
       
    public async Task<PagedResult<AuditLog>> GetAllAsync(AuditLogQueryParameters parameters)
    {
        // filtreleme işlemi yapıyorum her hangi bir değer geldiyse doldur gelmediyse pas geç
        var query = _context.AuditLogs
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.ActorId))
            query = query.Where(x => x.ActorId == parameters.ActorId);

        if (!string.IsNullOrWhiteSpace(parameters.EntityName))
            query = query.Where(x => x.EntityName == parameters.EntityName);

        if (!string.IsNullOrWhiteSpace(parameters.EntityId))
            query = query.Where(x => x.EntityId == parameters.EntityId);

        if (parameters.ActionType.HasValue)
            query = query.Where(x => x.ActionType == parameters.ActionType.Value);

        if (parameters.ActorType.HasValue)
            query = query.Where(x => x.ActorType == parameters.ActorType.Value);

        if (parameters.IsSuspicious.HasValue)
            query = query.Where(x => x.IsSuspicious == parameters.IsSuspicious.Value);

        if (parameters.IsRead.HasValue)
            query = query.Where(x => x.IsRead == parameters.IsRead.Value);

        if (parameters.FromDate.HasValue)
            query = query.Where(x => x.CreatedAt >= parameters.FromDate.Value);

        if (parameters.ToDate.HasValue)
            query = query.Where(x => x.CreatedAt <= parameters.ToDate.Value);

        // burada önce toplam kayıt sayısını sonra sayfalı listeyi çekiyorum
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return new PagedResult<AuditLog>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize
        };
    }

    public async Task UpdateFlagsAsync(Guid id, bool? isRead, bool? isSuspicious)
    {
        // burada sadece flag alanlarını güncelliyorum diğer alanlara dokunmuyorum
        var log = await _context.AuditLogs.FindAsync(id);

        if (log is null)
            return;

        if (isRead.HasValue)
            log.IsRead = isRead.Value;

        if (isSuspicious.HasValue)
            log.IsSuspicious = isSuspicious.Value;

        await _context.SaveChangesAsync();
    }
}