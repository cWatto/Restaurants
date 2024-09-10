using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo
{
    public class UploadRestaurantCommandHandler(ILogger<UploadRestaurantCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand>
    {
        public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating resource logo with id: {RestaurantId}", request.RestaurantId);
            var restaurant = await restaurantsRepository.FindAsync(request.RestaurantId);

            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
            {
                throw new ForbidException();
            }

            var logoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);
            restaurant.LogoUrl = logoUrl;

            await restaurantsRepository.SaveChanges();
        }
    }
}
