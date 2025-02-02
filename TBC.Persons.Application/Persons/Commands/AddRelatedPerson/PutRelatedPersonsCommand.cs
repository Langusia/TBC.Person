using AutoMapper;
using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Commands.AddRelatedPerson;

public record PutRelatedPersonsCommand(long Id, List<PutRelatedPerson> RelatedPersons) : IRequest<Result<bool>>;

public class PutRelatedPersonsCommandHandler(IPersonsRepository repository, IMapper mapper)
    : IRequestHandler<PutRelatedPersonsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(PutRelatedPersonsCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.GetWithRelatedPersons(request.Id);
        if (person is null)
            return Result.Failure<bool>(Error.NotFound);

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
        await repository.UpdateAsync(person, cancellationToken: cancellationToken);

        return Result.Success(true);
    }
}