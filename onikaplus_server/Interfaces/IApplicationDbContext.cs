using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using onikaplus_server.Models;

namespace onikaplus_server.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TechnicalInspectionRecord> TechnicalInspectionRecords { get; set; }
    DatabaseFacade Database { get; }
}