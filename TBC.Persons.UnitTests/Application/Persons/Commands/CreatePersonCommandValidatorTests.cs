using AutoFixture;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using NSubstitute;
using TBC.Persons.Application.Features.Commands.Persons.Create;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.UnitTests.Application.Persons.Commands;

public class CreatePersonCommandValidatorTests
{
    private readonly Fixture _fixture = new();
    private readonly CreatePersonCommandValidator _validator;

    public CreatePersonCommandValidatorTests()
    {
        var localizer = Substitute.For<IStringLocalizer<CreatePersonCommand>>();

        localizer[Arg.Any<string>()].Returns(callInfo =>
        {
            string key = callInfo.Arg<string>();
            return new LocalizedString(key, key + "_default");
        });

        _validator = new CreatePersonCommandValidator(localizer);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_FirstName_Is_Empty()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.FirstName, "")
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    public static IEnumerable<object[]> InvalidLengthTestData()
    {
        yield return ["ა"];
        yield return [new string('ა', 51)];
    }
    
    [Theory]
    [MemberData(nameof(InvalidLengthTestData))]
    public void CreatePersonCommandValidator_Should_Have_Error_When_FirstName_Is_Invalid_Length(string firstName)
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.FirstName, firstName)
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_FirstName_Contains_NonGeorgian_Letters()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.FirstName, "Giorgi")
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_LastName_Is_Empty()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.LastName, "")
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Theory]
    [MemberData(nameof(InvalidLengthTestData))]
    public void CreatePersonCommandValidator_Should_Have_Error_When_LastName_Is_Invalid_Length(string lastName)
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.LastName, lastName)
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }
    
    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_LastName_Contains_NonGeorgian_Letters()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.LastName, "Giorgadze")
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_FirstNameEng_Contains_NonLatin_Characters()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.FirstNameEng, "გიორგი")
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FirstNameEng);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_LastNameEng_Contains_NonLatin_Characters()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.LastNameEng, "გიორგაძე")
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.LastNameEng);
    }

    [Fact]
    public void CreatePersonCommandValidator_Have_Error_When_PersonalNumber_Is_Empty()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.PersonalNumber, "")
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PersonalNumber);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_PersonalNumber_Is_Not_Exactly_11_Digits()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.PersonalNumber, "12345")
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PersonalNumber);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_Gender_Is_Invalid()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.Gender, (Gender)999)
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Gender);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_CityId_Is_Zero()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.CityId, 0)
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CityId);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_DateOfBirth_Is_Empty()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.DateOfBirth, default(DateTime))
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_DateOfBirth_Is_In_The_Future()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.DateOfBirth, DateTime.UtcNow.AddDays(1))
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Have_Error_When_DateOfBirth_Is_Too_Old()
    {
        var model = _fixture
            .Build<CreatePersonCommand>()
            .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-100))
            .Create();
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void CreatePersonCommandValidator_Should_Pass_Validation_When_All_Fields_Are_Valid()
    {
        var model = _fixture.Build<CreatePersonCommand>()
            .With(x => x.FirstName, "გიორგი")
            .With(x => x.LastName, "გიორგაძე")
            .With(x => x.FirstNameEng, "Giorgi")
            .With(x => x.LastNameEng, "Giorgadze")
            .With(x => x.PersonalNumber, "12345678901")
            .With(x => x.Gender, Gender.Male)
            .With(x => x.CityId, 1)
            .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
            .Create();

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}