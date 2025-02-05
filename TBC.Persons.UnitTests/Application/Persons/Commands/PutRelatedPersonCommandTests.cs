using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TBC.Persons.Application.Features.Commands.Persons.PutRelatedPerson;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.UnitTests.Application.Persons.Commands;

public class PutRelatedPersonsCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly PutRelatedPersonsCommandHandler _handler;
    private readonly Fixture _fixture;

    public PutRelatedPersonsCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _handler = new PutRelatedPersonsCommandHandler(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task PutRelatedPersonsCommandHandler_Should_Return_Failure_When_Persons_Not_Found()
    {
        // Arrange
        var command = _fixture.Create<PutRelatedPersonCommand>();

        _unitOfWork.PersonsRepository.GetByIdsAsync(
                Arg.Any<List<long>>(), false, Arg.Any<CancellationToken>())!
            .Returns((List<Person>?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().BeEquivalentTo(Error.NotFound);
    }

    [Fact]
    public async Task PutRelatedPersonsCommandHandler_Should_Return_Success_When_Relation_Already_Exists_With_Same_Type()
    {
        // Arrange
        var person = _fixture.Create<Person>();
        var relatedPerson = _fixture.Create<Person>();

        var existingRelation = new RelatedPerson
        {
            RelatedPersonId = relatedPerson.Id,
            PersonId = person.Id,
            RelationType = RelationType.Colleague
        };

        person.RelatedPersons = new List<RelatedPerson> { existingRelation };

        var command = new PutRelatedPersonCommand(person.Id, RelationType.Colleague, relatedPerson.Id);

        _unitOfWork.PersonsRepository.GetByIdsAsync(
                Arg.Any<List<long>>(), false, Arg.Any<CancellationToken>())
            .Returns(new List<Person> { person, relatedPerson });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        person.RelatedPersons.Should().ContainSingle(x => x.RelatedPersonId == relatedPerson.Id);
        person.RelatedPersons.First().RelationType.Should().Be(RelationType.Colleague);

        await _unitOfWork.PersonsRepository.DidNotReceive()
            .UpdateAsync(Arg.Any<Person>(), cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PutRelatedPersonsCommandHandler_Should_Update_Relation_When_Relation_Exists_But_Type_Differs()
    {
        // Arrange
        var person = _fixture.Create<Person>();
        var relatedPerson = _fixture.Create<Person>();

        var existingRelation = new RelatedPerson
        {
            RelatedPersonId = relatedPerson.Id,
            PersonId = person.Id,
            RelationType = RelationType.Relative
        };

        person.RelatedPersons = new List<RelatedPerson> { existingRelation };

        var command = new PutRelatedPersonCommand(person.Id, RelationType.Colleague, relatedPerson.Id);

        _unitOfWork.PersonsRepository.GetByIdsAsync(
                Arg.Any<List<long>>(), false, Arg.Any<CancellationToken>())
            .Returns(new List<Person> { person, relatedPerson });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        person.RelatedPersons.Should().ContainSingle(x => x.RelatedPersonId == relatedPerson.Id);
        person.RelatedPersons.First().RelationType.Should().Be(RelationType.Colleague);

        await _unitOfWork.PersonsRepository.Received(1)
            .UpdateAsync(person, cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PutRelatedPersonsCommandHandler_Should_Add_New_Relation_When_Not_Exists()
    {
        // Arrange
        var person = _fixture.Create<Person>();
        var relatedPerson = _fixture.Create<Person>();

        person.RelatedPersons = new List<RelatedPerson>();

        var command = new PutRelatedPersonCommand(person.Id, RelationType.Relative, relatedPerson.Id);

        _unitOfWork.PersonsRepository.GetByIdsAsync(
                Arg.Any<List<long>>(), false, Arg.Any<CancellationToken>())
            .Returns(new List<Person> { person, relatedPerson });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        person.RelatedPersons.Should().ContainSingle(x => x.RelatedPersonId == relatedPerson.Id);
        person.RelatedPersons.First().RelationType.Should().Be(RelationType.Relative);

        await _unitOfWork.PersonsRepository.Received(1)
            .UpdateAsync(person, cancellationToken: Arg.Any<CancellationToken>());
    }
}