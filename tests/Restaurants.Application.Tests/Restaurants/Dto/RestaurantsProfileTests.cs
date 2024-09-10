using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Restaurants.Dto.Tests;


public class RestaurantsProfileTests
{
    private IMapper _mapper;

    public RestaurantsProfileTests()
    {

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantsProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {


        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "Restaurant Name",
            Description = "Description",
            Category = "Category",
            HasDelivery = true,
            ContactEmail = "ContactEmail@email.com",
            ContactNumber = "0411234533",
            Address = new Address()
            {
                City = "Test City",
                Street = "123 Fake Street",
                PostalCode = "Test"
            }
        };

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {


        var command = new CreateRestaurantCommand()
        {

            Name = "Restaurant Name",
            Description = "Description",
            Category = "Category",
            HasDelivery = true,
            ContactEmail = "ContactEmail@email.com",
            ContactNumber = "0411234533",
            City = "Test City",
            Street = "123 Fake Street",
            PostalCode = "Test"
        };

        var restaurant = _mapper.Map<Restaurant>(command);

        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);
    }


    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        var command = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "Restaurant Name",
            Description = "Description",
            HasDelivery = true,
        };

        var restaurant = _mapper.Map<Restaurant>(command);

        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
    }


}