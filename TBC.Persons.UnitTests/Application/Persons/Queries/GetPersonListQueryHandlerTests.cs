using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TBC.Persons.Application.Features.Queries.Persons.GetPersonListQuery;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Domain.Values;

namespace TBC.Persons.UnitTests.Application.Persons.Queries;

public class GetPersonListQueryHandlerTests
{
    private readonly IPersonsRepository _repository = Substitute.For<IPersonsRepository>();
    private readonly GetPersonListQueryHandler _handler;
    private readonly Fixture _fixture;

    public GetPersonListQueryHandlerTests()
    {
        _fixture = new Fixture();
        _handler = new GetPersonListQueryHandler(_repository);

        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetPersonListQueryHandler_Should_Return_Empty_List_When_No_Matching_Records()
    {
        // Arrange
        var query = _fixture.Build<GetPersonListQuery>()
            .With(q => q.FirstName, "NonExistingName")
            .With(q => q.LastName, "NonExistingLastName")
            .With(q => q.PersonalNumber, "00000000000")
            .With(q => q.pageSize, 10)
            .With(q => q.pageIndex, 1)
            .Create();

        var emptyList = new PaginatedList<Person>(new List<Person>(), 0, query.pageIndex, query.pageSize);

        _repository.GetPersonsAsync(
                query.FirstName,
                query.LastName,
                query.PersonalNumber,
                query.pageIndex,
                query.pageSize,
                Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(emptyList));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
       await _repository.Received(1).GetPersonsAsync(
            query.FirstName,
            query.LastName,
            query.PersonalNumber,
            query.pageIndex,
            query.pageSize,
            Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPersonListQueryHandler_Should_Return_Persons_When_They_Exist()
    {
        // Arrange
        var persons = _fixture.CreateMany<Person>(5).ToList();
        var paginatedPersons = new PaginatedList<Person>(persons, persons.Count, 1, 10);

        var query = _fixture.Build<GetPersonListQuery>()
            .With(q => q.FirstName, "Giorgi")
            .With(q => q.LastName, "Giorgadze")
            .With(q => q.PersonalNumber, "12345678901")
            .With(q => q.pageSize, 10)
            .With(q => q.pageIndex, 1)
            .Create();

        _repository.GetPersonsAsync(
                query.FirstName,
                query.LastName,
                query.PersonalNumber,
                query.pageIndex,
                query.pageSize,
                Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(paginatedPersons));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _repository.Received(1).GetPersonsAsync(
            query.FirstName,
            query.LastName,
            query.PersonalNumber,
            query.pageIndex,
            query.pageSize,
            Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetPersonListQueryHandler_Should_Return_Empty_When_Persons_Are_Not_Found()
    {
        // Arrange
        var query = _fixture.Build<GetPersonListQuery>()
            .With(q => q.pageSize, 10)
            .With(q => q.pageIndex, 1)
            .Create();

        _repository.GetPersonsAsync(
                query.FirstName,
                query.LastName,
                query.PersonalNumber,
                query.pageIndex,
                query.pageSize,
                Arg.Any<CancellationToken>())!
            .Returns(Task.FromResult(new PaginatedList<Person>(new List<Person>(), 0, query.pageIndex, query.pageSize)));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await _repository.Received(1).GetPersonsAsync(
            query.FirstName,
            query.LastName,
            query.PersonalNumber,
            query.pageIndex,
            query.pageSize,
            Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEmpty();
    }
}