using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using onikaplus_server.Models;

namespace onikaplus_server.DB.Mappings;

public abstract class BaseMap<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.RowVersion).IsRowVersion();

        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
    }
}