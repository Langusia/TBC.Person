using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using NSubstitute;
using System.Text;
using Microsoft.AspNetCore.Http;
using TBC.Persons.Application.Features.Commands.Persons.UploadImage;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.UnitTests.Application.Persons.Commands;

public class UploadPersonImageCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IWebHostEnvironment _environment = Substitute.For<IWebHostEnvironment>();
    private readonly UploadPersonImageCommandHandler _handler;
    private readonly Fixture _fixture;
    private readonly string _tempDirectory;

    public UploadPersonImageCommandHandlerTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "UploadTests");
        if (!Directory.Exists(_tempDirectory))
        {
            Directory.CreateDirectory(_tempDirectory);
        }

        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _environment.ContentRootPath.Returns(_tempDirectory);
        _handler = new UploadPersonImageCommandHandler(_unitOfWork, _environment);
    }

    [Fact]
    public async Task UploadPersonImageCommandHandler_Should_Create_Upload_Directory_If_Not_Exists()
    {
        // Arrange
        var uploadPath = Path.Combine(_tempDirectory, "uploads");
        if (Directory.Exists(uploadPath))
        {
            Directory.Delete(uploadPath, true);
        }

        var person = _fixture.Build<Person>()
            .With(x => x.PicturePath, (string?)null)
            .Create();

        var command = new UploadPersonImageCommand(1, CreateMockFile("test.jpg", 1024));

        _unitOfWork.PersonsRepository.GetByIdAsync(1, false, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(person);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.PersonsRepository.Received(1).GetByIdAsync(1, false, cancellationToken: Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        Directory.Exists(uploadPath).Should().BeTrue();
    }

    [Fact]
    public async Task UploadPersonImageCommandHandler_Should_Delete_Old_Image_If_Exists()
    {
        // Arrange
        var uploadPath = Path.Combine(_tempDirectory, "uploads");
        Directory.CreateDirectory(uploadPath);

        var oldImagePath = Path.Combine(uploadPath, "old.jpg");
        await File.WriteAllTextAsync(oldImagePath, "Old image content");

        var person = _fixture.Build<Person>()
            .With(x => x.PicturePath, "/uploads/old.jpg")
            .Create();

        var command = new UploadPersonImageCommand(1, CreateMockFile("new.jpg", 1024));

        _unitOfWork.PersonsRepository.GetByIdAsync(1, false, cancellationToken: Arg.Any<CancellationToken>())
            .Returns(person);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.PersonsRepository.Received(1).GetByIdAsync(1, false, cancellationToken: Arg.Any<CancellationToken>());
        await _unitOfWork.PersonsRepository.Received(1).UpdateAsync(person, false, cancellationToken: Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        File.Exists(oldImagePath).Should().BeFalse();
    }

    [Fact]
    public async Task UploadPersonImageCommandHandler_Should_Return_Failure_When_Person_Not_Found()
    {
        // Arrange
        var command = new UploadPersonImageCommand(1, CreateMockFile("test.jpg", 1024));

        _unitOfWork.PersonsRepository.GetByIdAsync(1, false, cancellationToken: Arg.Any<CancellationToken>())
            .Returns((Person?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.PersonsRepository.Received(1).GetByIdAsync(1, false, cancellationToken: Arg.Any<CancellationToken>());
        await _unitOfWork.PersonsRepository.Received(0).UpdateAsync(Arg.Any<Person>(), false, cancellationToken: Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().BeEquivalentTo(Error.NotFound);
    }

    private static IFormFile CreateMockFile(string fileName, long size)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Mock file content"));
        return new FormFile(stream, 0, size, "Data", fileName);
    }
}