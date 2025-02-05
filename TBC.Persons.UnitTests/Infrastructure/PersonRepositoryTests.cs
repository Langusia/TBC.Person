using AutoFixture;
using FluentAssertions;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Values;
using TBC.Persons.Infrastructure.Db.Contexts;
using TBC.Persons.Infrastructure.Db.Repositories;
using TBC.Persons.UnitTests.Fixtures;

namespace TBC.Persons.UnitTests.Infrastructure;

public class PersonRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly PersonRepository _repository;
    private readonly Fixture _fixture;

    public PersonRepositoryTests()
    {
        var fixture = new InMemoryDbContextFixture();
        _context = fixture.Context;
        _repository = new PersonRepository(_context);
        
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetPersonFullDataAsync_WhenPersonExists_ReturnsPersonWithRelatedEntities()
    {
        // Arrange
        var person = _fixture.Build<Person>()
            .Without(p => p.Id)
            .Without(p => p.RelatedPersons)
            .Without(p => p.PhoneNumbers)
            .Create();

        var phoneNumbers = _fixture.Build<PhoneNumber>().Without(p => p.Id).CreateMany(2).ToList();

        person.RelatedPersons = _fixture.CreateMany<RelatedPerson>().ToList();
        person.PhoneNumbers = phoneNumbers;

        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPersonFullDataAsync(person.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Should().BeEquivalentTo(person, options => options
            .Including(p => p.Id)
            .Including(p => p.RelatedPersons)
            .Including(p => p.PhoneNumbers)
        );
    }
    
    [Fact]
    public async Task GetByPersonalNumberAsync_WhenPersonExists_ReturnsPerson()
    {
        // Arrange
        var person = _fixture.Build<Person>()
            .Without(p => p.Id)
            .With(p => p.PersonalNumber, "12345678901")
            .Create();

        _context.Persons.Add(person);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByPersonalNumberAsync("12345678901", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(person);
    }

    [Fact]
    public async Task GetPersonsAsync_WhenMatchingPersonsExist_ReturnsPaginatedList()
    {
        // Arrange
        var persons = _fixture.Build<Person>().Without(p => p.Id).CreateMany(10).ToList();
        await _context.Persons.AddRangeAsync(persons);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository
            .GetPersonsAsync(null, null, null, 1, 5, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(5);
        result.TotalCount.Should().Be(10);
        result.PageIndex.Should().Be(1);
        result.PageSize.Should().Be(5);
    }
    
    [Fact]
    public async Task GetPersonsAsync_WhenFilteringByFirstName_ReturnsMatchingPersons()
    {
        // Arrange
        var person = _fixture.Build<Person>()
            .Without(p => p.Id)
            .With(p => p.FirstName, new MultiLanguageString("გიორგი", "Giorgi"))
            .Create();

        _context.Persons.Add(person);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPersonsAsync("Giorgi", null, null, 1, 5, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(1);
        result.Items.First().FirstName.Should().BeEquivalentTo(person.FirstName);
    }
}