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
                UserAgent = "Mozilla/5.0",
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
                UserAgent = "PostmanRuntime/7.43.0",
                CorrelationId = Guid.NewGuid().ToString()
            },
            new()
            {
                ActorId = "user-003",
                ActorName = "Ayşe Demir",
                ActorType = ActorType.User,
                ActionType = ActionType.Created,
                EntityName = "Order",
                EntityId = "ord-101",
                NewValues = """{"orderNo":"ORD-101","status":"Created"}""",
                IpAddress = "192.168.1.15",
                UserAgent = "Mozilla/5.0",
                CorrelationId = Guid.NewGuid().ToString()
            },
            new()
            {
                ActorId = "user-004",
                ActorName = "Zeynep Kaya",
                ActorType = ActorType.User,
                ActionType = ActionType.Logout,
                EntityName = "Session",
                EntityId = "sess-2",
                IpAddress = "192.168.1.20",
                UserAgent = "Mozilla/5.0",
                CorrelationId = Guid.NewGuid().ToString()
            },
            new()
            {
                ActorId = "user-005",
                ActorName = "Ahmet Yılmaz",
                ActorType = ActorType.User,
                ActionType = ActionType.PasswordChanged,
                EntityName = "AccountSecurity",
                EntityId = "acc-sec-5",
                OldValues = """{"password":"ahmet123"}""",
                NewValues = """{"password":"ahmet456"}""",
                IpAddress = "172.16.0.5",
                UserAgent = "Mozilla/5.0",
                IsSuspicious = true,
                CorrelationId = Guid.NewGuid().ToString()
            },
            new()
            {
                ActorId = "admin-002",
                ActorName = "Selin Aksoy",
                ActorType = ActorType.User,
                ActionType = ActionType.PermissionChanged,
                EntityName = "UserPermission",
                EntityId = "perm-22",
                OldValues = """{"permission":"ReadOnly"}""",
                NewValues = """{"permission":"FullAccess"}""",
                IpAddress = "10.0.0.12",
                UserAgent = "PostmanRuntime/7.43.0",
                IsSuspicious = true,
                CorrelationId = Guid.NewGuid().ToString()
            },
            new()
            {
                ActorId = "admin-003",
                ActorName = "Burak Arslan",
                ActorType = ActorType.User,
                ActionType = ActionType.RoleAssigned,
                EntityName = "UserRole",
                EntityId = "role-77",
                OldValues = """{"role":"User"}""",
                NewValues = """{"role":"Manager"}""",
                IpAddress = "10.0.0.21",
                UserAgent = "Mozilla/5.0",
                CorrelationId = Guid.NewGuid().ToString()
            }
        };

        context.AuditLogs.AddRange(logs);
        await context.SaveChangesAsync();
    }
}