namespace onikaplus_server.Interfaces;

public interface IAuditable
{
    DateTimeOffset Created { get; set; }
    DateTimeOffset Updated { get; set; }
}