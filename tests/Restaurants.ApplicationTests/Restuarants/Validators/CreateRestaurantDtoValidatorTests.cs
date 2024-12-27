using Xunit;
using Restaurants.Application.Restuarants.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurants.Application.Restuarants.Commands.CreateRestaurant;
using FluentValidation.TestHelper;

namespace Restaurants.Application.Restuarants.Validators.Tests
{
    public class CreateRestaurantDtoValidatorTests
    {
        [Fact()]
        public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var validator = new CreateRestaurantDtoValidator();

            var command = new CreateRestaurantCommand()
            {
                Name = "Test",
                Description = "test",
                Category = "Italian",
                ContactEmail = "test@test.com",
                PostalCode = "22-345"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact()]
        public void Validator_ForInValidCommand_ShouldHaveValidationErrors()
        {
            // Arrange
            var validator = new CreateRestaurantDtoValidator();

            var command = new CreateRestaurantCommand()
            {
                Name = "Te",
                Description = "",
                Category = "Italan",
                ContactEmail = "test.com",
                PostalCode = "22-34s5"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Name);
            result.ShouldHaveValidationErrorFor(r => r.Description);
            result.ShouldHaveValidationErrorFor(r => r.Category);
            result.ShouldHaveValidationErrorFor(r => r.ContactEmail);
            result.ShouldHaveValidationErrorFor(r => r.PostalCode);
        }

        [Theory()]
        [InlineData("Italian")]
        [InlineData("Mexican")]
        [InlineData("Japanese")]
        [InlineData("American")]
        [InlineData("Indian")]
        public void Validator_ForValidCategory_ShouldHaveValidationErrors(string category)
        {
            // Arrange
            var validator = new CreateRestaurantDtoValidator();

            var command = new CreateRestaurantCommand()
            {
                Category = category
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(r => r.Category);
        }
    }
}