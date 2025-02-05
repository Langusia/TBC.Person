using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using NSubstitute;
using TBC.Persons.Application.Features.Commands.Persons.Create;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.UnitTests.Application.Persons.Commands;

public class CreatePersonCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IStringLocalizer<CreatePersonCommand> _localizer = Substitute.For<IStringLocalizer<CreatePersonCommand>>();
    private readonly CreatePersonCommandHandler _handler;
    private readonly Fixture _fixture;

    public CreatePersonCommandHandlerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _handler = new CreatePersonCommandHandler(_unitOfWork, _mapper, _localizer);
    }

    [Fact]
    public async Task CreatePersonCommandHandler_Should_Return_Failure_When_Person_With_Same_PersonalNumber_Exists()
    {
        // Arrange
        var existingPerson = _fixture.Create<Person>();
        var command = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.PersonalNumber, existingPerson.PersonalNumber)
            .Create();

        _unitOfWork.PersonsRepository.GetByPersonalNumberAsync(command.PersonalNumber, Arg.Any<CancellationToken>())
            .Returns(existingPerson);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.PersonsRepository
            .Received(1)
            .GetByPersonalNumberAsync(command.PersonalNumber, Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreatePersonCommandHandler_Should_Return_Failure_When_City_Does_Not_Exist()
    {
        // Arrange
        var command = _fixture.Build<CreatePersonCommand>().Create();

        _unitOfWork.PersonsRepository
            .GetByPersonalNumberAsync(command.PersonalNumber, Arg.Any<CancellationToken>())
            .Returns((Person?)null);

        _unitOfWork.CityRepository
            .GetByIdAsync(
                command.CityId,
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns((City?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.PersonsRepository
            .Received(1)
            .GetByPersonalNumberAsync(command.PersonalNumber, Arg.Any<CancellationToken>());
        
        await _unitOfWork.CityRepository
            .Received(1)
            .GetByIdAsync(
                command.CityId,
                cancellationToken: Arg.Any<CancellationToken>());
        
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreatePersonCommandHandler_Should_Create_Person_And_Return_Id_When_Valid()
    {
        // Arrange
        var command = _fixture.Build<CreatePersonCommand>().Create();
        var mappedPerson = _fixture.Build<Person>().With(x => x.Id, 1).Create();
        var city = _fixture.Create<City>();

        _unitOfWork.PersonsRepository
            .GetByPersonalNumberAsync(
                command.PersonalNumber,
                Arg.Any<CancellationToken>())
            .Returns((Person?)null);

        _unitOfWork.CityRepository
            .GetByIdAsync(
                command.CityId,
                true,
                true,
                Arg.Any<CancellationToken>())
            .Returns(city);

        _mapper.Map<Person>(command).Returns(mappedPerson);

        _unitOfWork.PersonsRepository.AddAsync(mappedPerson, Arg.Any<CancellationToken>()).Returns(mappedPerson.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(true);
        result.Value.Should().Be(1);

        await _unitOfWork.PersonsRepository
            .Received(1)
            .GetByPersonalNumberAsync(command.PersonalNumber,
                Arg.Any<CancellationToken>());

        await _unitOfWork.CityRepository
            .Received(1)
            .GetByIdAsync(
                command.CityId,
                true,
                true,
                Arg.Any<CancellationToken>());

        await _unitOfWork.PersonsRepository
            .Received(1)
            .AddAsync(mappedPerson, Arg.Any<CancellationToken>());

        Received.InOrder(async void () =>
        {
            await _unitOfWork.PersonsRepository.AddAsync(mappedPerson, Arg.Any<CancellationToken>());
            await _unitOfWork.CompleteWorkAsync(Arg.Any<CancellationToken>());
        });
    }
}