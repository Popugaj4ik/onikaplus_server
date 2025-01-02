using NJsonSchema.Generation;

namespace onikaplus_server.Swager;

public class SwagerSchemaNameGenerator : DefaultSchemaNameGenerator, ISchemaNameGenerator
{
    public override string Generate(Type type)
    {
        return type.ToFriendlyString().Replace(".", string.Empty);
    }
}