namespace TBC.Persons.Domain.Interfaces;

public interface IPersonsRepository : IRepositoryBase<Person, long>
{
    Task<Person?> GetWithRelatedPersons(long id);
}