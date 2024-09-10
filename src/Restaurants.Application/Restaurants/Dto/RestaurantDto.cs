using Restaurants.Application.Dishes.Dto;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dto;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }

    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? LogoSasUrl { get; set; }

    public List<DishDto> Dishes { get; set; } = [];


    public static RestaurantDto? FromEntity(Restaurant? restaurant)
    {
        if (restaurant == null) return null;

        var restaurantDto = new RestaurantDto()
        {
            Category = restaurant.Category,
            Description = restaurant.Description,
            Id = restaurant.Id,
            HasDelivery = restaurant.HasDelivery,
            Name = restaurant.Name,
            City = restaurant.Address?.City,
            Street = restaurant.Address?.Street,
            PostalCode = restaurant.Address?.PostalCode,
            Dishes = restaurant.Dishes.Select(DishDto.FromEntity).ToList()
        };

        return restaurantDto;
    }
}
