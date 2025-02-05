using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TBC.Persons.Domain.Events;
using TBC.Persons.Infrastructure.Db.Contexts;

namespace TBC.Persons.Application.Features.EventHandlers;

[ExcludeFromCodeCoverage]
public class PersonDeletedReportingEventHandler(
    ReportingDbContext dbContext,
    ILogger<PersonDeletedReportingEventHandler> logger,
    IMapper mapper)
    : INotificationHandler<PersonDeletedEvent>
{
    public async Task Handle(PersonDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var person =
                await dbContext.Persons.FirstOrDefaultAsync(x => x.Id == notification.Id,
                    cancellationToken: cancellationToken);
            if (person is null)
            {
                logger.LogWarning("entity not found", notification);
                return;
            }

            dbContext.Persons.Remove(person);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"PersonDeletedEvent");
            throw;
        }
    }
}