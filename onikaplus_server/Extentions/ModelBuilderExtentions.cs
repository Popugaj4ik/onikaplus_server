using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using onikaplus_server.Interfaces;
using System.Reflection;

namespace onikaplus_server.Extentions;

public static class ModelBuilderExtentions
{
    // https://github.com/JasonGT/NorthwindTraders/blob/ae9e69564d456c57fa7a88f6aeab2154b89b35df/Northwind.Persistence/Extensions/ModelBuilderExtensions.cs
    public static void ApplyAllConfigurations(this ModelBuilder builder)
    {
        var applyConfigurationMethodInfo = builder
            .GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(m => m.Name.Equals("ApplyConfiguration", StringComparison.OrdinalIgnoreCase));

        var ret = typeof(Program).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Select(t => (t, i: t.GetInterfaces().FirstOrDefault(i => i.Name.Equals(typeof(IEntityTypeConfiguration<>).Name, StringComparison.Ordinal))))
            .Where(it => it.i != null)
            .Select(it => (et: it.i.GetGenericArguments()[0], cfgObj: Activator.CreateInstance(it.t)))
            .Select(it => applyConfigurationMethodInfo.MakeGenericMethod(it.et).Invoke(builder, new[] { it.cfgObj }))
            .ToList();
    }

    public static EntityTypeBuilder<T> MapAuditableInfo<T>(this EntityTypeBuilder<T> eb) where T : class, IAuditable
    {
        // NOTE: Maybe in the future.
        //eb.HasOne(x => x.CreatedBy)
        //    .WithMany()
        //    .HasForeignKey(x => x.CreatedById)
        //    .IsRequired(false);

        //eb.HasOne(x => x.ModifiedBy)
        //    .WithMany()
        //    .HasForeignKey(x => x.ModifiedById)
        //    .IsRequired(false);

        return eb;
    }
}