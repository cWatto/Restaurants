using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ThrowsNoErrors()
    {
        var logger = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        var mapper = new Mock<IMapper>();

        var restaurantRepo = new Mock<IRestaurantsRepository>();

        var restaurant = new Restaurant
        {
            Id = 1,
        };
        restaurantRepo.Setup(repo => repo.FindAsync(It.IsAny<int>()))
            .ReturnsAsync(restaurant);

        restaurantRepo.Setup(repo => repo.SaveChanges())
            .Returns(Task.CompletedTask);

        var authService = new Mock<IRestaurantAuthorizationService>();
        authService.Setup(s => s.Authorize(restaurant, It.IsAny<ResourceOperation>()))
            .Returns(true);

        var commandHandler = new UpdateRestaurantCommandHandler(
            logger.Object,
            mapper.Object,
            restaurantRepo.Object,
            authService.Object);

        var command = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Updated name",
            Description = "Updated description",
            HasDelivery = true,
        };

        await commandHandler.Handle(command, CancellationToken.None);


        restaurantRepo.Verify(r => r.FindAsync(command.Id), Times.Once);
        restaurantRepo.Verify(r => r.SaveChanges(), Times.Once);

    }

    [Fact()]
    public void Handle_ForInvalidRestaurantId_ThrowsNotFoundException()
    {
        var logger = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        var mapper = new Mock<IMapper>();

        var restaurantRepo = new Mock<IRestaurantsRepository>();

        restaurantRepo.Setup(repo => repo.FindAsync(It.IsAny<int>()))
            .ReturnsAsync((Restaurant)null);

        var authService = new Mock<IRestaurantAuthorizationService>();

        var commandHandler = new UpdateRestaurantCommandHandler(
            logger.Object,
            mapper.Object,
            restaurantRepo.Object,
            authService.Object);

        var command = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Updated name",
            Description = "Updated description",
            HasDelivery = true,
        };

        Action action = () => commandHandler.Handle(command, CancellationToken.None).Wait();

        action.Should()
            .Throw<NotFoundException>();
    }


    [Fact()]
    public async Task Handle_ForValidCommandWitoutAuthorization_ThrowsForbiddenException()
    {
        var logger = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        var mapper = new Mock<IMapper>();

        var restaurantRepo = new Mock<IRestaurantsRepository>();

        var restaurant = new Restaurant
        {
            Id = 1,
        };
        restaurantRepo.Setup(repo => repo.FindAsync(It.IsAny<int>()))
            .ReturnsAsync(restaurant);

        restaurantRepo.Setup(repo => repo.SaveChanges())
            .Returns(Task.CompletedTask);

        var authService = new Mock<IRestaurantAuthorizationService>();
        authService.Setup(s => s.Authorize(restaurant, It.IsAny<ResourceOperation>()))
            .Returns(false);

        var commandHandler = new UpdateRestaurantCommandHandler(
            logger.Object,
            mapper.Object,
            restaurantRepo.Object,
            authService.Object);

        var command = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Updated name",
            Description = "Updated description",
            HasDelivery = true,
        };

        Action action = () => commandHandler.Handle(command, CancellationToken.None).Wait();

        action.Should()
            .Throw<ForbidException>();

    }
}