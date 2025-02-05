using AutoFixture;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TBC.Persons.Application.Features.Commands.Persons.Update;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.UnitTests.Application.Persons.Commands;

public class UpdatePersonCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly IRepositoryBase<Person, long> _repository = Substitute.For<IRepositoryBase<Person, long>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly UpdatePersonCommandHandler _handler;
    private readonly Fixture _fixture;

    public UpdatePersonCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _handler = new UpdatePersonCommandHandler(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task UpdatePersonCommandHandler_Should_Return_Failure_When_Person_Does_Not_Exist()
    {
        // Arrange
        var command = _fixture.Create<UpdatePersonCommand>();

        _unitOfWork.PersonsRepository.GetByIdAsync(command.Id, true, cancellationToken: Arg.Any<CancellationToken>())
            .Returns((Person?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.PersonsRepository.Received(1)
            .GetByIdAsync(command.Id, false, cancellationToken: Arg.Any<CancellationToken>());
        await _unitOfWork.PersonsRepository.Received(0)
            .UpdateAsync(Arg.Any<Person>(), cancellationToken: Arg.Any<CancellationToken>());

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().BeEquivalentTo(Error.NotFound);
    }

    [Fact]
    public async Task UpdatePersonCommandHandler_Should_Update_Person_And_Return_Success_When_Person_Exists()
    {
        // Arrange
        var person = _fixture.Create<Person>();
        var command = _fixture.Build<UpdatePersonCommand>().With(x => x.Id, person.Id).Create();

        _unitOfWork.PersonsRepository.GetByIdAsync(command.Id, false, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(person);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await _unitOfWork.PersonsRepository.Received(1)
            .GetByIdAsync(command.Id, false, cancellationToken: Arg.Any<CancellationToken>());
        await _unitOfWork.PersonsRepository.Received(1)
            .UpdateAsync(person, cancellationToken: Arg.Any<CancellationToken>());

        Received.InOrder(async () =>
        {
            _mapper.Map(command, person);
            await _unitOfWork.PersonsRepository.UpdateAsync(person, cancellationToken: Arg.Any<CancellationToken>());
        });
    }
}