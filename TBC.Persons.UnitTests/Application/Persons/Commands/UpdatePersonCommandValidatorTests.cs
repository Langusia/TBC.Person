using AutoFixture;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using NSubstitute;
using TBC.Persons.Application.Features.Commands.Persons.Update;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.UnitTests.Application.Persons.Commands
{
    public class UpdatePersonCommandValidatorTests
    {
        private readonly UpdatePersonCommandValidator _validator;
        private readonly Fixture _fixture = new();

        public UpdatePersonCommandValidatorTests()
        {
            var localizer = Substitute.For<IStringLocalizer<UpdatePersonCommand>>();

            localizer[Arg.Any<string>()].Returns(callInfo =>
            {
                string key = callInfo.Arg<string>();
                return new LocalizedString(key, key + " - default message");
            });

            _validator = new UpdatePersonCommandValidator(localizer);
        }

        public static IEnumerable<object[]> InvalidLengthTestData()
        {
            yield return ["ა"];
            yield return [new string('ა', 51)];
        }

        [Theory]
        [MemberData(nameof(InvalidLengthTestData))]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_FirstName_Is_Invalid_Length(string firstName)
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.FirstName, firstName)
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_FirstName_Contains_NonGeorgian_Letters()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.FirstName, "Giorgi")
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_LastName_Contains_NonGeorgian_Letters()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.LastName, "Giorgadze")
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_FirstNameEng_Contains_NonLatin_Letters()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.FirstNameEng, "გიორგი")
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstNameEng);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_LastNameEng_Contains_NonLatin_Letters()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.LastNameEng, "გიორგაძე")
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.LastNameEng);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_PersonalNumber_Is_Not_11_Digits()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.PersonalNumber, "12345")
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.PersonalNumber);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_Gender_Is_Invalid()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.Gender, (Gender)999)
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Gender);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_CityId_Is_Negative()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.CityId, -1)
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.CityId);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_DateOfBirth_Is_In_Future()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.DateOfBirth, DateTime.UtcNow.AddDays(1))
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Have_Error_When_DateOfBirth_Is_Too_Old()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-100))
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Fact]
        public void UpdatePersonCommandValidator_Should_Pass_When_Command_Is_Valid()
        {
            var command = _fixture.Build<UpdatePersonCommand>()
                .With(x => x.Id, 1)
                .With(x => x.FirstName, "გიორგი")
                .With(x => x.LastName, "გიორგაძე")
                .With(x => x.FirstNameEng, "Giorgi")
                .With(x => x.LastNameEng, "Giorgadze")
                .With(x => x.PersonalNumber, "12345678901")
                .With(x => x.Gender, Gender.Male)
                .With(x => x.CityId, 1)
                .With(x => x.DateOfBirth, DateTime.UtcNow.AddYears(-30))
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}