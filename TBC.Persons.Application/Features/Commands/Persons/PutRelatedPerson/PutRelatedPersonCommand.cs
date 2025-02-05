using AutoMapper;
using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Events;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Features.Commands.Persons.PutRelatedPerson;

public record PutRelatedPersonCommand(long Id, RelationType RelationType, long RelatedPersonId) : IRequest<Result>;

public class PutRelatedPersonsCommandHandler(IUnitOfWork uot, IMapper mapper)
    : IRequestHandler<PutRelatedPersonCommand, Result>
{
    public async Task<Result> Handle(PutRelatedPersonCommand request, CancellationToken cancellationToken)
    {
        List<long> ids = [request.Id, request.RelatedPersonId];
        var persons = await uot.PersonsRepository.GetByIdsAsync(ids, false, cancellationToken);
        if (persons is null || persons.Count != 2)
            return Result.Failure(Error.NotFound);

        var masterPerson = persons.FirstOrDefault(x => x.Id == request.Id);
        var relative = persons.FirstOrDefault(x => x.Id != request.Id);

        var existingRelation = masterPerson!.RelatedPersons
            .FirstOrDefault(x => x.RelatedPersonId == request.RelatedPersonId);

        if (existingRelation != null)
        {
            if (existingRelation.RelationType == request.RelationType)
                return Result.Success();

            existingRelation.RelationType = request.RelationType;
            masterPerson.AddDomainEvent(new PersonRelationUpdatedEvent(existingRelation.Id, request.RelationType));
        }
        else
        {
            var newRelation = new RelatedPerson
            {
                RelationType = request.RelationType,
                RelatedPersonId = request.RelatedPersonId,
                PersonId = request.Id
            };

            masterPerson.RelatedPersons.Add(newRelation);
            masterPerson.AddDomainEvent(new PersonRelationCreatedEvent(request.Id, request.RelationType,
                request.RelatedPersonId));
        }

        await uot.PersonsRepository.UpdateAsync(masterPerson, cancellationToken: cancellationToken);
        await uot.CompleteWorkAsync(cancellationToken);

        return Result.Success();
    }
}