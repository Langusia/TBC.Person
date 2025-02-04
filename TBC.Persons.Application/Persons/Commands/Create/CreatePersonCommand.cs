using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Commands.Create;

public record CreatePersonCommand(
    string FirstName,
    string LastName,
    string? FirstNameEng,
    string? LastNameEng,
    Gender Gender,
    string PersonalNumber,
    DateTime DateOfBirth,
    int CityId,
    List<PhoneNumber> PhoneNumbers,
    string? PicturePath) : IRequest<Result<long>>;

public class CreatePersonCommandHandler(
    IUnitOfWork uot,
    IMapper mapper,
    IStringLocalizer<CreatePersonCommand> localizer)
    : IRequestHandler<CreatePersonCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var maybePerson =
            await uot.PersonsRepository.GetByPersonalNumberAsync(request.PersonalNumber, cancellationToken);
        if (maybePerson is not null)
            return Result.Failure<long>(Error.AlreadyExists(localizer["person with same number already exists"]));

        var city = await uot.CityRepository.GetByIdAsync(request.CityId, cancellationToken: cancellationToken);
        if (city is null)
            return Result.Failure<long>(new Error("Error.NotFound", localizer["city with given id not found"],
                ErrorTypeEnum.BadRequest));

        var personToAdd = mapper.Map<Person>(request);

        var id = await uot.PersonsRepository.AddAsync(personToAdd, cancellationToken);
        await uot.CompleteWorkAsync(cancellationToken);
        return Result.Success(personToAdd.Id);
    }
}