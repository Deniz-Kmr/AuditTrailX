using AuditTrailX.Core.Entities;
using AuditTrailX.Core.Enums;

namespace AuditTrailX.Data.Context;

public static class DataSeeder
{
    public static async Task SeedAsync(AuditTrailXDbContext context)
    {
        // zaten veri varsa seed etmiyorum her uygulama başlangıcında tekrar eklemesin
        if (context.AuditLogs.Any()) return;

        var logs = new List<AuditLog>
        {
            new()
            {
                ActorId = "user-001",
                ActorName = "Deniz Çelik",
                ActorType = ActorType.User,
                ActionType = ActionType.Login,
                EntityName = "Session",
                EntityId = "sess-1",
                IpAddress = "192.168.1.1",
                UserAgent = "Mozilla/5.0",
                CorrelationId = Guid.NewGuid().ToString()
            },
            new()
            {
                ActorId = "user-002",
                ActorName = "Kamer Çelik",
                ActorType = ActorType.User,
                ActionType = ActionType.Updated,
                EntityName = "UserProfile",
                EntityId = "user-002",
                // eski ve yeni değerleri json string olarak veriyorum
                OldValues = """{"email":"kamer@old.com"}""",
                NewValues = """{"email":"kamer@new.com"}""",
                IpAddress = "192.168.1.2",
                CorrelationId = Guid.NewGuid().ToString()
            },
            new()
            {
                ActorId = "admin-001",
                ActorName = "Mert Çelik",
                ActorType = ActorType.User,
                ActionType = ActionType.Deleted,
                EntityName = "Product",
                EntityId = "prod-55",
                IpAddress = "10.0.0.1",
                // şüpheli olarak işaretlenmiş bir kayıt ekliyorum
                IsSuspicious = true,
                CorrelationId = Guid.NewGuid().ToString()
            }
        };

        context.AuditLogs.AddRange(logs);
        await context.SaveChangesAsync();
    }
}