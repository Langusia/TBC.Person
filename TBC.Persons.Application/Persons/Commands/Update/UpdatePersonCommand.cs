using AutoMapper;
using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Commands.Update;

public record UpdatePersonCommand(
    long Id,
    string? FirstName,
    string? LastName,
    Gender? Gender,
    string? PersonalNumber,
    DateTime? DateOfBirth,
    int? CityId,
    List<PhoneNumber>? PhoneNumbers
) : IRequest<Result>;

public class UpdatePersonCommandHandler(IRepositoryBase<Person, long> repository, IMapper mapper)
    : IRequestHandler<UpdatePersonCommand, Result>
{
    public async Task<Result> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (person is null)
            return Result.Failure<bool>(Error.NotFound);

        mapper.Map(request, person);
        await repository.UpdateAsync(person, cancellationToken: cancellationToken);

        return Result.Success();
    }
}