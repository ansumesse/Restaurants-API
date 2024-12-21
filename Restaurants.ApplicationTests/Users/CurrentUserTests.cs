using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Users.Tests
{
    public class CurrentUserTests
    {
        // TestMethod_Scenario_ExpectResult
        [Fact()]
        public void IsInRolTest_WithMatchingRule_ShouldReturnTrue()
        {
            // Arrange
            var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoles.Admin, UserRoles.Owner], null, null);

            // Act
            var isInRole = currentUser.IsInRole(UserRoles.Admin);

            // Assert
            isInRole.Should().BeTrue();
        }

        [Theory()]
        [InlineData(UserRoles.Admin)]
        [InlineData(UserRoles.Owner)]
        public void IsInRolTest_WithMatchingRules_ShouldReturnTrue(string role)
        {
            // Arrange
            var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoles.Admin, UserRoles.Owner], null, null);

            // Act
            var isInRole = currentUser.IsInRole(role);

            // Assert
            isInRole.Should().BeTrue();
        }

        [Fact()]
        public void IsInRolTest_WithNoMatchingRule_ShouldReturnFalse()
        {
            // Arrange
            var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoles.Admin, UserRoles.Owner], null, null);

            // Act
            var isInRole = currentUser.IsInRole(UserRoles.User);

            // Assert
            isInRole.Should().BeFalse();
        }

        [Fact()]
        public void IsInRolTest_WithNoMatchingRuleCase_ShouldReturnFalse()
        {
            // Arrange
            var currentUser = new CurrentUser("1", "test@gmail.com", [UserRoles.Admin, UserRoles.Owner], null, null);

            // Act
            var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

            // Assert
            isInRole.Should().BeFalse();
        }
    }
}