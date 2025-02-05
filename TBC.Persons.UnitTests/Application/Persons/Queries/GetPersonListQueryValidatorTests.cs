using AutoFixture;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using NSubstitute;
using TBC.Persons.Application.Features.Queries.Persons.GetPersonListQuery;

namespace TBC.Persons.UnitTests.Application.Persons.Queries
{
    public class GetPersonListQueryValidatorTests
    {
        private readonly GetPersonListQueryValidator _validator;
        private readonly Fixture _fixture = new();

        public GetPersonListQueryValidatorTests()
        {
            var localizer = Substitute.For<IStringLocalizer<GetPersonListQuery>>();

            localizer[Arg.Any<string>()].Returns(callInfo =>
            {
                string key = callInfo.Arg<string>();
                return new LocalizedString(key, key + " - default message");
            });

            _validator = new GetPersonListQueryValidator(localizer);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void GetPersonListQueryValidator_Should_Have_Error_When_PageSize_Is_Invalid(int pageSize)
        {
            var query = _fixture.Build<GetPersonListQuery>()
                .With(x => x.pageSize, pageSize)
                .Create();

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.pageSize);
        }

        [Fact]
        public void GetPersonListQueryValidator_Should_Have_Error_When_PageSize_Is_Null()
        {
            var query = _fixture.Build<GetPersonListQuery>()
                .With(q => q.FirstName, (string?)null)
                .With(q => q.LastName, (string?)null)
                .With(q => q.PersonalNumber, (string?)null)
                .With(q => q.pageSize, (int?)null)
                .With(q => q.pageIndex, 1)
                .Create();

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.pageSize);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public void GetPersonListQueryValidator_Should_Have_Error_When_PageIndex_Is_Invalid(int pageIndex)
        {
            var query = _fixture.Build<GetPersonListQuery>()
                .With(x => x.pageIndex, pageIndex)
                .Create();

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.pageIndex);
        }

        [Fact]
        public void GetPersonListQueryValidator_Should_Have_Error_When_PageIndex_Is_Null()
        {
            var query = _fixture.Build<GetPersonListQuery>()
                .With(q => q.FirstName, (string?)null)
                .With(q => q.LastName, (string?)null)
                .With(q => q.PersonalNumber, (string?)null)
                .With(q => q.pageSize, 10)
                .With(q => q.pageIndex, (int?)null)
                .Create();

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.pageIndex);
        }

        [Fact]
        public void GetPersonListQueryValidator_Should_Pass_Validation_When_PageSize_And_PageIndex_Are_Valid()
        {
            var query = _fixture.Build<GetPersonListQuery>()
                .With(x => x.pageSize, 10)
                .With(x => x.pageIndex, 2)
                .Create();

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}