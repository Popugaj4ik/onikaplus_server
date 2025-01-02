using onikaplus_server.Interfaces;

namespace onikaplus_server.Models;

public class BaseEntity : IBaseEntity, IAuditable, IDeletable
{
    public Guid Id { get; set; }
    public byte[] RowVersion { get; set; } = [];
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
    public bool IsDeleted { get; set; }
}