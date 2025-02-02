using AutoMapper;
using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Commands.Create;

public record CreatePersonCommand(
    string FirstName,
    string LastName,
    Gender Gender,
    string PersonalNumber,
    DateTime DateOfBirth,
    int CityId,
    List<PhoneNumber> PhoneNumbers,
    string PicturePath,
    List<RelatedPerson> RelatedPersons) : IRequest<Result<long>>;

public class CreatePersonCommandHandler(IPersonsRepository repository, IMapper mapper)
    : IRequestHandler<CreatePersonCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var personToAdd = mapper.Map<Person>(request);
        await repository.AddAsync(personToAdd, cancellationToken);

        return Result.Success(personToAdd.Id);
    }
}