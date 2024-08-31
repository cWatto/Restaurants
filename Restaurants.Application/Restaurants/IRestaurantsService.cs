using Restaurants.Application.Restaurants.Dto;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    Task<IEnumerable<RestaurantDto>> GetAllRestaurants();
    Task<RestaurantDto?> GetRestaurant(int id);
    Task<int> Create(CreateRestaurantDto dto);
}