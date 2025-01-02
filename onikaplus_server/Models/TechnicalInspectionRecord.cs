namespace onikaplus_server.Models;

public class TechnicalInspectionRecord : BaseEntity
{
    public string OwnerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string VehicleModel { get; set; } = string.Empty;
    public string StateRegistrationNumber { get; set; } = string.Empty;
    public string VehiucleVinNumber { get; set; } = string.Empty;
    public DateTimeOffset InspectionTime { get; set; } = DateTimeOffset.Now;
    public string AdditionalInfo { get; set; } = string.Empty;
}