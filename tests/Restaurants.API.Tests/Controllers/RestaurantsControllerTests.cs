using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dto;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Controllers.Tests
{
    public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock = new();

        public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                                                    _ => _restaurantRepositoryMock.Object));
                });
            });
        }

        [Fact()]
        public async void GetAll_ForValidRequest_Returns200Ok()
        {
            var client = _factory.CreateClient();
            var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact()]
        public async void GetAll_ForInvalidRequest_Returns400BadRequest()
        {
            var client = _factory.CreateClient();
            var result = await client.GetAsync("/api/restaurants");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact()]
        public async void Get_ForNonExistingId_Returns404NotFound()
        {
            var id = 1123;

            _restaurantRepositoryMock.Setup(m => m.FindAsync(id))
                                     .ReturnsAsync((Restaurant?)null);

            var client = _factory.CreateClient();
            var result = await client.GetAsync($"/api/restaurants/{id}");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact()]
        public async void Get_ForExistingId_Returns200Ok()
        {
            var id = 1250;
            var restaurant = new Restaurant()
            {
                Id = id,
                Name = "Test",
                Description = "Test description",
            };

            _restaurantRepositoryMock.Setup(m => m.FindAsync(id))
                                     .ReturnsAsync(restaurant);

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/api/restaurants/{id}");

            var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

            restaurantDto.Should().NotBeNull();
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Description.Should().Be(restaurant.Description);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

    }
}