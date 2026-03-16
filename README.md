# AuditTrailX

> Kurumsal uygulamalarda **kim ne yaptı, ne zaman yaptı, ne değişti** sorularını merkezi bir yerde yanıtlayan audit logging servisi.

---

## Neden AuditTrailX?

Çoğu projede veri değişiklikleri gerçekleşir ama sonradan izlemek zorlaşır. AuditTrailX bu problemi çözer:

- ✅ Değişikliği kim yaptı?
- ✅ Ne zaman yaptı?
- ✅ Eski değer neydi, yeni değer ne oldu?
- ✅ Hangi IP'den, hangi istekten geldi?

---

## Mimari
```
AuditTrailX.API      → Controller, Middleware, DI, Auth
AuditTrailX.Service  → Business logic, mapping
AuditTrailX.Data     → EF Core, Repository, Configurations, Migrations
AuditTrailX.Core     → Entities, DTOs, Interfaces, Enums
```

---

## Tech Stack

| Katman | Teknoloji |
|---|---|
| Backend | .NET 8 / ASP.NET Core Web API |
| Veritabanı | PostgreSQL + EF Core 8 |
| Mimari | Clean Architecture |
| Auth | JWT Bearer Authentication |
| Dökümantasyon | Swagger UI |
| Test | xUnit + Moq + FluentAssertions |

---

## Özellikler

- 🔐 JWT ile korunan endpoint yapısı
- 📝 Audit log oluşturma
- 🔍 Filtrelenebilir ve sayfalı listeleme (actor, entity, tarih, aksiyon tipi)
- 🚩 Flag yönetimi (`IsRead`, `IsSuspicious`)
- 📤 JSON export desteği
- ⚡ Global exception middleware
- 🔗 Correlation ID middleware

---

## Kurulum
```bash
git clone https://github.com/Deniz-Kmr/AuditTrailX.git
cd AuditTrailX
```

`appsettings.Development.json` oluştur:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=YOUR_DATABASE_NAME;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  },
  "JwtSettings": {
    "SecretKey": "YOUR-SECRET-KEY-MIN-32-CHARACTERS-LONG",
    "Issuer": "AuditTrailX",
    "Audience": "AuditTrailX.Client",
    "ExpirationMinutes": 60
  }
}
```

Migration ve veritabanını güncelle:
```bash
cd AuditTrailX.API
dotnet ef database update --project ../AuditTrailX.Data --startup-project .
dotnet run
```

Swagger UI: `https://localhost:{PORT}/swagger`

---

## Endpoint'ler

| Method | Endpoint | Açıklama |
|---|---|---|
| POST | /api/auth/token | JWT token al |
| POST | /api/auditlogs | Yeni log oluştur |
| GET | /api/auditlogs | Filtreli ve sayfalı liste |
| GET | /api/auditlogs/{id} | Tek kayıt detayı |
| PATCH | /api/auditlogs/{id}/flags | Flag güncelle |
| GET | /api/auditlogs/export | JSON export |

---

## Örnek İstek

Token al:
```bash
curl -X POST https://localhost:{PORT}/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{"username":"deniz","password":"deniz1234"}'
```

Log oluştur:
```bash
curl -X POST https://localhost:{PORT}/api/auditlogs \
  -H "Authorization: Bearer {TOKEN}" \
  -H "Content-Type: application/json" \
  -d '{
    "actorId": "user-1",
    "actorName": "Deniz Çelik",
    "actorType": 1,
    "actionType": 2,
    "entityName": "UserProfile",
    "entityId": "user-1",
    "oldValues": "{\"email\":\"eski@mail.com\"}",
    "newValues": "{\"email\":\"yeni@mail.com\"}"
  }'
```