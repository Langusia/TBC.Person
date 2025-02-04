using MediatR;
using Microsoft.AspNetCore.Mvc;
using TBC.Persons.API.Abstractions;
using TBC.Persons.API.Extensions;
using TBC.Persons.Application.Persons.Commands.Create;
using TBC.Persons.Application.Persons.Commands.Update;
using TBC.Persons.Application.Persons.Commands.UploadImage;
using TBC.Persons.Application.Persons.Query.GetPersonListQuery;
using TBC.Persons.Application.Persons.Query.GetPersonQuery;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Messages;

namespace TBC.Persons.API;

[ApiController]
[Route("persons")]
public class PersonController : ApiController
{
    public PersonController(ISender sender) : base(sender)
    {
    }

    [HttpPost("{personId}/upload-image")]
    public async Task<ActionResult<ApiServiceResponse<string>>> UploadImage(long personId, IFormFile image) =>
        await Result.Create(new UploadPersonImageCommand(personId, image))
            .Bind(command => Sender.Send(command))
            .Match(
                success => Ok(new SuccessApiServiceResponse<string>(success)),
                HandleFailure<string>
            );

    [HttpPost]
    public async Task<ActionResult<ApiServiceResponse<long>>> Create([FromBody] CreatePersonCommand request) =>
        await Result.Create(request)
            .Bind(command => Sender.Send(request))
            .Match(
                success => Ok(new SuccessApiServiceResponse<long>(success)),
                HandleFailure<long>
            );

    [HttpPatch]
    public async Task<ActionResult<ApiServiceResponse>> Update([FromBody] UpdatePersonCommand request) =>
        await Result.Create(request)
            .Bind(command => Sender.Send(request))
            .Match(() => Ok(new SuccessApiServiceResponse()),
                fail => HandleFailure(fail));

    [HttpGet]
    public async Task<ActionResult<ApiServiceResponse<PaginatedList<Person>>>> Get(
        [FromQuery] GetPersonListQuery request,
        CancellationToken cancellationToken) =>
        await Result.Create(request)
            .Bind(query => Sender.Send(query, cancellationToken))
            .Match(
                success => Ok(new SuccessApiServiceResponse<PaginatedList<Person>>(success)),
                HandleFailure<PaginatedList<Person>>
            );

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiServiceResponse<Person>>> Get(long id, CancellationToken cancellationToken) =>
        await Result.Create(new GetPersonQuery(id))
            .Bind(query => Sender.Send(query, cancellationToken))
            .Match(
                success => Ok(new SuccessApiServiceResponse<Person>(success)),
                HandleFailure<Person>
            );
}