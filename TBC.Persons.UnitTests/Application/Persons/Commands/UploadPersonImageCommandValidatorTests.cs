using System.Text;
using AutoFixture;
using AutoFixture.Kernel;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using NSubstitute;
using TBC.Persons.Application.Features.Commands.Persons.UploadImage;

namespace TBC.Persons.UnitTests.Application.Persons.Commands;

public class UploadPersonImageCommandValidatorTests
{
    private readonly UploadPersonImageCommandValidator _validator;
    private readonly Fixture _fixture = new();

    public UploadPersonImageCommandValidatorTests()
    {
        var localizer = Substitute.For<IStringLocalizer<UploadPersonImageCommand>>();

        localizer[Arg.Any<string>()].Returns(callInfo =>
        {
            string key = callInfo.Arg<string>();
            return new LocalizedString(key, $"{key} translated");
        });

        localizer["file too large maxSize is : {0} bytes", Arg.Any<object>()]
            .Returns(callInfo =>
            {
                var size = callInfo.ArgAt<object>(1);
                return new LocalizedString("file too large", $"file too large maxSize is : {size} bytes");
            });

        _validator = new UploadPersonImageCommandValidator(localizer);
        _fixture.Customizations.Add(new TypeRelay(typeof(IFormFile), typeof(FormFile)));

    }

    [Theory]
    [InlineData("test.txt")]
    [InlineData("test.pdf")]
    [InlineData("test.exe")]
    public void UploadPersonImageCommandValidator_Should_Have_Error_When_File_Has_Invalid_Extension(string fileName)
    {
        var file = CreateMockFile(fileName, 1024);
        var command = new UploadPersonImageCommand(1, file);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Image);
    }

    [Fact]
    public void UploadPersonImageCommandValidator_Have_Error_When_File_Exceeds_Max_Size()
    {
        var file = CreateMockFile("test.jpg", 6 * 1024 * 1024);
        var command = new UploadPersonImageCommand(1, file);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Image);
    }

    [Theory]
    [InlineData("test.jpg")]
    [InlineData("test.jpeg")]
    [InlineData("test.png")]
    [InlineData("test.gif")]
    public void UploadPersonImageCommandValidator_Should_Pass_When_File_Is_Valid(string fileName)
    {
        var file = CreateMockFile(fileName, 1024 * 1024);
        var command = new UploadPersonImageCommand(1, file);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static IFormFile CreateMockFile(string fileName, long size)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("Mock file content"));
        return new FormFile(stream, 0, size, "Data", fileName);
    }
}