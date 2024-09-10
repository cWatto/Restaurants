using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests
{
    public class CreatedMultipleRestaurantsRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHascreatedMultipleRestaurants_ShouldSucceed()
        {
            var currentUser = new CurrentUser("1", "test@email.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(m => m.GetCurrentUser())
                .Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2",
                }
            };

            var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantsRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantsRequirement(2);
            var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantsRepositoryMock.Object, userContextMock.Object);

            var context = new AuthorizationHandlerContext([requirement], null, null);

            await handler.HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }

        [Fact()]
        public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
        {
            var currentUser = new CurrentUser("1", "test@email.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(m => m.GetCurrentUser())
                .Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id,
                },
                new()
                {
                    OwnerId = "2",
                }
            };

            var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantsRepositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(restaurants);

            var requirement = new CreatedMultipleRestaurantsRequirement(2);
            var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantsRepositoryMock.Object, userContextMock.Object);

            var context = new AuthorizationHandlerContext([requirement], null, null);

            await handler.HandleAsync(context);

            context.HasSucceeded.Should().BeFalse();
            context.HasFailed.Should().BeTrue();
        }
    }
}