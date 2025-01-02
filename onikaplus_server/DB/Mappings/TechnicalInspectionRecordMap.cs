using Microsoft.EntityFrameworkCore.Metadata.Builders;
using onikaplus_server.Models;

namespace onikaplus_server.DB.Mappings;

public class TechnicalInspectionRecordMap : BaseMap<TechnicalInspectionRecord>
{
    public override void Configure(EntityTypeBuilder<TechnicalInspectionRecord> builder)
    {
        base.Configure(builder);
    }
}