using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Events;
using TBC.Persons.Infrastructure.Db.Contexts;

namespace TBC.Persons.Application.Features.EventHandlers;

[ExcludeFromCodeCoverage]
public class PersonCreatedReportingEventHandler(
    ReportingDbContext dbContext,
    IMapper mapper,
    ILogger<PersonCreatedReportingEventHandler> logger
)
    : INotificationHandler<PersonCreatedEvent>
{
    public async Task Handle(PersonCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var personToAdd = mapper.Map<Person>(notification);
            await dbContext.Persons.AddAsync(personToAdd, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"PersonCreatedEvent");
            throw;
        }
    }
}