using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using onikaplus_server.MediatR.TechincalInspectionRecord.Commands;

namespace onikaplus_server.Controllers;

[ApiController]
[Route("~/api/[controller]")]
public class TechnicalInspectionRecordsController : Controller
{
    [HttpPost("[action]")]
    [AllowAnonymous]
    public Task CreateNewInspectionRequest(
        [FromBody] CreateNewInspectionRequest.Command command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken) => sender.Send(command, cancellationToken);
}