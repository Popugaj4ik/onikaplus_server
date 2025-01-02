using MediatR;

namespace onikaplus_server.MediatR.TechincalInspectionRecord.Commands;

public class CreateNewInspectionRequest : IRequestHandler<CreateNewInspectionRequest.Command>
{
    public class Command : IRequest
    {
        public string VehicleOwnerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public string StateRegistrationNumber { get; set; } = string.Empty;
        public string VehicleVinNumber { get; set; } = string.Empty;
        public DateTimeOffset InspectionTime { get; set; } = DateTimeOffset.Now;
        public string AdditionalInfo { get; set; } = string.Empty;
    }

    public Task Handle(Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}