using AuditTrailX.Core.Enums;

namespace AuditTrailX.Core.DTOs.Request;

public class AuditLogQueryParameters
{
    // burada filtreleme için kullanılacak alanları tutuyorum
    public string? ActorId { get; set; }
    public string? EntityName { get; set; }
    public string? EntityId { get; set; }
    public ActionType? ActionType { get; set; }
    public ActorType? ActorType { get; set; }
    public bool? IsSuspicious { get; set; }
    public bool? IsRead { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    // burada varsayılan sayfa boyutunu 20 olarak başlatıyorum
    private int _pageSize = 20;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        // burada çok büyük veri dönmemek için page size değerini en fazla 100 yapıyorum
        set => _pageSize = value > 100 ? 100 : value;
    }
}