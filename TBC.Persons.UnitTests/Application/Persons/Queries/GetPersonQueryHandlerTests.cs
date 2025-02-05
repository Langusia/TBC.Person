using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TBC.Persons.Application.Features.Queries.Persons.GetPersonQuery;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.UnitTests.Application.Persons.Queries
{
    public class GetPersonQueryHandlerTests
    {
        private readonly GetPersonQueryHandler _handler;
        private readonly IPersonsRepository _repository;
        private readonly Fixture _fixture;

        public GetPersonQueryHandlerTests()
        {
            _repository = Substitute.For<IPersonsRepository>();
            _handler = new GetPersonQueryHandler(_repository);
            _fixture = new Fixture();
            
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetPersonQueryHandler_Should_Return_Person_When_Found()
        {
            // Arrange
            var person = _fixture.Create<Person>();
            var query = new GetPersonQuery(person.Id);

            _repository.GetPersonFullDataAsync(query.Id, Arg.Any<CancellationToken>())
                .Returns(person);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            await _repository.Received(1).GetPersonFullDataAsync(query.Id, Arg.Any<CancellationToken>());
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(person);
        }

        [Fact]
        public async Task GetPersonQueryHandler_Should_Return_Null_When_Person_Not_Found()
        {
            // Arrange
            var query = new GetPersonQuery(123);

            _repository.GetPersonFullDataAsync(query.Id, Arg.Any<CancellationToken>())
                .Returns((Person?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            await _repository.Received(1).GetPersonFullDataAsync(query.Id, Arg.Any<CancellationToken>());
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeNull();
        }
    }
}