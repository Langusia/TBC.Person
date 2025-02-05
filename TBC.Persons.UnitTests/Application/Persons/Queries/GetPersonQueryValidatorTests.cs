using FluentValidation.TestHelper;
using TBC.Persons.Application.Features.Queries.Persons.GetPersonQuery;

namespace TBC.Persons.UnitTests.Application.Persons.Queries
{
    public class GetPersonQueryValidatorTests
    {
        private readonly GetPersonQueryValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_Id_Is_Valid()
        {
            // Arrange
            var query = new GetPersonQuery(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}