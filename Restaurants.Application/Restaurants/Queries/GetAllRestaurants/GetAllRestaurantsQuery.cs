using MediatR;
using Restaurants.Application.Restaurants.Dto;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>
{

}
