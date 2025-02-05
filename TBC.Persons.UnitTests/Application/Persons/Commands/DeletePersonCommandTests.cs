using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using NSubstitute;
using TBC.Persons.Application.Features.Commands.Persons.Delete;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.UnitTests.Application.Persons.Commands;

public class DeletePersonCommandHandlerTests
{
    private readonly Fixture _fixture;
    private readonly DeletePersonCommandHandler _handler;

    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IStringLocalizer<DeletePersonCommand> _localizer = Substitute.For<IStringLocalizer<DeletePersonCommand>>();

    public DeletePersonCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _handler = new DeletePersonCommandHandler(_unitOfWork, _mapper, _localizer);
    }

    [Fact]
    public async Task DeletePersonCommandHandler_Should_Return_Failure_When_Person_Does_Not_Exist()
    {
        // Arrange
        var command = _fixture.Create<DeletePersonCommand>();

        _unitOfWork.PersonsRepository
            .GetByIdAsync(command.Id, cancellationToken: Arg.Any<CancellationToken>())
            .Returns((Person?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.PersonsRepository
            .Received(1)
            .GetByIdAsync(command.Id, cancellationToken: Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors[0].Should().BeEquivalentTo(Error.NotFound);
    }

    [Fact]
    public async Task DeletePersonCommandHandler_Should_Delete_Person_And_Return_Success_When_Person_Exists()
    {
        // Arrange
        var person = _fixture.Create<Person>();
        var command = _fixture.Build<DeletePersonCommand>().With(x => x.Id, person.Id).Create();

        _unitOfWork.PersonsRepository.GetByIdAsync(command.Id, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(person);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await _unitOfWork.PersonsRepository
            .Received(1)
            .GetByIdAsync(command.Id, cancellationToken: Arg.Any<CancellationToken>());

        Received.InOrder(async () =>
        {
            await _unitOfWork.PersonsRepository.DeleteAsync(person, Arg.Any<CancellationToken>());
            await _unitOfWork.CompleteWorkAsync(Arg.Any<CancellationToken>());
        });
    }
}