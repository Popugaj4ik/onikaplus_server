using System.ComponentModel.DataAnnotations;

namespace onikaplus_server.Interfaces;

public interface IBaseEntity
{
    [Key]
    Guid Id { get; set; }

    byte[] RowVersion { get; set; }
}