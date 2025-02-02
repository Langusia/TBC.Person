using MediatR;
using Microsoft.AspNetCore.Mvc;
using TBC.Persons.API.Abstractions;
using TBC.Persons.API.Extensions;
using TBC.Persons.Application.Persons.Commands.Create;
using TBC.Persons.Application.Persons.Commands.Update;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Messages;

namespace TBC.Persons.API;

[ApiController]
[Route("persons")]
public class PersonController : ApiController
{
    private readonly IMediator _mediator;

    public PersonController(IMediator mediator, ISender sender) : base(sender)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiServiceResponse<long>>> Create([FromBody] CreatePersonCommand request) =>
        await Result.Create(request)
            .Bind(command => Sender.Send(request))
            .Match(
                success => Ok(new SuccessApiServiceResponse()),
                HandleFailure<long>
            );

    [HttpPatch]
    public async Task<ActionResult<ApiServiceResponse<bool>>> Update([FromBody] UpdatePersonCommand request) =>
        await Result.Create(request)
            .Bind(command => Sender.Send(request))
            .Match(
                success => Ok(new SuccessApiServiceResponse<bool>(success)),
                HandleFailure<bool>
            );

    [HttpGet]
    public async Task<IActionResult> Get([FromBody] CreatePersonCommand request) =>
        Ok(await _mediator.Send(request));
}