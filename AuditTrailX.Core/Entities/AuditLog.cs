using AuditTrailX.Core.Enums;

namespace AuditTrailX.Core.Entities;

public class AuditLog
{
    public Guid Id { get; set; }

    // burada işlemi yapan kullanıcı servis ya da sistem bilgisini tutuyorum
    public string ActorId { get; set; } = null!;
    public string ActorName { get; set; } = null!;
    public ActorType ActorType { get; set; }

    // burada yapılan aksiyonun türünü tutuyorum
    public ActionType ActionType { get; set; }

    // burada işlemin hangi kayıt üstünde yapıldığını tutuyorum
    public string EntityName { get; set; } = null!;
    public string EntityId { get; set; } = null!;

    // burada eski ve yeni değerleri json string olarak saklayacağım
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }

    // burada isteğin geldiği ortam bilgisini tutuyorum
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? CorrelationId { get; set; }

    // burada kaydın okunup okunmadığını ve şüpheli olup olmadığını işaretliyorum
    public bool IsRead { get; set; } = false;
    public bool IsSuspicious { get; set; } = false;

    // burada audit kaydının oluşturulma zamanını utc olarak tutuyorum
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}