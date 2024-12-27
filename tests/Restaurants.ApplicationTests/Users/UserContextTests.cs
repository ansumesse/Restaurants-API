using Xunit;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Restaurants.Domain.Constants;
using FluentAssertions;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUserTest_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var birthOfDate = new DateOnly(2003, 5, 1);
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, UserRoles.Admin),
                new Claim(ClaimTypes.Role, UserRoles.User),
                new Claim(AppClaimTypes.Nationality, "German"),
                new Claim(AppClaimTypes.DateOfBirth, birthOfDate.ToString("yyyy-MM-dd"))
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "Test"));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = user
            });
            var userContext = new UserContext(httpContextAccessorMock.Object);
            // Act
            var currentUser = userContext.GetCurrentUser();

            // Assert
            currentUser.Should().NotBeNull();
            currentUser.Id.Equals("1");
            currentUser.Email.Equals("test@test.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
            currentUser.Nationality.Equals("Egyptian");
            currentUser.DateOfBirth.Equals(birthOfDate);
        }
        [Fact()]
        public void GetCurrentUserTest_WithNoUser_ThrowsInvalidOperationException()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
            var userContext = new UserContext(httpContextAccessorMock.Object);
            // Act
            Action action =() => userContext.GetCurrentUser();

            // Assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("User context is not present");
        }
        [Fact()]
        public void GetCurrentUserTest_WithNotAuthenticatedUser_ShouldReturnNull()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var birthOfDate = new DateOnly(2003, 5, 1);
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, UserRoles.Admin),
                new Claim(ClaimTypes.Role, UserRoles.User),
                new Claim(AppClaimTypes.Nationality, "German"),
                new Claim(AppClaimTypes.DateOfBirth, birthOfDate.ToString("yyyy-MM-dd"))
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(userClaims));

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = user
            });
            var userContext = new UserContext(httpContextAccessorMock.Object);
            // Act
            var currentUser = userContext.GetCurrentUser();

            // Assert
            currentUser.Should().BeNull();
        }
        [Fact()]
        public void GetCurrentUserTest_WithUserHasNoClaims_ShouldReturnNull()
        {
            // Arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var user = new ClaimsPrincipal();

            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = user
            });
            var userContext = new UserContext(httpContextAccessorMock.Object);
            // Act
            var currentUser = userContext.GetCurrentUser();

            // Assert
            currentUser.Should().BeNull();
        }

    }
}