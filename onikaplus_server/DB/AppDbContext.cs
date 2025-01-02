using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using onikaplus_server.Extentions;
using onikaplus_server.Interfaces;
using onikaplus_server.Models;
using System.Linq.Expressions;

namespace onikaplus_server.DB;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public DbSet<TechnicalInspectionRecord> TechnicalInspectionRecords { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        SetGlobalQueryFilters(modelBuilder);

        modelBuilder.ApplyAllConfigurations();

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, but should be sufficient for most use cases.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                               || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }
    }

    public override int SaveChanges()
    {
        ApplyAuditableRules();
        ApplyDeletableRules();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditableRules();
        ApplyDeletableRules();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(IDeletable).IsAssignableFrom(e.ClrType));

        foreach (var entityType in entityTypes)
        {
            var parameter = Expression.Parameter(entityType.ClrType);
            var property = Expression.Property(parameter, nameof(IDeletable.IsDeleted));
            var filter = Expression.NotEqual(property, Expression.Constant(true));
            var lambda = Expression.Lambda(filter, parameter);

            entityType.SetQueryFilter(lambda);
        }
    }

    private void ApplyAuditableRules()
    {
        ChangeTracker.Entries<IAuditable>()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified))
            .AsParallel()
            .ForAll(entity =>
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.Created = DateTimeOffset.UtcNow;
                        entity.Entity.Updated = entity.Entity.Created;
                        break;

                    case EntityState.Modified:
                        entity.Entity.Updated = DateTimeOffset.UtcNow;
                        break;
                }
            });
    }

    private void ApplyDeletableRules()
    {
        ChangeTracker.Entries<IDeletable>()
            .Where(e => e.State == EntityState.Deleted)
            .AsParallel()
            .ForAll(entity =>
            {
                entity.State = EntityState.Modified;
                entity.Entity.IsDeleted = true;
            });
    }
}