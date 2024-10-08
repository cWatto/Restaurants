﻿using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishes;

public class DeleteDishesForRestaurantCommandHandler(
    ILogger<DeleteDishesForRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Clearing all dishes for Restaurant with ID {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.FindAsync(request.RestaurantId);

        if (restaurant == null) throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
        {
            throw new ForbidException();
        }

        await dishesRepository.Delete(restaurant.Dishes);
        await restaurantsRepository.SaveChanges();
    }
}
