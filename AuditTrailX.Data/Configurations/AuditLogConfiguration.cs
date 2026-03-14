using AuditTrailX.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditTrailX.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        // burada tablo adını snake_case olarak veriyorum
        builder.ToTable("audit_logs");

        // burada primary key tanımlıyorum ve id yi postgres tarafında uuid olarak ürettiriyorum
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        // burada zorunlu alanları ve max uzunlukları belirliyorum
        builder.Property(x => x.ActorId).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ActorName).IsRequired().HasMaxLength(200);

        // burada enum alanlarını veritabanında string olarak saklıyorum okunması daha rahat olsun diye
        builder.Property(x => x.ActorType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.ActionType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.EntityName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.EntityId).IsRequired().HasMaxLength(100);

        // burada eski ve yeni değerleri jsonb olarak saklıyorum
        builder.Property(x => x.OldValues).HasColumnType("jsonb");
        builder.Property(x => x.NewValues).HasColumnType("jsonb");

        builder.Property(x => x.IpAddress).HasMaxLength(50);
        builder.Property(x => x.UserAgent).HasMaxLength(500);
        builder.Property(x => x.CorrelationId).HasMaxLength(100);

        builder.Property(x => x.IsRead).HasDefaultValue(false);
        builder.Property(x => x.IsSuspicious).HasDefaultValue(false);

        // burada createdat alanını postgres tarafında utc mantığıyla varsayılan dolduruyorum
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'");

        // burada sık filtrelenecek alanlara index ekliyorum
        builder.HasIndex(x => x.ActorId).HasDatabaseName("ix_audit_logs_actor_id");
        builder.HasIndex(x => x.EntityName).HasDatabaseName("ix_audit_logs_entity_name");
        builder.HasIndex(x => x.CreatedAt).HasDatabaseName("ix_audit_logs_created_at");
        builder.HasIndex(x => x.ActionType).HasDatabaseName("ix_audit_logs_action_type");
        builder.HasIndex(x => x.IsSuspicious).HasDatabaseName("ix_audit_logs_is_suspicious");
    }
}