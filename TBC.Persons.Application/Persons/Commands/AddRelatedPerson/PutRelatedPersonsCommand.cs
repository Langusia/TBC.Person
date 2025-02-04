using AutoMapper;
using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Commands.AddRelatedPerson;

public record PutRelatedPersonsCommand(long Id, List<PutRelatedPerson> RelatedPersons) : IRequest<Result<bool>>;

public class PutRelatedPersonsCommandHandler(IUnitOfWork uot, IMapper mapper)
    : IRequestHandler<PutRelatedPersonsCommand, Result>
{
    public async Task<Result> Handle(PutRelatedPersonsCommand request, CancellationToken cancellationToken)
    {
        var person = await uot.PersonsRepository.GetPersonFullDataAsync(request.Id, cancellationToken);
        if (person is null)
            return Result.Failure(Error.NotFound);

        // Map incoming related persons to entity models
        var incomingRelations = request.RelatedPersons
            .Select(rp => new RelatedPerson
            {
                RelatedPersonId = rp.Id,
                RelationType = rp.RelationType,
                PersonId = request.Id
            })
            .ToList();

        // Remove related persons that are no longer present
        person.RelatedPersons.Where(existing =>
                !incomingRelations.Any(incoming => incoming.RelatedPersonId == existing.RelatedPersonId)).ToList()
            .ForEach(x => x.IsActive = false);

        // Add new related persons
        foreach (var incoming in incomingRelations)
        {
            var existingRelation = person.RelatedPersons
                .FirstOrDefault(rp => rp.RelatedPersonId == incoming.RelatedPersonId);

            if (existingRelation is null)
            {
                // Add new relation
                person.RelatedPersons.Add(incoming);
            }
            else
            {
                // Update existing relation if needed
                existingRelation.RelationType = incoming.RelationType;
            }
        }

        // Persist changes
        await uot.PersonsRepository.UpdateAsync(person, cancellationToken: cancellationToken);
        await uot.CompleteWorkAsync(cancellationToken);
        return Result.Success();
    }
}