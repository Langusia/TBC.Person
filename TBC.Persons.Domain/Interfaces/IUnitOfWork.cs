namespace TBC.Persons.Domain.Interfaces;

public interface IUnitOfWork
{
    IPersonsRepository PersonsRepository { get; }
    ICityRepository CityRepository { get; }
    Task<int> CompleteWorkAsync(CancellationToken token);
}