using AutoFixture;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using NSubstitute;
using TBC.Persons.Application.Features.Commands.Persons.PutRelatedPerson;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.UnitTests.Application.Persons.Commands
{
    public class PutRelatedPersonsCommandValidatorTests
    {
        private readonly PutRelatedPersonsCommandValidator _validator;
        private readonly Fixture _fixture = new();

        public PutRelatedPersonsCommandValidatorTests()
        {
            var localizer = Substitute.For<IStringLocalizer<PutRelatedPersonCommand>>();

            localizer[Arg.Any<string>()].Returns(callInfo =>
            {
                string key = callInfo.Arg<string>();
                return new LocalizedString(key, key + " - default message");
            });

            _validator = new PutRelatedPersonsCommandValidator(localizer);
        }

        [Fact]
        public void PutRelatedPersonsCommandValidator_Should_Have_Error_When_RelatedPersonId_Is_Null()
        {
            var command = _fixture.Build<PutRelatedPersonCommand>()
                .With(x => x.RelatedPersonId, 0)
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.RelatedPersonId);
        }

        [Fact]
        public void PutRelatedPersonsCommandValidator_Have_Error_When_RelationType_Is_Invalid()
        {
            var command = _fixture.Build<PutRelatedPersonCommand>()
                .With(x => x.RelationType, (RelationType)999)
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.RelationType);
        }

        [Fact]
        public void PutRelatedPersonsCommandValidator_Pass_When_Command_Is_Valid()
        {
            var command = _fixture.Build<PutRelatedPersonCommand>()
                .With(x => x.Id, 1)
                .With(x => x.RelatedPersonId, 2)
                .With(x => x.RelationType, RelationType.Acquaintance)
                .Create();

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
