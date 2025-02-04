using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TBC.Persons.Application.Persons.Commands.Create;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Commands.Delete;

public record DeletePersonCommand(long Id) : IRequest<Result>;

public class DeletePersonCommandHandler(
    IUnitOfWork uot,
    IMapper mapper,
    IStringLocalizer<DeletePersonCommand> localizer)
    : IRequestHandler<DeletePersonCommand, Result>
{
    public async Task<Result> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await uot.PersonsRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (person is null)
            return Result.Failure(Error.NotFound);

        await uot.PersonsRepository.DeleteAsync(person, cancellationToken: cancellationToken);
        await uot.CompleteWorkAsync(cancellationToken);
        return Result.Success();
    }
}