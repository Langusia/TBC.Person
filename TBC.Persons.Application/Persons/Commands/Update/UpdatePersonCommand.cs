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
    string? FirstNameEng,
    string? LastNameEng,
    Gender? Gender,
    string? PersonalNumber,
    DateTime? DateOfBirth,
    int? CityId,
    List<PhoneNumber>? PhoneNumbers
) : IRequest<Result>;

public class UpdatePersonCommandHandler(IUnitOfWork uot, IMapper mapper)
    : IRequestHandler<UpdatePersonCommand, Result>
{
    public async Task<Result> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await uot.PersonsRepository.GetByIdAsync(request.Id, false, cancellationToken: cancellationToken);
        if (person is null)
            return Result.Failure<bool>(Error.NotFound);

        mapper.Map(request, person);
        await uot.PersonsRepository.UpdateAsync(person, cancellationToken: cancellationToken);
        await uot.CompleteWorkAsync(cancellationToken);
        return Result.Success();
    }
}