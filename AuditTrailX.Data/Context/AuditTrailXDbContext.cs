using Microsoft.EntityFrameworkCore;
using AuditTrailX.Core.Entities;
using AuditTrailX.Data.Configurations;

namespace AuditTrailX.Data.Context;

public class AuditTrailXDbContext : DbContext
{
    public AuditTrailXDbContext(DbContextOptions<AuditTrailXDbContext> options) : base(options)
    {
    }
    
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
    }
}